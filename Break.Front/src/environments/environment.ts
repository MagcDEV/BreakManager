/**
 * Configuration for the development environment.
 */
export const environment = {
  production: false,
  /**
   * The base URL for the Break.Api backend in the development environment.
   * This should match the HTTPS URL specified in the API's launchSettings.json.
   * It should be the base address (e.g., 'https://localhost:7267'),
   * *without* the '/api' suffix.
   */
  apiUrl: 'https://localhost:7267' // Updated to match launchSettings.json
};