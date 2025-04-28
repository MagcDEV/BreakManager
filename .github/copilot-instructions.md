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

# Copilot Instructions for Modern .NET Development (Targeting .NET 9+)

## Overall Goals & Style

* **Target .NET 9:** Prioritize features, APIs, and patterns available in .NET 8 and expected/previewed for .NET 9. Utilize the latest C# language versions (C# 12/13+).
* **Clean & Idiomatic C#:** Write clean, readable, and maintainable C# code. Follow Microsoft's C# Coding Conventions and Framework Design Guidelines.
* **Performance:** Leverage performance improvements in .NET. Use `async`/`await` correctly, optimize data access (EF Core), and utilize performance-focused APIs where appropriate (e.g., `Span<T>`, `System.Text.Json`).
* **Security:** Implement security best practices (authentication, authorization, data protection, input validation, secrets management).
* **SOLID Principles:** Adhere to SOLID principles, especially Single Responsibility Principle (SRP) and Dependency Inversion Principle (DIP) through Dependency Injection (DI).
* **Asynchronous Everywhere:** Prefer asynchronous operations (`async`/`await`) for I/O-bound tasks (database access, HTTP calls) to ensure scalability.

## Specific Instructions

### 1. C# Language Features (Latest Versions)

* **Immutability:** Prefer immutable data structures. Use `record` types (positional or nominal) for DTOs and entities where appropriate. Utilize `readonly` members and `init`-only setters.
* **Nullability:** Enable Nullable Reference Types (`#nullable enable`) project-wide. Handle potential nulls explicitly using null-conditional operators (`?.`), null-coalescing operators (`??`), and appropriate checks. Avoid the null-forgiving operator (`!`) unless absolutely necessary and justified.
* **Pattern Matching:** Leverage pattern matching (type patterns, property patterns, logical patterns, list patterns) for cleaner conditional logic and data extraction.
* **LINQ:** Use LINQ effectively for querying collections and data sources. Be mindful of deferred execution and query performance, especially with EF Core (prefer database-side evaluation).
* **Expression-bodied Members:** Use for concise single-line methods, properties, and constructors.
* **File-Scoped Namespaces:** Use `namespace MyNamespace;` for cleaner code files.
* **Primary Constructors:** Utilize primary constructors (C# 12+) for cleaner class definitions, especially in ASP.NET Core controllers, services, and record types, to reduce boilerplate for constructor injection.
* **Collection Expressions:** Use collection expressions (`[...]`) (C# 12+) for creating arrays, lists, spans, etc., where applicable.
* **`using` Declarations:** Prefer `using` declarations (`using var resource = ...;`) over `using` blocks for better readability when scope allows.
* **String Interpolation:** Use interpolated strings (`$""`) over `string.Format` or concatenation.

### 2. ASP.NET Core (Web API Focus)

* **Minimal APIs:** Prefer Minimal APIs for new HTTP endpoints for conciseness and performance. Use `MapGroup` to organize related endpoints. Fallback to MVC Controllers if complex view rendering or advanced filter pipelines are strictly needed (less common for SPA backends).
* **Dependency Injection (DI):** Rely heavily on the built-in DI container.
    * Register services with appropriate lifetimes (`Singleton`, `Scoped`, `Transient`). Use `Scoped` for `DbContext` and services that depend on it within a request.
    * Use constructor injection primarily. Avoid property injection and the Service Locator anti-pattern.
* **Configuration:** Use the `IConfiguration` interface and the Options pattern (`IOptions<T>`, `IOptionsSnapshot<T>`, `IOptionsMonitor<T>`) to access strongly-typed configuration settings. Bind configuration sections to POCOs/records.
* **Middleware:** Understand the request pipeline. Use built-in middleware effectively (Authentication, Authorization, CORS, Exception Handling, etc.). Create custom middleware for cross-cutting concerns when necessary.
* **Routing:** Use Minimal API conventions (`MapGet`, `MapPost`, etc.) or attribute routing (`[Route]`, `[HttpGet]`, etc.) if using Controllers.
* **Model Binding & Validation:**
    * Leverage automatic model binding from request bodies, route parameters, query strings.
    * Use Data Annotations (`[Required]`, `[MaxLength]`, etc.) or libraries like `FluentValidation` for input validation.
    * Return standard `Results.ValidationProblem` or `ValidationProblemDetails` for validation failures in APIs.
* **Logging:** Use `ILogger<T>` obtained via DI for logging. Avoid `Console.WriteLine`. Configure logging providers (e.g., Console, Serilog, Application Insights) and use structured logging.

### 3. Data Access (Entity Framework Core)

* **Use EF Core:** Default choice for relational database access.
* **Async Operations:** Always use asynchronous EF Core methods (`SaveChangesAsync`, `ToListAsync`, `FirstOrDefaultAsync`, etc.) in async controller actions or services.
* **DbContext Lifetime:** Register `DbContext` as `Scoped` in DI. Inject `DbContext` into services, not directly into controllers (unless very simple).
* **Query Performance:**
    * Be mindful of LINQ-to-SQL translation. Avoid operations that cause client-side evaluation on large datasets.
    * Use `AsNoTracking()` or `AsNoTrackingWithIdentityResolution()` for read-only queries to improve performance.
    * Prevent N+1 query problems using `Include()`/`ThenInclude()` (use judiciously) or explicit projections (`Select()`).
    * Consider compiled queries for highly performance-sensitive paths.
* **Migrations:** Use EF Core Migrations to manage database schema changes.
* **Concurrency Control:** Implement optimistic concurrency control using concurrency tokens where necessary.

### 4. Asynchronous Programming (`async`/`await`)

* **Use `async`/`await`:** Use `async` and `await` for all I/O-bound operations (database, network calls, file system access).
* **Avoid `async void`:** Avoid `async void` methods, except for event handlers. Prefer `async Task` or `async Task<T>`.
* **`ConfigureAwait(false)`:** Generally *not* required in ASP.NET Core application code (endpoint handlers, services) due to the absence of a specific synchronization context. It remains important in general-purpose library code.
* **Cancellation Tokens:** Propagate `CancellationToken` through async call chains, especially in long-running operations or requests that can be cancelled by the client.

### 5. Error Handling & Resilience

* **Global Exception Handling:** Implement global exception handling using middleware (`UseExceptionHandler`) to catch unhandled exceptions and return standardized error responses (e.g., `ProblemDetails`).
* **Specific Exceptions:** Catch specific exceptions where meaningful recovery or alternative logic is possible. Avoid catching base `Exception`.
* **Result Pattern:** Consider using a Result pattern (custom or libraries like `FluentResults`) for operations that can fail in predictable ways (e.g., validation errors, resource not found), instead of throwing exceptions for control flow.
* **Resilience:** Implement resilience patterns (retries, circuit breakers) using libraries like `Polly` for calls to external services.

### 6. Security

* **Authentication:** Implement authentication using ASP.NET Core Identity (for user accounts) or JWT Bearer authentication (common for SPAs) or other relevant schemes.
* **Authorization:** Use policies, roles, and attributes (`[Authorize]`) to control access to endpoints and resources.
* **Secrets Management:** Use User Secrets (development), Environment Variables, Azure Key Vault, or other secure configuration providers. **Never** store secrets in source code or configuration files directly.
* **HTTPS:** Enforce HTTPS using HSTS middleware (`UseHsts()`).
* **CORS:** Configure Cross-Origin Resource Sharing (CORS) correctly and restrictively using `UseCors()`.
* **Input Validation:** Already covered, but crucial for security (preventing injection attacks).
* **Data Protection:** Use ASP.NET Core Data Protection APIs for encrypting sensitive data at rest if needed.
* **Rate Limiting:** Implement rate limiting middleware (`UseRateLimiter`) to prevent abuse.

### 7. Testing

* **Unit Tests:** Write unit tests using frameworks like xUnit, NUnit, or MSTest. Mock dependencies using libraries like Moq or NSubstitute. Focus on testing business logic in isolation.
* **Integration Tests:** Use `WebApplicationFactory<T>` for in-memory integration testing of the API pipeline, including middleware, routing, binding, and interaction with test doubles (e.g., in-memory database, mocked external services). Test containers (e.g., `Testcontainers.NET`) can be used for testing against real dependencies like databases.
* **Testable Code:** Write code with DI and clear interfaces/abstractions to facilitate testing.

### 8. Code Style & Structure

* **Project Structure:** Organize projects logically (e.g., separate projects for Domain, Application, Infrastructure, API/Web).
* **Namespaces:** Use clear and consistent namespaces.
* **.NET Analyzers:** Enable and configure .NET code analyzers (Roslyn Analyzers). Address warnings and suggestions to maintain code quality and adopt best practices.
* **XML Documentation Comments:** Use `///` comments for public types and members to explain their purpose, parameters, and return values.

---

**Assistant Note:** When generating .NET code, explain *why* a particular modern C# feature or .NET pattern (like Minimal APIs, Records, Primary Constructors, `AsNoTracking`, Result Pattern) is being used, especially if it contrasts with older practices. Prioritize the instructions above when suggesting code snippets or refactoring existing .NET code. Target .NET 9 idioms and C# 12/13+ features where applicable.