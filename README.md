# CrumbsAndCo — Graduaatsproef Programmeren

Artisanale bakkerij webshop gebouwd met Angular (frontend) en ASP.NET Core (backend).

---

## Tech Stack

| Laag | Technologie |
|---|---|
| Frontend | Angular 17+ (standalone components) |
| Backend | ASP.NET Core Web API (.NET 10) |
| Database | Azure SQL Server (SQL Server) |
| Authenticatie | JWT tokens |
| Betaling | Stripe |
| Hosting DB | Azure SQL Database |
| Hosting API | Azure App Service |
| Hosting FE | Azure Static Web Apps |
| CI/CD | Azure DevOps Pipelines |

---

## Projectstructuur

```
Crumbs/
├── CrumbsBackend/
│   ├── Crumbs.API                  ← Controllers, Program.cs
│   ├── Crumbs.API.Contracts        ← Request/Response modellen
│   ├── Crumbs.Domain.Models        ← Domeinmodellen + interfaces
│   ├── Crumbs.Domain.Services      ← Businesslogica
│   ├── Crumbs.Persistence          ← DbContext, repositories, mapping
│   └── Crumbs.Persistence.Entities ← EF Core entiteiten
└── CrumbsFrontend/                 ← Angular project (nog aan te maken)
```

---

## Stappenplan

### Fase 1 — Database & Backend fundament ✅
- [x] Azure SQL Database aanmaken
- [x] Solution aanmaken met drielagen architectuur
- [x] EF Core entiteiten schrijven
- [x] DbContext configureren
- [x] Connection string instellen
- [x] Eerste migratie aanmaken en uitvoeren
- [ ] Repository interfaces schrijven
- [ ] Repository implementaties schrijven
- [ ] Mapping tussen entiteiten en domeinmodellen
- [ ] Service laag opzetten

### Fase 2 — Authenticatie
- [ ] JWT configuratie in Program.cs
- [ ] Register endpoint (POST /api/auth/register)
- [ ] Login endpoint (POST /api/auth/login)
- [ ] Wachtwoord hashen met BCrypt
- [ ] JWT token genereren en teruggeven
- [ ] Rolgebaseerde autorisatie instellen (customer/admin)

### Fase 3 — API Endpoints
- [ ] ProductsController (GET all, GET by id, GET by category)
- [ ] CategoriesController (GET all)
- [ ] OrdersController (POST order, GET own orders)
- [ ] AdminController (CRUD producten, alle bestellingen bekijken)
- [ ] FluentValidation toevoegen voor requests

### Fase 4 — Angular Frontend opzetten
- [ ] Angular project aanmaken (ng new)
- [ ] Angular Material installeren
- [ ] Routing instellen
- [ ] HTTP interceptor voor JWT token
- [ ] Environment configuratie (dev/prod)

### Fase 5 — Angular Pagina's
- [ ] Navbar component
- [ ] Homepagina
- [ ] Over ons pagina
- [ ] Webshop pagina (productlijst)
- [ ] Productdetail pagina
- [ ] Winkelwagen (localStorage)
- [ ] Login/registratie pagina
- [ ] Contact pagina

### Fase 6 — Stripe Betalingen
- [ ] Stripe account aanmaken
- [ ] Stripe NuGet package installeren in backend
- [ ] PaymentController aanmaken
- [ ] Checkout sessie aanmaken
- [ ] Webhook verwerken (betaling bevestigd)
- [ ] Stripe checkout integreren in Angular

### Fase 7 — Admin Panel
- [ ] Admin guard in Angular
- [ ] Admin dashboard pagina
- [ ] Producten beheren (toevoegen/bewerken/verwijderen)
- [ ] Categorieën beheren
- [ ] Bestellingen overzicht
- [ ] Bestellingstatus bijwerken

### Fase 8 — Deployment
- [ ] Azure App Service aanmaken voor backend
- [ ] Azure Static Web Apps aanmaken voor frontend
- [ ] Azure DevOps pipeline voor backend (CI/CD)
- [ ] Azure DevOps pipeline voor frontend (CI/CD)
- [ ] Domein koppelen
- [ ] CORS instellen voor productie URL

### Fase 9 — Afwerking
- [ ] Responsive design controleren
- [ ] Productafbeeldingen uploaden
- [ ] Laadindicatoren toevoegen
- [ ] Foutmeldingen afhandelen
- [ ] Testen van volledige bestelflow

---

## Lokaal draaien

### Backend
```bash
cd CrumbsBackend
# Open CrumbsBackend.slnx in Visual Studio
# F5 om te starten
# API beschikbaar op https://localhost:7xxx
```

### Frontend
```bash
cd CrumbsFrontend
npm install
ng serve
# App beschikbaar op http://localhost:4200
```

---

## Omgevingsvariabelen

Vervang in `appsettings.json`:
- `{your_password}` → jouw Azure SQL wachtwoord
- JWT Secret → willekeurige lange string

Deel nooit je `appsettings.json` publiek met ingevuld wachtwoord.
