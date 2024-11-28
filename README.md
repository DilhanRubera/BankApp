# Bank Data Web Application

## Overview

This application is a two-tier architecture system with a **business layer** that exposes APIs for performing various banking operations, and a **presentation layer** that serves the user interface. It is designed to provide both **user dashboards** and **admin dashboards** to manage financial data, transactions, and accounts.

### Architecture

1. **Business Layer (Web Services)**:
   - Exposed through RESTful APIs via `/api/` endpoint.
   - Handles all data processing, business logic, and database interaction.
   - Key operations include retrieving user data, processing transactions, and managing accounts.
   - **Technologies Used**:
     - **ASP.NET Core MVC** for building web services.
     - **Entity Framework Core** for database interaction and management.
     - **SQL Database** for storing user data, transactions, and other financial information.

2. **Presentation Layer (Webpage)**:
   - Accessible through the root `/` endpoint.
   - Built with HTML, CSS, and JavaScript (AJAX) to communicate with the business layer's API.
   - Dynamically updates the UI based on API responses, e.g., displaying transaction histories or managing money transfers.
   - **Technologies Used**:
     - **HTML/CSS** for structuring and styling the webpage.
     - **JavaScript** for making asynchronous API requests and dynamically updating the UI.

## Features

### User Dashboard
- **User Profile Information**: Displays the user's name, profile picture, email, and phone number, with options for updating personal details.
- **Account Summary**: Quick overview of the user’s accounts, balances, and access to transaction history.
- **Transaction History**: View and filter recent transactions with options to sort by date or amount.
- **Money Transfer**: Initiate transfers between accounts or to other users. Includes form validation and error handling.
- **Security**: Secure login and logout functionalities to protect user data.

### Admin Dashboard
- **Admin Profile Information**: Admin's personal details and profile information.
- **User Management**: Tools to manage user accounts (create, edit, deactivate, reset passwords).
- **Transaction Management**: View all user transactions with filtering and sorting options.
- **Security and Access Control**: Logs to track admin activities and system changes, ensuring secure access to the platform.
