### Migration in Entity Framework Core
# PMC Command	dotnet CLI command	Usage

add-migration <migration name>	Add <migration name>	Creates a migration by adding a migration snapshot.
dotnet ef migrations add MyFirstMigration

Update-database	Update	Updates the database schema based on the last migration snapshot.
dotnet ef database update

Remove-migration	Remove	Removes the last migration snapshot.
dotnet ef migrations remove

