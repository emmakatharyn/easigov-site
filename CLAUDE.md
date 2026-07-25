# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

This is the static website for EASI Gov, a New Mexico-based third party administrator (TPA) for health insurance. The site was originally exported from Webflow but has been refactored to plain HTML/CSS/JS with no build system or external framework.

## Development

No build tools or package manager. Open any `.html` file directly in a browser to preview. There is no local dev server configured.

## Architecture

### Pages
- `index.html` — Main homepage
- `daca.html` — DACA program information page
- `daca-carriers.html` — Comparison of health insurance carriers available to DACA recipients (BCBS, Presbyterian, Molina, United Healthcare)
- `daca-carrier-forms.html` — Enrollment forms for DACA carriers

### CSS
- `css/normalize.css` — Browser normalization (vendor, do not edit)
- `css/easi.css` — All site styles: base reset, nav, layout utilities, and page-specific styles

`daca-carriers.html` and `daca-carrier-forms.html` also contain inline `<style>` blocks for the carrier card grid, which is specific to those pages.

### JS
- `js/nav.js` — Vanilla JS for the mobile hamburger menu toggle (adds/removes `is-open` class on `.nav-menu` and `.menu-button`)

### Assets
- `images/` — Local image assets (gitignored; production images are served from `https://easigov.com/images/`)

## Key Details

- Google Analytics tracking ID: `G-QNKEZ0LK37` (present in all page `<head>` sections)
- Fonts: Merriweather and Montserrat loaded via Google Fonts `<link>` tag
- The mobile nav collapses at 991px. The hamburger button (`.menu-button`) is hidden on desktop and shown on mobile via CSS. Clicking it toggles the `is-open` class, which `nav.js` manages.
- Layout class `layout-grid` applies a two-column grid (1fr 1fr) to the services and business-info sections, collapsing to one column at ≤ 767px.
- The site has bilingual content (English/Spanish); Spanish enrollment form links should be maintained alongside English ones.
- `css/webflow.css` and `js/webflow.js` are leftover Webflow files that are no longer referenced and can be deleted.
