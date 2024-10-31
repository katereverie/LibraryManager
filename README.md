# Library Manager (To be updated)

## Overview

Library Manager is a cross-platform application designed to streamline and simplify the management of library resources. <br>

### Core Features

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

https://github.com/user-attachments/assets/4c64d2ef-7acb-4d59-bda6-4f0e7ed25aab

### Media Management 

https://github.com/user-attachments/assets/d30b3124-de16-483c-9d7b-f43f143374db


### Checkout Management

https://github.com/user-attachments/assets/b0cccf5a-fb6d-47d6-90a3-3dbb0dd03b53


### API Testing (Swagger, Console Logging)

https://github.com/user-attachments/assets/25a34d5c-256b-49e3-b4ba-059a0417d79f


