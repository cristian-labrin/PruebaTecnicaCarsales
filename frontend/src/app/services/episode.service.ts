import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Episode } from '../models/episode.model';
import { PagedResponse } from '../models/paged-response.model';

@Injectable({ providedIn: 'root' })
export class EpisodeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/episodes`;

  getEpisodes(page: number): Observable<PagedResponse<Episode>> {
    return this.http.get<PagedResponse<Episode>>(`${this.baseUrl}?page=${page}`);
  }
}
