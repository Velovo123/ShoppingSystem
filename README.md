# ShoppingSystem

## Project Overview

This repository is aimed at developing a comprehensive application using ASP.NET Core. The project demonstrates key functionalities of ASP.NET Core, including role-based access control, policy-based authorization, and claim-based access management.

### Key Features

- **Role-Based Authorization**: Implemented using `[Authorize]` attributes and policies to control access based on user roles (e.g., Admin, Buyer).
- **Claim-Based Access**: Specific features are accessible based on user claims, particularly focusing on the `buyerType` claim with values like `none`, `regular`, `golden`, and `wholesale`.
- **Admin Panel**: An Admin tab that allows users with the 'Admin' role to manage other users, including editing roles and claims.
- **Discount Page**: Accessible only to users with a `buyerType` of `golden` or `wholesale`.

## Project Structure

- **Controllers**: Handles the business logic for various entities such as ShoppingCart, Users, and Orders.
- **Services**: Contains services for handling specific business logic, like authorization and user management.
- **Models**: Includes the data models representing different entities in the application.
- **DbContext**: A structured context for managing the database interactions following patterns.

