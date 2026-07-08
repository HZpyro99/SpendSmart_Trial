# SpendSmart

A learning project to familiarize with **ASP.NET Core MVC**, **C#**, and **Razor HTML**.

## Overview

SpendSmart is a small ASP.NET Core MVC application built on .NET 10 that demonstrates core web development concepts through two main features:

1. **Expense Tracker** — A simple CRUD application for tracking expenses with descriptions and values
2. **Calculator Implementations** — Two separate calculator interfaces to explore different approaches to expression evaluation

The project uses Entity Framework Core with an in-memory database, Bootstrap for styling, and server-side rendering with Razor views.

## Learning Goals

This project covers:
- ASP.NET Core MVC fundamentals (controllers, models, views, routing)
- Entity Framework Core (DbContext, LINQ queries, in-memory database)
- Razor HTML templating (@directives, tag helpers, form binding)
- C# language features (LINQ, switch expressions, exception handling)
- HTML forms and data binding between views and controllers

## Features

### Expense Tracker
- View all expenses with a running total
- Create new expenses or edit existing ones
- Delete expenses

### Calculator #1 (Simple)
- Basic arithmetic operations (add, subtract, multiply, divide)
- One action per operation
- Uses `decimal` for precision

### Calculator #2 (Expression Evaluator)
- Evaluates full mathematical expressions (e.g., `2 + 3 * 4`)
- Server-side evaluation using tokenizer → shunting-yard algorithm → postfix evaluation
- Respects operator precedence and parentheses
- Uses `double` for numeric values
- Includes error handling and debug output

## Getting Started

### Prerequisites
- .NET 10 SDK

### Build and Run

```bash
# Restore packages
dotnet restore

# Build the project
dotnet build

# Run the development server (auto-launches browser)
dotnet run --project WebApplication1
```

The app will open at `http://localhost:5170` (HTTP) or `https://localhost:7046` (HTTPS).

## Project Structure

```
WebApplication1/
├── Controllers/           # MVC controllers
│   ├── HomeController.cs  # Expense CRUD & simple calculator
│   └── CalcuCont.cs       # Expression calculator
├── Models/                # Data models
│   ├── Expense.cs         # Expense entity
│   ├── Calcu.cs           # Expression calculator model
│   ├── Calculate.cs       # Simple calculator model
│   └── ExpensesDB.cs      # EF Core DbContext
├── Views/                 # Razor views
│   ├── Home/              # Views for expense & simple calculator
│   ├── CalcuCont/         # Views for expression calculator
│   └── Shared/            # Layout and shared views
├── wwwroot/               # Static files (CSS, JS)
└── Program.cs             # ASP.NET Core app configuration
```

## Notes

- **In-memory database**: Data is not persisted across app restarts. Restarting the app clears all expenses.
- **Two calculators**: Be aware that there are two separate, independent calculator implementations in this codebase. They serve as different learning examples.
- **Namespace inconsistency**: The project uses multiple namespace conventions (`SpendSmart.Models`, `Calculator.Models`, `ActualCalculator.Models`, `WebApplication1.*`) — a real-world reminder to establish consistent naming conventions early in a project.

## Next Steps for Learning

Consider exploring:
- Adding validation to the Expense form (data annotations)
- Migrating from in-memory to a persistent database (SQLite)
- Adding unit tests for calculator logic
- Converting to a client-side calculator to practice JavaScript
- Implementing filtering or sorting on the expense list
