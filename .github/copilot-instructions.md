# Project Requirement Document: Angular Retail Sales & Inventory App

**Version:** 1.0
**Date:** 2025-04-16

## 1. Introduction

### 1.1 Purpose
This document outlines the functional requirements for the Angular web application designed as a frontend for the `Break.Api`. The application will serve as a Point of Sale (POS) and Inventory Management system for a retail store environment.

### 1.2 Scope
**In Scope:**
* User Authentication (Login, Registration).
* Product (Item) Management (Create, Read, Update, Delete - CRUD).
* Inventory Viewing (as part of Product details).
* Inventory Level Updates (via Product update).
* Sales Transaction Processing (Create, View List, View Details, Confirm, Cancel).
* Basic reporting derived from available data (e.g., Sales List).

**Out of Scope:**
* Advanced Reporting (complex analytics, custom dashboards beyond simple lists/details).
* Direct hardware integration (barcode scanners, receipt printers - beyond capturing input/displaying data).
* Payment gateway integration (processing actual credit card payments).
* Supplier/Purchase Order Management (unless implicitly part of Product).
* Multi-location inventory management (API implies single stock count per product).
* Offline capabilities.

### 1.3 Definitions, Acronyms, and Abbreviations
* **API:** Application Programming Interface (`Break.Api`)
* **UI:** User Interface (The Angular Application)
* **PRD:** Project Requirement Document
* **POS:** Point of Sale
* **CRUD:** Create, Read, Update, Delete
* **Product/Item:** Refers to goods sold by the retail store. The backend API uses "Item".
* **JWT:** JSON Web Token

## 2. User Roles and Characteristics

* **Cashier:** Primarily uses the Sales/POS module. May have limited access to view products.
* **Stock Manager:** Manages products and inventory levels. Requires access to Product Management features.
* **Administrator:** Full access to all features, including user registration (if implemented via UI) and potentially system settings.

## 3. Functional Requirements

This section details the features the application must provide, largely based on the capabilities exposed by the `Break.Api`.

### 3.1 Authentication & Authorization (Based on `/api/auth/...`)
* **FR-AUTH-01:** The system **shall** provide a login interface where users can enter their username and password.
* **FR-AUTH-02:** Upon successful login (`POST /api/auth/login`), the system **shall** grant the user access based on their role and store an authentication token (e.g., JWT) for subsequent API calls.
* **FR-AUTH-03:** The system **shall** restrict access to features based on the logged-in user's role (Requires frontend route guards and potentially backend authorization checks).
* **FR-AUTH-04:** The system **shall** provide a logout mechanism that clears the user's session/token.
* **FR-AUTH-05:** (Optional - Requires Admin Role) The system **shall** allow an Administrator to register new users (`POST /api/auth/register`) by providing a username, email, password, and roles.

