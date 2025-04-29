/**
 * Interface matching the backend response for a successful login.
 * Based on Break.Api.Endpoints.AuthEndpoints login response.
 */
export interface AuthResponse {
  token: string;
}