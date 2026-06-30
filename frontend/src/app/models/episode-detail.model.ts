import { CharacterSummary } from './character-summary.model';

export interface EpisodeDetail {
  id: number;
  name: string;
  airDate: string;
  episode: string;
  characters: CharacterSummary[];
}
