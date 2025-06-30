# MeetYourMatch 💖  
**A full-stack dating web application built with ASP.NET Core 8 and Angular 18**

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet) ![Angular](https://img.shields.io/badge/Angular-18-red) ![Deployed](https://img.shields.io/badge/Deployed-Azure-blue) ![License](https://img.shields.io/badge/license-MIT-brightgreen)

> 🔗 **Live Demo**: [https://meet-your-match.azurewebsites.net](https://meet-your-match.azurewebsites.net)

---

## 🌟 Overview

**MeetYourMatch** is a real-world, full-stack web application built using the latest versions of **.NET 8** and **Angular 18**. It simulates a modern matchmaking platform with real-time chat, role-based authentication, photo uploads, and a rich user experience.

---

## 🔧 Tech Stack

### 🖥️ Frontend – Angular 18
- Angular CLI & RxJS
- Angular Reactive & Template Forms
- ngx-bootstrap, ngx-toastr, ng-gallery
- Bootstrap 5 + Bootswatch Vapor theme
- Font Awesome icons
- SignalR for real-time features
- Route guards and JWT handling

### 🔙 Backend – ASP.NET Core 8
- ASP.NET Core Web API
- Entity Framework Core 8
- AutoMapper
- ASP.NET Core Identity (User + Role Management)
- JWT Authentication & Authorization
- SignalR (chat, presence)
- Cloudinary integration
- API Versioning + Global Exception Handling

---

## 🔐 Key Features

- ✅ JWT-based User Authentication
- ✅ Role-Based Access Control (Admin/User)
- ✅ Real-Time Messaging and Presence with SignalR
- ✅ Drag-and-drop Profile Photo Upload (Cloudinary)
- ✅ Filtering, Sorting, and Pagination
- ✅ Responsive UI with Bootswatch themes
- ✅ Reactive + Template Forms with Validation
- ✅ Centralized Error Handling (UI + API)
- ✅ Hosted on Azure App Service (Free Tier)

---

## 🚀 Getting Started

### 🔙 Backend Setup (ASP.NET Core API)
1. Navigate to the API project:
    ```bash
    cd API
    ```
2. Restore packages:
    ```bash
    dotnet restore
    ```
3. Update the database:
    ```bash
    dotnet ef database update
    ```
4. Run the API:
    ```bash
    dotnet run
    ```

> API will be available at `https://localhost:5001`

---

### 🖥️ Frontend Setup (Angular)
1. Navigate to the client project:
    ```bash
    cd Client
    ```
2. Install dependencies:
    ```bash
    npm install
    ```
3. Run the Angular development server:
    ```bash
    ng serve --ssl true
    ```

> App will run at `https://localhost:4200`

---

## 🌐 Deployment

- ✅ **Live URL**: [https://meet-your-match.azurewebsites.net](https://meet-your-match.azurewebsites.net)
- Angular build output is configured to go to `API/wwwroot` using Angular’s `outputPath`.
- Hosted on **Azure App Service**
- Supports both Linux (Nginx + Kestrel) and Windows (IIS) deployments

---

## 📚 Course Reference

This application was built as part of a **project-based course** updated for .NET 8 and Angular 17/18. It covers building a full-stack dating application using modern technologies and best practices.

> _"The absolute best course for building an API in .NET Core and working with Angular."_ – Jim  
> _"Real-world experience using technologies that are in demand."_ – Daniyal

---

## 🙋‍♂️ Author

**Mahesh Kumar Singh**  
🔗 [LinkedIn](https://www.linkedin.com/in/maheshkumarsingh/)  
🌐 [Live App](https://meet-your-match.azurewebsites.net)  
📁 [GitHub Repo](https://github.com/maheshkumarsingh/MeetYourMatch)

---

## 📝 License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.

---

## ⭐ Contributions

Contributions, issues, and feature requests are welcome!  
If you like this project, don’t forget to ⭐ the repo and share it with others.

