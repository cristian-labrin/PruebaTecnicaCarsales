import { Component, inject, signal, OnInit } from '@angular/core';
import { EpisodeService } from '../../services/episode.service';
import { Episode } from '../../models/episode.model';
import { PagedResponse } from '../../models/paged-response.model';

@Component({
  selector: 'app-episode-list',
  standalone: true,
  templateUrl: './episode-list.html',
  styleUrl: './episode-list.css'
})
export class EpisodeList implements OnInit {
  private readonly episodeService = inject(EpisodeService);

  protected readonly episodes = signal<Episode[]>([]);
  protected readonly currentPage = signal<number>(1);
  protected readonly totalPages = signal<number>(0);
  protected readonly hasNext = signal<boolean>(false);
  protected readonly hasPrev = signal<boolean>(false);
  protected readonly loading = signal<boolean>(false);
  protected readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.loadEpisodes(1);
  }

  protected loadEpisodes(page: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.episodeService.getEpisodes(page).subscribe({
      next: (response: PagedResponse<Episode>) => {
        this.episodes.set(response.items);
        this.currentPage.set(response.currentPage);
        this.totalPages.set(response.totalPages);
        this.hasNext.set(response.hasNext);
        this.hasPrev.set(response.hasPrev);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No pudimos cargar los episodios. Intenta nuevamente.');
        this.loading.set(false);
      }
    });
  }

  protected goToPage(page: number): void {
    if (page < 1 || page > this.totalPages()) {
      return;
    }
    this.loadEpisodes(page);
  }
}
