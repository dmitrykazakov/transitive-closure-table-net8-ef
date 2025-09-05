cd Migrations
del *.cs
cd ..
dotnet ef migrations add InitialCreate --context AppDbContext
dotnet ef database update --context AppDbContext
