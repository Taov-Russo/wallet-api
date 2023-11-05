# Introduction
This is a software development task that focuses on designing and implementing a Wallet System for a sports betting platform. The primary goal is to create a service responsible for managing user funds, also known as wallets. The system should be able to handle a large number of concurrent users and maintain fault tolerance, ensuring that it continues to operate even in the event of node failures.

## Prerequisites
- Microsoft SQL Server 2019 or later
- Microsoft SQL Server Management Studio
- Postman (or an application Swagger)

## Setup
Execute the scripts located in the scripts folder of the project in SQL Server Management Studio to create a new database called WalletApiDB, tables and a service login.

## Functionality
The wallet system provides the following functionality:
- **Create a wallet:** Create a new wallet for a user.
- **Add funds to a wallet:** Add funds to a user’s wallet.
- **Remove funds from a wallet:** Remove funds from a user’s wallet.
- **Query the current state of a wallet:** Get the current balance of a user’s wallet.
- **Prevent negative balance:** The balance of a wallet cannot be negative.
- **Prevent double spending:** A user can’t spend the same funds twice.
- **REST APIs:** The client should interact with the service with REST APIs.