import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // Explicitly disable prerendering (use Client rendering) for parameterized product routes
  {
    path: 'products/:id',
    renderMode: RenderMode.Client // Or RenderMode.Blocking if SSR is desired for these
  },
  {
    path: 'products/:id/edit',
    renderMode: RenderMode.Client // Or RenderMode.Blocking
  },
  // Add other parameterized routes here if they exist (e.g., 'sales/:id')

  // The wildcard route will handle all other routes and attempt to prerender them
  {
    path: '**',
    renderMode: RenderMode.Prerender
  }
];