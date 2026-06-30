import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Episode } from '../models/episode.model';
import { PagedResponse } from '../models/paged-response.model';
import { EpisodeDetail } from '../models/episode-detail.model';

@Injectable({ providedIn: 'root' })
export class EpisodeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/episodes`;

  getEpisodes(page: number, name?: string): Observable<PagedResponse<Episode>> {
    let params = new HttpParams().set('page', page);
    if (name) {
      params = params.set('name', name);
    }
    return this.http.get<PagedResponse<Episode>>(this.baseUrl, { params });
  }

  getEpisodeById(id: number): Observable<EpisodeDetail> {
    return this.http.get<EpisodeDetail>(`${this.baseUrl}/${id}`);
  }
}
