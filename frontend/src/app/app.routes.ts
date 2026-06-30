import { Routes } from '@angular/router';
import { EpisodeList } from './features/episode-list/episode-list';
import { EpisodeDetail } from './features/episode-detail/episode-detail';
import { CharacterDetail } from './features/character-detail/character-detail';

export const routes: Routes = [
  { path: '', component: EpisodeList },
  { path: 'episode/:id', component: EpisodeDetail },
  { path: 'character/:id', component: CharacterDetail },
  { path: '**', redirectTo: '' }
];
