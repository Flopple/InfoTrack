import ky from 'ky';
import { env } from './env.ts';

export const httpClient = ky.create({
  prefixUrl: env.apiUrl,
  headers: {
    'Content-Type': 'application/json',
  },
});
