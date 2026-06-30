import { Component, inject, signal, OnInit, OnDestroy } from '@angular/core';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { EpisodeService } from '../../services/episode.service';
import { Episode } from '../../models/episode.model';
import { PagedResponse } from '../../models/paged-response.model';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-episode-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './episode-list.html',
  styleUrl: './episode-list.css'
})
export class EpisodeList implements OnInit, OnDestroy {
  private readonly episodeService = inject(EpisodeService);
  private readonly searchInput = new Subject<string>();
  private readonly destroy = new Subject<void>();

  protected readonly episodes = signal<Episode[]>([]);
  protected readonly currentPage = signal<number>(1);
  protected readonly totalPages = signal<number>(0);
  protected readonly hasNext = signal<boolean>(false);
  protected readonly hasPrev = signal<boolean>(false);
  protected readonly loading = signal<boolean>(false);
  protected readonly error = signal<string | null>(null);
  protected readonly searchTerm = signal<string>('');

  ngOnInit(): void {
    this.searchInput
      .pipe(debounceTime(400), distinctUntilChanged(), takeUntil(this.destroy))
      .subscribe((term) => {
        this.searchTerm.set(term);
        this.loadEpisodes(1);
      });

    this.loadEpisodes(1);
  }

  ngOnDestroy(): void {
    this.destroy.next();
    this.destroy.complete();
  }

  protected onSearch(value: string): void {
    this.searchInput.next(value);
  }

  protected clearSearch(): void {
      this.searchTerm.set('');
      this.searchInput.next('');
      this.loadEpisodes(1);
    }

  protected loadEpisodes(page: number): void {
    this.loading.set(true);
    this.error.set(null);

    this.episodeService.getEpisodes(page, this.searchTerm()).subscribe({
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
