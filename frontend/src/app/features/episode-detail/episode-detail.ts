import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { EpisodeService } from '../../services/episode.service';
import { EpisodeDetail as EpisodeDetailModel } from '../../models/episode-detail.model';

@Component({
  selector: 'app-episode-detail',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './episode-detail.html',
  styleUrl: './episode-detail.css'
})
export class EpisodeDetail implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly episodeService = inject(EpisodeService);

  protected readonly episode = signal<EpisodeDetailModel | null>(null);
  protected readonly loading = signal<boolean>(false);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadEpisode(id);
  }

  protected loadEpisode(id: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.episodeService.getEpisodeById(id).subscribe({
      next: (data) => {
        this.episode.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No pudimos cargar el episodio. Intenta nuevamente.');
        this.loading.set(false);
      }
    });
  }
}
