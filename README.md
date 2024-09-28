## Introduction

### Project Description

Library Manager is a cross-platform application designed to streamline and simplify the management of library resources. <br>

### Core Features

It's core features include:<br>
* __Borrower Management__: register, track, and manage library users, including their borrow history (in the form of checkout logs), personal information.
* __Media Management__: catalog, organize, and manage the details of different types of media, such as books, DVDs, and digital audios; generate reports, such as most popular media items.
* __Checkout Management__: oversee the borrowering and returning of media, tracking due dates of borrowed media.

## Architecture

### Pattern (n-Tier-architecture)

- __UI__: Provides a console-based user interface for managing library operations.
- __API__: This is the entry point for external clients. It handles HTTP requests and routing to services through controllers for borrowers, media, and checkouts. The API includes Swagger documentation for API endpoint description and testing, and it also implements validation to ensure data integrity.
- __Application__: Contains business logic for managing borrowers, media, and checkouts.
- __Core__: Defines the domain models and repository interfaces that represent the core entities in the system.
- __Data__: Manages data persistence through Dapper and Entity Framework, providing flexibility in database access.
- __UnitTest__: Ensures the correctness of the business logic and repository implementations through unit testing.

### Tech-Stack

- Language: C#
- Frameworks: ASP.NET Core (Web APIs), Entity Framework Core (ORM), Dapper (light ORM), Swagger (API documentation), FluentValidation (model validation), etc.
- Database-related Operations: Microsoft SQL Server (Database Management), Docker (Containerization), Dapper and Entity Framework Core (Data Access)
- Tests: NUnit
- Other tools: Postman, Azure Data Studio, Swagger, Visual Studio (IDE)

## Visual Demonstration

### Borrower Management





