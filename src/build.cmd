del Migrations\*.cs
dotnet ef migrations add InitialCreate --context AppDbContext
dotnet ef database update --context AppDbContext
