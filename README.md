# CManager – Customer Management System (Console & WPF)

## Overview

CManager is a customer management application built using **C# and
.NET**.\
The solution demonstrates a layered architecture and applies **SOLID
principles**,\
the **Repository Pattern**, **Service Pattern**, and **MVVM** for the
graphical user interface.

The project includes: - A **Console Application** for basic customer
management - A **WPF GUI Application** for an extended graphical
experience - **File-based JSON persistence** - **Unit tests** for
service and repository logic

------------------------------------------------------------------------

## Solution Structure

    CManager
    │
    ├── CManager.Core
    │   ├── Models
    │   └── Interfaces
    │
    ├── CManager.Application
    │   ├── Services
    │   └── Helpers
    │
    ├── CManager.Infrastructure
    │   └── Data
    │
    ├── CManager.Presentation.ConsoleApp
    │   └── Controllers
    │
    ├── CManager.Presentation.GuiApp
    │   ├── Views
    │   ├── ViewModels
    │   └── Services
    │
    └── CManager.Tests
        └── UnitTests

------------------------------------------------------------------------

## Architecture & Design Patterns

### Layers

-   **Presentation** (Console + WPF GUI)
-   **Application** (Services, validation, helpers)
-   **Infrastructure** (JSON file persistence)
-   **Core** (Models and interfaces)

### Applied Patterns & Principles

-   Repository Pattern
-   Service Pattern
-   MVVM
-   Single Responsibility Principle (SRP)
-   Interface Segregation Principle (ISP)
-   Dependency Inversion Principle (DIP)

------------------------------------------------------------------------

## Persistence

Customer data is stored in a **JSON file** located in:

    %AppData%/CManager/

Serialization and deserialization are handled by a dedicated helper
class.

------------------------------------------------------------------------

## Console Application Features

-   Create customer
-   View all customers
-   View specific customer by email
-   Delete customer by email
-   Menu-based navigation loop

------------------------------------------------------------------------

## GUI Application Features (WPF)

-   List customers
-   Create new customer
-   Edit existing customer
-   Delete customer
-   Navigation using `ContentControl`
-   MVVM with CommunityToolkit.Mvvm

------------------------------------------------------------------------

## Unit Testing

-   Framework: **xUnit**
-   Mocking: **Moq**

### Tested Components

- **CustomerService**  
  Tested using a mocked repository (Moq) to verify business logic in isolation,
  without relying on file system or persistence concerns.

- **CustomerRepository**  
  Tested using real JSON file persistence to validate data access behavior,
  serialization, and file-based storage.

Temporary files are created for repository tests and cleaned up after each test
execution to ensure test isolation and prevent side effects.

Repository-level tests were added voluntarily to validate real persistence logic,
even though only service-level unit tests were required by the assignment.

------------------------------------------------------------------------

## AI Usage Disclosure

AI tools were used **only as a discussion and learning aid** to: -
Clarify architectural concepts - Understand MVVM and unit testing
strategies - Improve documentation and comments - Review Microsoft
documentation

AI was **not used** to: - Generate business logic - Design application
architecture - Implement repositories or services - Write XAML layouts
or bindings

AI-assisted content is limited to documentation and comments and remains
below the allowed threshold.

------------------------------------------------------------------------

## References (Microsoft Documentation)

-   .NET Unit Testing Best Practices\
    https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices

-   WPF Application Overview\
    https://learn.microsoft.com/en-us/dotnet/desktop/wpf/overview

-   WPF Data Binding\
    https://learn.microsoft.com/dotnet/desktop/wpf/data/data-binding-overview

-   WPF ListView & GridView\
    https://learn.microsoft.com/dotnet/desktop/wpf/controls/listview-overview

-   ContentControl\
    https://learn.microsoft.com/dotnet/api/system.windows.controls.contentcontrol

-   System.Text.Json\
    https://learn.microsoft.com/dotnet/standard/serialization/system-text-json-overview

------------------------------------------------------------------------

## How to Run

### Console Application

1.  Set `CManager.Presentation.ConsoleApp` as startup project
2.  Run the application
3.  Use the menu to manage customers

### GUI Application

1.  Set `CManager.Presentation.GuiApp` as startup project
2.  Run the application
3.  Use the graphical interface to manage customers

------------------------------------------------------------------------

## Author

Developed as part of a course assignment to demonstrate proficiency in
C#, software architecture, and testing.
