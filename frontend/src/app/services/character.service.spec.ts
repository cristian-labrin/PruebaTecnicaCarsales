import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { CharacterService } from './character.service';
import { Character } from '../models/character.model';
import { environment } from '../../environments/environment';

describe('CharacterService', () => {
  let service: CharacterService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        CharacterService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(CharacterService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('debería pedir un personaje por su id', () => {
    const mockCharacter: Character = {
      id: 1,
      name: 'Rick Sanchez',
      status: 'Alive',
      species: 'Human',
      gender: 'Male',
      image: 'https://rickandmortyapi.com/api/character/avatar/1.jpeg',
      origin: 'Earth (C-137)',
      location: 'Citadel of Ricks'
    };

    service.getCharacterById(1).subscribe((character) => {
      expect(character.name).toBe('Rick Sanchez');
      expect(character.origin).toBe('Earth (C-137)');
      expect(character.status).toBe('Alive');
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/characters/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockCharacter);
  });
});
