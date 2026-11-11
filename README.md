# ctis465-blogs

CTIS 465 Microservices Project - Blogs

This is a simple blog microservice written with ASP.NET Core Web API (.NET 8). Users can create blogs and add tags to them. We used Entity Framework Core with SQLite for the database and MediatR for the CRUD operations.

## How to Run

1. Open `BlogsMS.sln` in Visual Studio.
2. Set `Blogs.API` as the startup project.
3. Run the project. Swagger will open in the browser.
4. (Optional) Call `GET /api/SeedDb` to fill the database with sample data.

Or from the terminal:

```
cd Blogs.API
dotnet run
```

Then go to `http://localhost:5298/swagger`.

## Team Members

- Mevlüt Yılmaz
- Semih Şengül

## Projects

- CORE
- Blogs.APP
- Blogs.API

## Entities

- Blog
- Tag
- BlogTag
