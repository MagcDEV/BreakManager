# Copilot Instructions for Modern Angular Development (C#/.NET Background)

## Overall Goals & Style

* **Modern Angular:** Prioritize features and patterns from Angular v16+ (Standalone APIs, Signals, Typed Forms).
* **TypeScript First:** Leverage TypeScript's strong typing. Avoid `any` type whenever possible. Use interfaces and types extensively (similar to C#). Define clear data contracts.
* **Readability & Maintainability:** Generate clean, well-commented, and organized code. Follow the official Angular Style Guide. Keep functions and files concise (e.g., functions < 75 lines, files < 400 lines).
* **Performance:** Implement code that is performant by default (OnPush, trackBy, lazy loading).
* **Security:** Follow security best practices (sanitization, avoiding risky patterns).
* **Modularity & Reusability:** Design components and services to be reusable and self-contained.

## Specific Instructions

### 1. Components

* **Prefer Standalone Components:** Generate new components as `standalone: true`. Include necessary dependencies (`imports` array) directly in the component decorator.
    * Import `CommonModule` or specific directives/pipes (`NgIf`, `NgFor`, `AsyncPipe`, etc.) as needed, instead of the entire `CommonModule`.
* **`ChangeDetectionStrategy.OnPush`:** Use `OnPush` change detection strategy by default for new components to optimize performance.
* **Smart/Container & Dumb/Presentational Components:** Differentiate between components that manage state/logic (smart) and components purely for display (dumb). Pass data down via `@Input()` and emit events up via `@Output()`.
* **Strongly Typed Inputs/Outputs:** Define clear types for `@Input()` and `@Output()` properties. Use `signal` inputs where appropriate (Angular v17.1+).
* **Lifecycle Hooks:** Implement lifecycle hooks (`OnInit`, `OnDestroy`, etc.) when necessary, ensuring proper cleanup (e.g., unsubscribing observables if not using `async` pipe or `takeUntilDestroyed`).
* **Templates:**
    * Keep template logic minimal. Complex logic should reside in the component class.
    * Use `*ngFor` with `trackBy` for lists to improve rendering performance.
    * Use the `async` pipe (`| async`) to handle observables/promises directly in the template, minimizing manual subscription management.
    * Avoid string concatenation for dynamic HTML. Use safe binding mechanisms like `[innerHTML]` with proper sanitization if absolutely necessary.
    * Use `ng-container` for structural directives (`*ngIf`, `*ngFor`) when you don't need an extra HTML element.

### 2. Services & Dependency Injection

* **Single Responsibility Principle (SRP):** Services should have a clear, single purpose (e.g., data fetching, authentication, logging).
* **Injectable Services:** Provide services using `@Injectable({ providedIn: 'root' })` for tree-shakable, singleton services by default. Consider providing in specific component/module scopes if needed.
* **Use `HttpClient`:** For HTTP requests, use the `HttpClient` module. Define clear interface types for request/response data.
* **Centralize Logic:** Move business logic, data access, and state management logic out of components and into services.

### 3. State Management

* **Signals:** For local component state or simple shared state (via services), prefer Angular Signals (`signal`, `computed`, `effect`).
* **NgRx (or alternatives like NGXS, Akita):** For complex, global application state:
    * If using NgRx, consider `SignalStore` or `SignalState` for a more modern, signal-based approach.
    * Follow standard patterns (Actions, Reducers/Methods, Selectors/Computed, Effects/Methods for side-effects).
    * Keep state immutable.

### 4. Forms

* **Reactive Forms:** Prefer Reactive Forms over Template-Driven Forms for non-trivial forms due to better testability, scalability, and explicit control.
* **Typed Forms:** Always use Strictly Typed Reactive Forms (introduced in Angular v14). Define interfaces for form structures. Use `FormBuilder`, `FormGroup`, `FormControl<T>`, `FormArray`.
* **Validation:** Implement validation logic within the component class using built-in or custom validators. Provide clear user feedback for validation errors.

### 5. Routing

* **Lazy Loading:** Implement lazy loading for feature modules/routes using `loadChildren` (for NgModules) or `loadComponent` (for standalone components/routing configurations) to improve initial load times.
* **Route Guards:** Use route guards (`CanActivate`, `CanDeactivate`, `resolve`) for protecting routes and pre-fetching data. Generate guards as functions (preferred modern approach).
* **Standalone Routing APIs:** Use standalone routing APIs (`provideRouter`, functional guards/resolvers) for new applications or when migrating.

### 6. Modules (Less emphasis with Standalone APIs)

* **AppModule/Bootstrap:** Bootstrap the application using a standalone component (`bootstrapApplication(AppComponent, {providers: [...]})`).
* **Feature Modules (If used):** If still using NgModules, group related components, services, and pipes into feature modules. Keep them focused.
* **Shared Modules (If used):** Create shared modules for commonly used components, directives, and pipes across different feature modules, but prefer importing standalone components directly where needed.

### 7. Coding Style & TypeScript

* **Avoid `any`:** Use specific types, `unknown`, or generics instead.
* **Use `const`:** Prefer `const` over `let` when variables are not reassigned.
* **Immutability:** Treat objects and arrays as immutable, especially when dealing with state management or `@Input` properties with `OnPush`. Use spread syntax (`...`) or libraries like Immer.
* **ESLint:** Adhere to configured ESLint rules.
* **Naming Conventions:** Follow Angular naming conventions (e.g., `my-component.component.ts`, `MyComponent`, `my-service.service.ts`, `MyService`). Lower camelCase for properties/methods.
* **Comments:** Add comments where necessary to explain complex logic or intent (JSDoc style preferred).

### 8. Error Handling

* Implement robust error handling for HTTP requests (e.g., using `catchError` in RxJS pipes) and other asynchronous operations.
* Provide user-friendly error messages.

### 9. Security

* **Sanitization:** Trust Angular's default sanitization. Be cautious with methods like `bypassSecurityTrustHtml`. Understand the risks.
* **Avoid Template Injection:** Never concatenate user input directly into template strings.
* **HTTP Security:** Use `HttpClient` which has built-in XSRF/CSRF protection support (requires server-side setup). Ensure HTTPS is used.
* **Dependencies:** Keep Angular and third-party libraries up-to-date.

### 10. Testing

* Generate unit tests (`.spec.ts`) for components, services, and pipes using TestBed and potentially libraries like `jest` or `spectator`.
* Focus on testing component inputs, outputs, public methods, and template interactions.
* Mock dependencies effectively.

---

**Assistant Note:** When generating code, explain *why* a particular pattern or feature (like Standalone Components or Signals) is being used if it seems relevant to the context. Prioritize the instructions above when suggesting code snippets or refactoring existing code.