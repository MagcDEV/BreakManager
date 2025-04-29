/**
 * Interface matching the backend Break.Contracts.Requests.LoginRequest
 */
export interface LoginRequest {
  username: string;
  password: string;
}

/**
 * Interface matching the backend Break.Contracts.Requests.RegisterUserRequest
 * based on the openapi.json definition.
 */
export interface RegisterUserRequest {
  username: string;
  email: string;
  password: string;
  /**
   * Roles to assign to the user.
   * Based on openapi.json, this is required but nullable.
   * For self-registration, sending an empty array [] is typical.
   */
  roles: string[] | null; // Array of strings or null
}