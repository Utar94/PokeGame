# Pok�Game API

Pok�mon game management Web API.

## Migrations

This project is setup to use migrations. All the commands below must be executed in the solution directory.

### Create a migration

To create a new migration, execute the following command. Do not forget to specify a name!

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --project src/PokeGame.Infrastructure --startup-project src/PokeGame.Web --context EventContext`

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --project src/PokeGame.Infrastructure.ReadModel --startup-project src/PokeGame.Web --context ReadContext`

### Remove a migration

To remove the latest new migration, execute the following command.

`dotnet ef migrations remove --project src/PokeGame.Infrastructure --startup-project src/PokeGame.Web --context EventContext`

`dotnet ef migrations remove --project src/PokeGame.Infrastructure.ReadModel --startup-project src/PokeGame.Web --context ReadContext`

### Generate a script

To generate a script, execute the following command. You can optionally specify a _from_ migration name.

`dotnet ef migrations script <FROM_MIGRATION_NAME>? --project src/PokeGame.Infrastructure --startup-project src/PokeGame.Web --context EventContext`

`dotnet ef migrations script <FROM_MIGRATION_NAME>? --project src/PokeGame.Infrastructure.ReadModel --startup-project src/PokeGame.Web --context ReadContext`
