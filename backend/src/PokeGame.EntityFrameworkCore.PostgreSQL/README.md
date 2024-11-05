﻿# PokeGame.EntityFrameworkCore.SqlServer

Provides an implementation of a relational event store to be used with Pokémon game management Web application, Entity Framework Core and PostgreSQL.

## Migrations

This project is setup to use migrations. All the commands below must be executed in the solution directory.

### Create a migration

To create a new migration, execute the following command. Do not forget to provide a migration name!

```sh
dotnet ef migrations add <YOUR_MIGRATION_NAME> --context PokeGameContext --project src/PokeGame.EntityFrameworkCore.PostgreSQL --startup-project src/PokeGame
```

### Remove a migration

To remove the latest unapplied migration, execute the following command.

```sh
dotnet ef migrations remove --context PokeGameContext --project src/PokeGame.EntityFrameworkCore.SqlServer --startup-project src/PokeGame
```

### Generate a script

To generate a script, execute the following command. Do not forget to provide a source migration name!

```sh
dotnet ef migrations script <SOURCE_MIGRATION> --context PokeGameContext --project src/PokeGame.EntityFrameworkCore.SqlServer --startup-project src/PokeGame
```