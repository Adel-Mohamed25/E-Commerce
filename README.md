# E-Commerce
# E-Commerce Management System

![E-Commerce](assets/images/ecommerce.png)

## 📌 Overview
The **E-Commerce Management System** is a full-featured platform built using **.NET Core** with **Onion Architecture**, ensuring scalability, maintainability, and high performance. It provides a seamless shopping experience with user authentication, product management, and order processing.

## 🚀 Features

✅ **Onion Architecture** - Modular and scalable code structure.
✅ **Entity Framework Core** - Repository Pattern \& Unit of Work.
✅ **Identity Authentication \& Authorization** - Secure user management.
✅ **Redis Distributed Caching** - Enhances performance.
✅ **Serilog Logging** - Error tracking and monitoring.
✅ **Pagination \& Filtering** - Optimized product browsing.
✅ **RESTful API** - Fully documented with Swagger.

## 🛠 Tech Stack

| Technology     | Description |
|---------------|------------|
| **C#** | Core programming language |
| **.NET Core** | Backend framework |
| **ASP.NET Web API** | RESTful API development |
| **SQL Server** | Database management |
| **Redis** | Distributed caching |
| **Serilog** | Logging and monitoring |
| **Swagger** | API documentation |

## 📂 Project Structure
```
ECommerceManagementSystem/
│── API/            # API Controllers
│── Application/    # Business Logic
│── Contracts/      # Interfaces for Abstraction
│── Domain/         # Entities & Core Logic
│── Infrastructure/  # Database & Caching
│── Persistence/    # Data Persistence Layer
│── Services/       # Business Services
│── Models/         # Data Models
```

## 🔧 Installation & Setup
1. **Clone the Repository**
```bash
git clone https://github.com/Adel-Mohamed25/E-Commerce.git
cd E-Commerce
```
2. **Configure Database** (Ensure SQL Server is running)
3. **Apply Migrations**
```bash
dotnet ef database update
```
4. **Run the Application**
```bash
dotnet run
```
5. **Access Swagger API Documentation**
```
https://localhost:44303/swagger/index.html
```

## 📖 API Endpoints
| Method | Endpoint | Description |
|--------|---------|-------------|
| **GET** | `/api/products` | Get all products |
| **POST** | `/api/auth/register` | Register a new user |
| **POST** | `/api/auth/login` | Login user |
| **PUT** | `/api/orders/{id}` | Update order |
| **DELETE** | `/api/orders/{id}` | Cancel order |

## 📜 License
This project is licensed under the MIT License.

## 🤝 Contributing
Contributions are welcome! Feel free to fork this repository and submit a pull request.

## 📞 Contact
📧 **Email:** [adelmohammedfayed@gmail.com](mailto:adelmohammedfayed@gmail.com)
🔗 **LinkedIn:** [Adel Mohamed](https://www.linkedin.com/in/adelmohamed25)
🐙 **GitHub:** [Adel-Mohamed25](https://github.com/Adel-Mohamed25)

---
Made with ❤️ by **Adel Mohamed Abdullah** 🚀

