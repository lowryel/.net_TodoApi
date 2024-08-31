##### Migration in VSCode
* Migration in visual studio code using .NET framework and EF Core can ve done in the following way:
   - Ensure you have the necessary tools installed:
    - Run this command in your terminal to install the EF Core CLI tools globally:
`dotnet tool install --global dotnet-ef`

- Open your project in Visual Studio Code.
- Open the integrated terminal in VS Code (View > Terminal or Ctrl+`).
- Navigate to your project directory (where the .csproj file is located) if you're not already there.
- To create a new migration:
`dotnet ef migrations add YourMigrationName`


- Replace "YourMigrationName" with a descriptive name for your migration.
- To apply the migration to the database:
`dotnet ef database update`

- To list all migrations:
    `dotnet ef migrations list`

- To remove the last migration (if it hasn't been applied to the database):
`dotnet ef migrations remove`

- To generate SQL script for a migration (without applying it):
`dotnet ef migrations script`


***Additional tips:***

- If your DbContext is in a different project than your startup project, you'll need to specify both:
`dotnet ef migrations add YourMigrationName --project YourDataProject --startup-project YourWebProject`

- To target a specific database context (if you have multiple):
`dotnet ef migrations add YourMigrationName --context YourDbContext`

- If you're using a specific environment:
`dotnet ef database update --environment Production`



