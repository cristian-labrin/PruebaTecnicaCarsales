import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { EpisodeList } from './episode-list';
import { PagedResponse } from '../../models/paged-response.model';
import { Episode } from '../../models/episode.model';
import { environment } from '../../../environments/environment';

describe('EpisodeList', () => {
  let fixture: ComponentFixture<EpisodeList>;
  let httpMock: HttpTestingController;

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

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EpisodeList],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(EpisodeList);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('debería cargar episodios al iniciar', () => {
    fixture.detectChanges(); // dispara ngOnInit

    const req = httpMock.expectOne(`${environment.apiUrl}/episodes?page=1`);
    req.flush(mockResponse);
    fixture.detectChanges();

    const cards = fixture.nativeElement.querySelectorAll('.card');
    expect(cards.length).toBe(1);
    expect(fixture.nativeElement.textContent).toContain('Pilot');
  });

  it('debería mostrar el componente de error si la carga falla', () => {
    fixture.detectChanges();

    const req = httpMock.expectOne(`${environment.apiUrl}/episodes?page=1`);
    req.flush('Error', { status: 500, statusText: 'Server Error' });
    fixture.detectChanges();

    const errorEl = fixture.nativeElement.querySelector('app-error-message');
    expect(errorEl).not.toBeNull();
  });

  it('debería cargar la página siguiente al navegar', () => {
    fixture.detectChanges();
    httpMock.expectOne(`${environment.apiUrl}/episodes?page=1`).flush(mockResponse);
    fixture.detectChanges();

    // Buscar el botón "Siguiente" y hacer click
    const buttons = fixture.nativeElement.querySelectorAll('.btn');
    const nextButton = Array.from(buttons).find(
      (b: any) => b.textContent.trim() === 'Siguiente'
    ) as HTMLButtonElement;
    nextButton.click();

    const req2 = httpMock.expectOne(`${environment.apiUrl}/episodes?page=2`);
    req2.flush({ ...mockResponse, currentPage: 2, hasPrev: true });
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('Página 2 de 3');
  });

  it('debería buscar con debounce al escribir', async () => {
      fixture.detectChanges();
      httpMock.expectOne(`${environment.apiUrl}/episodes?page=1`).flush(mockResponse);
      fixture.detectChanges();

      const input = fixture.nativeElement.querySelector('.search__input') as HTMLInputElement;
      input.value = 'rick';
      input.dispatchEvent(new Event('input'));

      // Esperamos a que pase el tiempo del debounce (400ms) de forma real
      await new Promise((resolve) => setTimeout(resolve, 450));

      const req = httpMock.expectOne(`${environment.apiUrl}/episodes?page=1&name=rick`);
      req.flush(mockResponse);
      fixture.detectChanges();
    });
});
