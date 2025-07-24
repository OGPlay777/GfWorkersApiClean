# Metalworks Order Management System

This (unfinished/in developement) project is a backend system designed for a metal processing and painting company to manage and track the full lifecycle of customer orders. It allows employees to log their work, monitor task progress, and control the production pipeline in real time.

> ⚠️ This is a **public demo version** of the project. All sensitive data such as IP addresses, passwords, and secret keys have been removed or replaced with placeholder values.

## 🔧 Features

- Full tracking of orders from creation to completion
- Time tracking of work performed by each employee
- Stage-based processing for manufacturing and painting
- Role-based authorization and secure authentication
- Email and WhatsApp notifications for status updates
- Dockerized microservices architecture for easy deployment

## 🧱 Technologies Used

- ASP.NET Core (REST API)
- IdentityServer4 (Authorization & JWT via Cookie) 
- Entity Framework Core + MySQL
- Redis (caching of frequent queries)
- Serilog (structured logging)
- Docker (for deployment and testing)

## 🗂️ Project Structure

- `Main API` — Handles core business logic for orders and operations
- `Identity Server` — Provides authentication & authorization https://github.com/OGPlay777/IdentityServerAppClean
- `Notification Service` — Sends email/WhatsApp updates https://github.com/OGPlay777/NotificationService_Clean

## 🚀 Getting Started

This repository is intended for demonstration purposes and does not include real credentials. To run it:

## 📄 License

This repository is shared under the MIT license for demonstration and portfolio purposes.