### 3.2 Product (Item) Management (Based on `/api/item/...`)
* **FR-PROD-01:** The system **shall** display a list of all products (`GET /api/item`).
* **FR-PROD-02:** The product list view **should** allow users to search, filter (e.g., by category), and sort products based on available fields. (Note: API spec doesn't explicitly define query parameters for this; frontend implementation may be needed).
* **FR-PROD-03:** The system **shall** allow authorized users (Stock Manager, Admin) to add a new product (`POST /api/item`) by providing required details (Product Code, Barcode, Name, Description, Category) and optional details (Unit Price, Reorder Qty, Initial Stock Levels - min/max/current). See `CreateItemRequest` schema.
* **FR-PROD-04:** The system **shall** display the detailed information for a specific product (`GET /api/item/{id}`), including its current inventory levels (`quantityInStock`, `minimumStockLevel`, etc.).
* **FR-PROD-05:** The system **shall** allow authorized users to update an existing product's details (`PUT /api/item/{id}`), including its inventory levels (`quantityInStock`). See `UpdateItemRequest` schema.
* **FR-PROD-06:** The system **shall** allow authorized users to delete a product (`DELETE /api/item/{id}`). (Consider soft delete vs. hard delete implications).
* **FR-PROD-07:** The system **should** provide clear validation feedback for product creation/update forms based on API requirements (e.g., required fields).

### 3.3 Inventory Management (Derived from Product Management)
* **FR-INV-01:** The system **shall** display the current `quantityInStock`, `minimumStockLevel`, and `maximumStockLevel` when viewing a product's details (see FR-PROD-04).
* **FR-INV-02:** The system **shall** allow authorized users to update the `quantityInStock` for a product via the product update feature (see FR-PROD-05). This serves as the primary mechanism for stock adjustments based on the provided API.
* **FR-INV-03:** (Optional - Frontend Logic) The system **should** visually indicate products whose `quantityInStock` is at or below their `minimumStockLevel` in the product list or a dedicated low-stock view.

### 3.4 Sales / Point of Sale (POS) (Based on `/api/sale/...`)
* **FR-SALE-01:** The system **shall** provide an interface for creating a new sales transaction (`POST /api/sale`).
* **FR-SALE-02:** Users **shall** be able to add products (items) to the sale, specifying the quantity for each (`SaleItemRequest` within `CreateSaleRequest`). Item addition should be possible via search or barcode input.
* **FR-SALE-03:** The system **shall** calculate and display the running total for the current sale based on items added and their unit prices. (Note: Tax/discount calculation logic might need frontend implementation if not handled by the API during sale creation).
* **FR-SALE-04:** The system **shall** allow users to apply a coupon code to the sale (`couponCode` in `CreateSaleRequest`).
* **FR-SALE-05:** Upon completion (e.g., payment received offline), the user **shall** be able to confirm the sale (`POST /api/sale/{id}/confirm`). (Note: The API implies a sale is created first, then confirmed).
* **FR-SALE-06:** The system **shall** allow users to cancel an existing sale (`POST /api/sale/{id}/cancel`). (Clarify if only pending sales or also confirmed sales can be cancelled).
* **FR-SALE-07:** The system **shall** display a list of all past sales transactions (`GET /api/sale`). The list should be sortable (e.g., by date) and potentially filterable.
* **FR-SALE-08:** The system **shall** allow users to view the details of a specific past sale (`GET /api/sale/{id}`), including the items sold, quantities, and total amount.
* **FR-SALE-09:** (Optional - Frontend) The system **should** provide an option to display/print a receipt for a confirmed sale.

## 4. Non-Functional Requirements (Examples)

* **NFR-PERF-01:** The UI should load key views (product list, sales POS) within 3 seconds on a standard broadband connection.
* **NFR-USE-01:** The application must be responsive and usable on both desktop and tablet screen sizes.
* **NFR-SEC-01:** All communication with the `Break.Api` must use HTTPS.
* **NFR-SEC-02:** Authentication tokens must be handled securely on the client-side.
* **NFR-MAINT-01:** Code should adhere to the Angular Style Guide and include comments for complex logic.

## 5. API Dependencies

* The functionality of this Angular application is dependent on the `Break.Api` as defined in the provided OpenAPI v3.0.4 specification.
* The frontend application must interact with the API endpoints listed in the specification to perform its functions.
* Frontend data models (TypeScript interfaces/classes) must align with the API's request/response schemas (e.g., `CreateItemRequest`, `UpdateItemRequest`, `CreateSaleRequest`, etc.).

## 6. Assumptions

* The `Break.Api` handles all core business logic (e.g., price calculation, inventory deduction upon sale confirmation).
* The API provides meaningful error messages that can be displayed to the user.
* User roles and permissions are correctly managed by the backend API based on the authenticated user.
* Updating a product's `quantityInStock` via `PUT /api/item/{id}` is the intended method for all inventory adjustments (receiving stock, corrections, etc.). No separate "Inventory Adjustment" endpoint exists in the provided spec.
* The API endpoints for retrieving lists (`GET /api/item`, `GET /api/sale`) return sufficient data for display and basic filtering/sorting on the frontend. Pagination support from the API is assumed or will need frontend handling.