# Pok�Game API

Pok�mon game management Web API.

## Migrations

This project is setup to use migrations. All the commands below must be executed in the target project.

Th target project for `EventContext` is **PokeGame.Infrastructure**.

Th target project for `ReadContext` is **PokeGame.Infrastructure.ReadModel**.

### Create a migration

To create a new migration, execute the following command. Do not forget to specify a name!

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --startup-project ../PokeGame.Web --context EventContext`

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --startup-project ../PokeGame.Web --context ReadContext`

### Remove a migration

To remove the latest new migration, execute the following command.

`dotnet ef migrations remove --startup-project ../PokeGame.Web --context EventContext`

`dotnet ef migrations remove --startup-project ../PokeGame.Web --context ReadContext`

### Generate a script

To generate a script, execute the following command. You can optionally specify a _from_ migration name.

`dotnet ef migrations script <FROM_MIGRATION_NAME>? --startup-project ../PokeGame.Web --context EventContext`

`dotnet ef migrations script <FROM_MIGRATION_NAME>? --startup-project ../PokeGame.Web --context ReadContext`
