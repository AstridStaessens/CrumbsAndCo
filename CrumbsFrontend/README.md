# Crumbs & Co — Angular Website

Volledige Angular 17 website voor Crumbs & Co bakkerij op bestelling.

## Projectstructuur

```
src/app/
├── components/          # Gedeelde componenten (globaal)
│   ├── header/          # Navigatie met routerLink + cart badge
│   ├── footer/          # Footer met links
│   └── cart/            # Cart sidebar (slide-in)
├── pages/               # Pagina's (lazy loaded via router)
│   ├── home/            # Homepage met hero, featured, USPs
│   ├── shop/            # Webshop met categorie-filter + cart
│   ├── op-maat/         # Bestelformulier met foto-upload
│   ├── galerij/         # Fotogalerij
│   └── contact/         # Contactpagina + formulier
├── models/
│   └── product.model.ts # Product interface + PRODUCTS data
├── services/
│   └── cart.service.ts  # CartService (BehaviorSubject, providedIn: 'root')
├── app.component.ts     # Root component
├── app.config.ts        # provideRouter + provideAnimations
└── app.routes.ts        # Routes met lazy loading
```

## Routes

| URL        | Pagina            |
|------------|-------------------|
| `/`        | Home              |
| `/shop`    | Webshop           |
| `/op-maat` | Bestelling op maat|
| `/galerij` | Galerij           |
| `/contact` | Contact           |

## Installatie & starten

```bash
# 1. Dependencies installeren
yarn install

# 2. Dev server starten
yarn start
# → http://localhost:4200

# 3. Productie build
yarn build
```

## Logo instellen

Zet `transparent-logo.png` in `src/assets/logo.png`.

## Kenmerken

- **Angular 17** met standalone components
- **Lazy loading** per pagina via `loadComponent`
- **Cart service** met RxJS `BehaviorSubject` — globale state
- **Reactive Forms** via `NgModel` (FormsModule)
- **RouterLink / RouterLinkActive** voor navigatie
- **SCSS** per component
- Foto upload (bestandsnaam preview) op Op maat pagina
- Formuliervalidatie met `[disabled]` op submit-knop
- `withViewTransitions()` voor vloeiende pagina-overgangen
