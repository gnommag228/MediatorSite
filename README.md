# MediatorSite

A full-stack web application built with **C#** and **ASP.NET Core** (Razor Pages), designed for managing client booking requests, appointment scheduling, and automated notifications for a consulting/mediation service.

---

### Features

* **Calendar & Slot Reservation** — booking form with server-side validation that checks for already-taken time slots and rejects double bookings.
* **Telegram Bot Integration** — new booking requests trigger an instant real-time notification to the site administrator via the Telegram Bot API, including a quick-reply WhatsApp link.
* **Admin Portal** — a separate, session-protected admin area for reviewing and managing incoming booking requests.
* **CI/CD Pipeline** — every push to `main` triggers a GitHub Actions workflow that restores, builds, and runs the automated test suite; the Docker build used for deployment on Render also runs the tests again as a second gate before the image is published.

---

### Tech Stack & Architecture

* **Framework:** .NET 9 / ASP.NET Core (Razor Pages)
* **ORM & Database:** Entity Framework Core with SQLite, using code-first migrations applied automatically on startup
* **Integrations:** Telegram Bot API (via `IHttpClientFactory`)
* **Security:** Session-based access control for the admin area; secrets and connection strings supplied via environment variables / configuration, kept out of source control
* **Testing:** xUnit unit tests (`MediatorSite.Tests`)
* **CI/CD:** GitHub Actions (build + test on every push) and a multi-stage Dockerfile deployed to Render, with `dotnet test` gating the production image build

---

### Project Structure

* **`Pages/`** — Razor Pages (UI & page models), including the public booking page and the `Admin` page
  * **`Shared/`** — layouts and partial views
* **`Models/`** — `AppDbContext` and EF Core entity classes (e.g. `Booking`)
* **`Utilities/`** — small standalone helpers, e.g. `MarkdownSanitizer` for escaping Telegram Markdown in user input
* **`Migrations/`** — EF Core database migrations
* **`MediatorSite.Tests/`** — xUnit test project
* **`.github/workflows/`** — CI pipeline definition (`build.yml`)
* **`appsettings.json`** / **`appsettings.Development.json`** — configuration (secrets excluded from source control)

---

### Continuous Integration / Continuous Deployment

* **CI (GitHub Actions):** on every push to `main`, the pipeline checks out the code, restores dependencies, builds the solution, and runs the full test suite. A failing test fails the build and is visible directly on GitHub.
* **CD (Render):** the app is deployed via a multi-stage Docker build. The build stage runs `dotnet test` before `dotnet publish`, so a failing test also blocks the Docker image from being produced — the final runtime image only ever contains code that passed its tests.
* Note: GitHub Actions and the Render build currently run as two independent checks rather than one gating the other — a natural next step would be wiring Render's deploy to trigger only after GitHub Actions succeeds.

---

### Testing

Unit tests currently cover `MarkdownSanitizer`, verifying that Telegram Markdown special characters (`*`, `_`, `` ` ``) are correctly escaped in user-submitted text before being sent to the Telegram API.

```bash
dotnet test
```
  

  ## https://yulia-mediator.onrender.com/

<img width="1882" height="949" alt="image" src="https://github.com/user-attachments/assets/32a1588c-61a2-4cde-9c2c-f274fe6d6138" />

<img width="1918" height="457" alt="image" src="https://github.com/user-attachments/assets/2ab8e634-3a48-4c37-babe-a4774b7fe843" />

<img width="1918" height="869" alt="image" src="https://github.com/user-attachments/assets/ade31866-87bc-4ff1-875b-6d94cfca4a57" />

<img width="1895" height="863" alt="image" src="https://github.com/user-attachments/assets/a22ace0d-50f5-4ff9-8b9a-84e9383191f1" />




