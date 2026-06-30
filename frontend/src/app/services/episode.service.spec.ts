import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { EpisodeService } from './episode.service';
import { PagedResponse } from '../models/paged-response.model';
import { Episode } from '../models/episode.model';
import { environment } from '../../environments/environment';

describe('EpisodeService', () => {
  let service: EpisodeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        EpisodeService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(EpisodeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('debería pedir los episodios de la página indicada', () => {
    const mockResponse: PagedResponse<Episode> = {
      items: [
        { id: 1, name: 'Pilot', airDate: 'December 2, 2013', episode: 'S01E01' }
      ],
      currentPage: 1,
      totalPages: 3,
      totalItems: 51,
      hasNext: true,
      hasPrev: false
    };

    service.getEpisodes(1).subscribe((response) => {
      expect(response.items.length).toBe(1);
      expect(response.items[0].name).toBe('Pilot');
      expect(response.totalPages).toBe(3);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/episodes?page=1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('debería incluir el filtro de nombre en la petición', () => {
    service.getEpisodes(1, 'rick').subscribe();

    const req = httpMock.expectOne(
      `${environment.apiUrl}/episodes?page=1&name=rick`
    );
    expect(req.request.method).toBe('GET');
    req.flush({
      items: [], currentPage: 1, totalPages: 0,
      totalItems: 0, hasNext: false, hasPrev: false
    });
  });
});
