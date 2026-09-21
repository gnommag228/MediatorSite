# MediatorSite 

A full-stack web application built with **C#** and **ASP.NET Core** (Razor Pages) designed for managing client booking services, appointment slots, and automated notifications.

---

###  Features

* **Calendar & Slot Reservation:** Interactive slot booking logic with automated validation to prevent double-booking.
* **Telegram Bot Integration:** Instant real-time notifications via the Telegram Bot API sent directly to the site administrator upon new booking requests.
* **Admin Portal:** Secured administrative area with authentication for managing incoming requests, reviewing client bookings, and updating schedule slots.
* **CI/CD & Deployment:** Configured for automated continuous deployment to Render directly from GitHub using environment variables for sensitive configuration management.

---

###  Tech Stack & Architecture

* **Framework:** .NET 8 / ASP.NET Core (Razor Pages, Web API)
* **ORM & Database:** Entity Framework Core with SQLite (database migrations enabled)
* **Integrations:** Telegram Bot API
* **Security:** Authentication & Authorization for admin routes, environment variables for secrets
* **Deployment:** Render (Hosting & CD)

---

###  Project Structure

* **`Pages/`** — Razor Pages (UI & Page Models)
  * **`Admin/`** — Admin panel & booking management
  * **`Shared/`** — Layouts & partial views
* **`Models/`** — Data entities & EF Core models
* **`Services/`** — Business logic (Telegram API, Booking validation)
* **`Data/`** — DbContext and EF Core Migrations
* **`appsettings.json`** — Configuration file

  

  ## https://yulia-mediator.onrender.com/

<img width="1882" height="949" alt="image" src="https://github.com/user-attachments/assets/32a1588c-61a2-4cde-9c2c-f274fe6d6138" />

<img width="1918" height="457" alt="image" src="https://github.com/user-attachments/assets/2ab8e634-3a48-4c37-babe-a4774b7fe843" />

<img width="1918" height="869" alt="image" src="https://github.com/user-attachments/assets/ade31866-87bc-4ff1-875b-6d94cfca4a57" />

<img width="1895" height="863" alt="image" src="https://github.com/user-attachments/assets/a22ace0d-50f5-4ff9-8b9a-84e9383191f1" />




