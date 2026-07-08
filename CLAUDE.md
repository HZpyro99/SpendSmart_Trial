# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

SpendSmart is an ASP.NET Core MVC (.NET 10) web app with two features: an expense tracker backed by EF Core, and two independent calculator implementations. There is a single project, `WebApplication1`, with no test project in the solution.

## Commands

Run all commands from the repo root (where `SpendSmart.slnx` lives) or from `WebApplication1/`.

- Build: `dotnet build`
- Run (dev server, auto-launches browser): `dotnet run --project WebApplication1`
  - HTTP: `http://localhost:5170`, HTTPS: `https://localhost:7046` (see `WebApplication1/Properties/launchSettings.json`)
- Restore packages: `dotnet restore`

There are no automated tests in this repo currently.

## Architecture

Standard ASP.NET Core MVC layout: `Controllers/`, `Models/`, `Views/<ControllerName>/`, `wwwroot/`. Routing is the default convention-based route (`{controller=Home}/{action=Index}/{id?}`) configured in `Program.cs`.

### Data layer

- `SpendSmartDbContext` (`Models/ExpensesDB.cs`) is an EF Core `DbContext` with a single `DbSet<Expense>`.
- The database is **in-memory only** (`UseInMemoryDatabase`, configured in `Program.cs`) — data does not persist across app restarts, and there is no migrations setup.

### Expense tracker (`HomeController`)

CRUD for `Expense` records lives entirely in `HomeController`: `Expenses` (list + total), `CreateEditExpense` (shared create/edit view, branches on whether `id` is null), `Forms` (upsert — checks `model.Id == 0` to decide `Add` vs `Update`), `DeleteExpense`.

### Two unrelated calculator implementations

The codebase has two separate, non-interacting calculators — be careful not to conflate them when making changes:

1. **Simple arithmetic calculator** — `HomeController.Calcadd/Calcsub/Calcmul/Calcdiv`, model `Calculator.Models.Calc` (`Models/Calculate.cs`). One action per operator; each takes a posted `Calc` and re-renders the shared `Views/Home/Calculator.cshtml` view. `decimal` operands.
2. **Expression calculator** — `CalcuCont` controller (`Controllers/CalcuCont.cs`), model `ActualCalculator.Models.Calcu` (`Models/Calcu.cs`), view `Views/CalcuCont/Actualcalc.cshtml`. Parses and evaluates a full infix expression string server-side via a hand-rolled tokenizer → shunting-yard → postfix evaluator (`Tokenize` / `ToPostfix` / `Evaluate` in `CalcuCont`). `double` operands. Errors during evaluation are caught and surfaced via `ViewBag.Error`; intermediate tokens/postfix/stack state are also stashed in `ViewBag` for debugging.

### Namespace inconsistency

Namespaces do not follow a single convention across the project — note this when adding new files or `using` statements: `Program.cs` and `ExpensesDB.cs`/`Expense.cs` use `SpendSmart.Models`, `Calculate.cs` uses `Calculator.Models`, `Calcu.cs` uses `ActualCalculator.Models`, and everything else (controllers, `ErrorViewModel`) uses `WebApplication1.*`. The assembly/project name itself is `WebApplication1` even though the solution and repo are `SpendSmart`.
