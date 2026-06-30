import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CharacterService } from '../../services/character.service';
import { Character } from '../../models/character.model';
import { Location } from '@angular/common';

@Component({
  selector: 'app-character-detail',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './character-detail.html',
  styleUrl: './character-detail.css'
})
export class CharacterDetail implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly characterService = inject(CharacterService);
  private readonly location = inject(Location);

  protected readonly character = signal<Character | null>(null);
  protected readonly loading = signal<boolean>(false);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadCharacter(id);
  }

  protected goBack(): void {
    this.location.back();
  }

  protected loadCharacter(id: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.characterService.getCharacterById(id).subscribe({
      next: (data) => {
        this.character.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No pudimos cargar el personaje. Intenta nuevamente.');
        this.loading.set(false);
      }
    });
  }
}
