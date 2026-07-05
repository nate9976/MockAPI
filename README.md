# .NET 9 Web API Evaluation Task
This task is not meant to be graded against strict criteria. It serves as a starting point for our technical interview conversation. Please keep the solution simple, and focus on clean code, best practices, and maintainability.

## Overview
Create a small **.NET Web API** with an in-memory/mock data store of `Product` items.


### Category Model
- **Id**
- **Name**
- **Description**

### Product Model
- **Id**
- **Name**
- **Price**
- **CategoryId** (references the Category model)

## Requirements
- **Endpoint 1:** Implement one endpoint that returns filtered products (e.g., filter by name, category, min/max price).
- **Endpoint 2:** Implement one endpoint that updates products price and/or name.
- **Abstraction:** The controller must depend on a data access interface (injected), so different product categories can swap implementations without changing the controller code.
- **Validation:** Handle invalid input gracefully.

## Unit Tests
- Ensure filtering works correctly.
- Verify the controller/handler works when wired to a fake/in-memory data access implementation.

## Submission
- Use **Git** for version control.
- When finished, email the URL with access to your repository.
- Use Swagger (OpenAPI) or a similar tool for API documentation and interactive testing (e.g., Swagger UI).

---

**That’s all — keep it simple and have fun!**

---