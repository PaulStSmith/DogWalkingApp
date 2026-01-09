# Dog Walking Manager – WinForms Code Challenge

## Overview

The **Dog Walking Manager** is a Windows Forms application built with **.NET 8** that manages clients, their dogs, and dog-walking sessions.
It was developed as a technical challenge to demonstrate **professional WinForms development**, with emphasis on:

* Correct use of **simple and complex data binding**
* Clean, maintainable architecture
* Thoughtful UI/UX decisions
* Real-world engineering practices

The application persists data locally and is fully functional after restart.

## Key Features

* **Client, Dog, and Walk management** (create, edit, delete)
* **Hierarchical navigation** (Client → Dog → Walk)
* **Search** across clients, dogs, and walk notes
* **Validation with user-friendly messages**
* **Authentication** (simple login for demo purposes)
* **Persistent storage** using a local SQL database
* **Comprehensive test coverage** for domain and service logic

## Technology Stack

* **.NET 8.0 (Windows)**
* **Windows Forms**
* **Entity Framework Core**
* **SQL Server LocalDB**
* **xUnit + FluentAssertions** for testing

## Architecture

The solution is organized into clear layers to keep concerns separated:

```
DogWalkingApp.sln
├── Domain      // Entities + validation logic
├── Data        // EF Core DbContext, repositories, migrations
├── Services    // Business logic and orchestration
├── UI          // WinForms, ViewModels, binding logic
├── Resources   // UI and validation strings (localization-ready)
└── Tests       // Unit tests
```

This structure allows the UI to remain simple while keeping business rules and persistence logic isolated and testable.

## Data Binding Strategy (Important)

This application explicitly demonstrates **both WinForms data binding types** requested in the challenge.

### Complex Data Binding

Used for collections displayed in grids:

* `DataGridView` controls are bound via `BindingSource`
* Backed by `BindingList<T>` ViewModels
* Examples:

  * Dogs list
  * Walks list

```csharp
// COMPLEX BINDING
dgvDogs.DataSource = _dogsBindingSource;
dgvWalks.DataSource = _walksBindingSource;
```

### Simple Data Binding

Used for individual fields in detail panels and editor dialogs:

* TextBoxes, DateTimePickers, NumericUpDowns
* Bound directly to ViewModel properties
* Updates occur immediately via `INotifyPropertyChanged`

```csharp
// SIMPLE BINDING
txtClientName.DataBindings.Add(
    "Text",
    _clientDetailViewModel,
    nameof(ClientDetailViewModel.Name),
    false,
    DataSourceUpdateMode.OnPropertyChanged);
```

**Important note:**
Controls are bound to **in-memory ViewModels**, *not* directly to the database.
Database access is explicit and controlled (load, save, refresh), avoiding chatty or fragile UI behavior.

## Validation

Validation rules live in the **domain entities** and are enforced consistently across the application.

* Each entity implements a validation contract
* Validation is triggered before persistence
* Errors are shown to the user in a clear, focused way

This keeps validation logic centralized, testable, and reusable.

## Persistence

* Uses **SQL Server LocalDB**
* Database schema is managed via **EF Core migrations**
* Data persists between application runs
* A small default user is created automatically for demo purposes

## Tests

The solution includes a comprehensive test suite covering:

* Entity validation rules
* Service-layer behavior
* Repository persistence logic
* Authentication

Tests are isolated, fast, and run against an in-memory database.

## How to Run

### Prerequisites

* Windows
* Visual Studio 2022 or 2026
* .NET 8 SDK
* SQL Server LocalDB (installed by default with Visual Studio)

### Steps

1. Open `DogWalkingApp.sln`
2. Set `DogWalkingApp.UI` as the startup project
3. Press **F5**

### Login

A default demo user is created automatically:

```
Username: admin
Password: admin123
```

Thank you for taking the time to review this submission.
