﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'ReadModel') THEN
        CREATE SCHEMA "ReadModel";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'ReadModel') THEN
        CREATE SCHEMA "ReadModel";
    END IF;
END $EF$;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "ReadModel"."Abilities" (
    "AbilityId" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(100) NOT NULL,
    "Description" text NULL,
    "Notes" text NULL,
    "Reference" character varying(2048) NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedOn" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedOn" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Abilities" PRIMARY KEY ("AbilityId")
);

CREATE TABLE "ReadModel"."Items" (
    "ItemId" integer GENERATED BY DEFAULT AS IDENTITY,
    "Category" integer NOT NULL DEFAULT 0,
    "DefaultModifier" double precision NULL,
    "Price" integer NULL,
    "Name" character varying(100) NOT NULL,
    "Description" text NULL,
    "Notes" text NULL,
    "Picture" character varying(2048) NULL,
    "Reference" character varying(2048) NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedOn" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedOn" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Items" PRIMARY KEY ("ItemId")
);

CREATE TABLE "ReadModel"."Moves" (
    "MoveId" integer GENERATED BY DEFAULT AS IDENTITY,
    "Type" integer NOT NULL DEFAULT 0,
    "Category" integer NOT NULL DEFAULT 0,
    "Name" character varying(100) NOT NULL,
    "Description" text NULL,
    "Accuracy" smallint NULL,
    "Power" smallint NULL,
    "PowerPoints" smallint NOT NULL DEFAULT 0,
    "StatusCondition" integer NULL,
    "StatusChance" smallint NULL,
    "StatisticStages" character varying(100) NULL,
    "AccuracyStage" smallint NOT NULL DEFAULT 0,
    "EvasionStage" smallint NOT NULL DEFAULT 0,
    "VolatileConditions" text NULL,
    "Notes" text NULL,
    "Reference" character varying(2048) NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedOn" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedOn" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Moves" PRIMARY KEY ("MoveId")
);

CREATE TABLE "ReadModel"."Species" (
    "SpeciesId" integer GENERATED BY DEFAULT AS IDENTITY,
    "Number" integer NOT NULL,
    "PrimaryType" integer NOT NULL DEFAULT 0,
    "SecondaryType" integer NULL,
    "Name" character varying(100) NOT NULL,
    "Category" character varying(100) NULL,
    "Description" text NULL,
    "GenderRatio" double precision NULL,
    "Height" double precision NULL,
    "Weight" double precision NULL,
    "BaseExperienceYield" integer NULL,
    "BaseFriendship" smallint NOT NULL DEFAULT 0,
    "CatchRate" smallint NULL,
    "LevelingRate" integer NOT NULL DEFAULT 0,
    "EggCycles" smallint NULL,
    "BaseStatistics" character varying(100) NULL,
    "EvYield" character varying(50) NULL,
    "Notes" text NULL,
    "Picture" character varying(2048) NULL,
    "Reference" character varying(2048) NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedOn" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedOn" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Species" PRIMARY KEY ("SpeciesId")
);

CREATE TABLE "ReadModel"."Users" (
    "UserId" integer GENERATED BY DEFAULT AS IDENTITY,
    "Username" character varying(256) NOT NULL,
    "Email" character varying(256) NULL,
    "FullName" character varying(256) NULL,
    "Locale" character varying(16) NULL,
    "Picture" character varying(2048) NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedOn" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedOn" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Users" PRIMARY KEY ("UserId")
);

CREATE TABLE "ReadModel"."Evolutions" (
    "EvolvingSpeciesId" integer NOT NULL,
    "EvolvedSpeciesId" integer NOT NULL,
    "Method" integer NOT NULL DEFAULT 0,
    "Gender" integer NULL,
    "HighFriendship" boolean NOT NULL DEFAULT FALSE,
    "ItemId" integer NULL,
    "Level" smallint NOT NULL DEFAULT 0,
    "Location" character varying(100) NULL,
    "MoveId" integer NULL,
    "Region" integer NULL,
    "TimeOfDay" integer NULL,
    "Notes" text NULL,
    CONSTRAINT "PK_Evolutions" PRIMARY KEY ("EvolvingSpeciesId", "EvolvedSpeciesId"),
    CONSTRAINT "FK_Evolutions_Items_ItemId" FOREIGN KEY ("ItemId") REFERENCES "ReadModel"."Items" ("ItemId"),
    CONSTRAINT "FK_Evolutions_Moves_MoveId" FOREIGN KEY ("MoveId") REFERENCES "ReadModel"."Moves" ("MoveId"),
    CONSTRAINT "FK_Evolutions_Species_EvolvedSpeciesId" FOREIGN KEY ("EvolvedSpeciesId") REFERENCES "ReadModel"."Species" ("SpeciesId") ON DELETE CASCADE,
    CONSTRAINT "FK_Evolutions_Species_EvolvingSpeciesId" FOREIGN KEY ("EvolvingSpeciesId") REFERENCES "ReadModel"."Species" ("SpeciesId") ON DELETE CASCADE
);

CREATE TABLE "ReadModel"."RegionalSpecies" (
    "SpeciesId" integer NOT NULL,
    "Region" integer NOT NULL,
    "Number" integer NOT NULL,
    CONSTRAINT "PK_RegionalSpecies" PRIMARY KEY ("SpeciesId", "Region"),
    CONSTRAINT "FK_RegionalSpecies_Species_SpeciesId" FOREIGN KEY ("SpeciesId") REFERENCES "ReadModel"."Species" ("SpeciesId") ON DELETE CASCADE
);

CREATE TABLE "ReadModel"."SpeciesAbilities" (
    "SpeciesId" integer NOT NULL,
    "AbilityId" integer NOT NULL,
    CONSTRAINT "PK_SpeciesAbilities" PRIMARY KEY ("SpeciesId", "AbilityId"),
    CONSTRAINT "FK_SpeciesAbilities_Abilities_AbilityId" FOREIGN KEY ("AbilityId") REFERENCES "ReadModel"."Abilities" ("AbilityId") ON DELETE CASCADE,
    CONSTRAINT "FK_SpeciesAbilities_Species_SpeciesId" FOREIGN KEY ("SpeciesId") REFERENCES "ReadModel"."Species" ("SpeciesId") ON DELETE CASCADE
);

CREATE TABLE "ReadModel"."Trainers" (
    "TrainerId" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserId" integer NULL,
    "Region" integer NOT NULL DEFAULT 0,
    "Number" integer NOT NULL,
    "Checksum" smallint NOT NULL DEFAULT 0,
    "Money" integer NOT NULL DEFAULT 0,
    "PlayTime" integer NOT NULL DEFAULT 0,
    "Gender" integer NOT NULL DEFAULT 0,
    "Name" character varying(100) NOT NULL,
    "Description" text NULL,
    "Notes" text NULL,
    "Picture" character varying(2048) NULL,
    "Reference" character varying(2048) NULL,
    "NationalPokedex" boolean NOT NULL DEFAULT FALSE,
    "PokedexCount" integer NOT NULL DEFAULT 0,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedOn" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedOn" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Trainers" PRIMARY KEY ("TrainerId"),
    CONSTRAINT "FK_Trainers_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "ReadModel"."Users" ("UserId")
);

CREATE TABLE "ReadModel"."Inventory" (
    "TrainerId" integer NOT NULL,
    "ItemId" integer NOT NULL,
    "Quantity" integer NOT NULL,
    CONSTRAINT "PK_Inventory" PRIMARY KEY ("TrainerId", "ItemId"),
    CONSTRAINT "FK_Inventory_Items_ItemId" FOREIGN KEY ("ItemId") REFERENCES "ReadModel"."Items" ("ItemId") ON DELETE CASCADE,
    CONSTRAINT "FK_Inventory_Trainers_TrainerId" FOREIGN KEY ("TrainerId") REFERENCES "ReadModel"."Trainers" ("TrainerId") ON DELETE CASCADE
);

CREATE TABLE "ReadModel"."Pokedex" (
    "TrainerId" integer NOT NULL,
    "SpeciesId" integer NOT NULL,
    "HasCaught" boolean NOT NULL,
    "UpdatedOn" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Pokedex" PRIMARY KEY ("TrainerId", "SpeciesId"),
    CONSTRAINT "FK_Pokedex_Species_SpeciesId" FOREIGN KEY ("SpeciesId") REFERENCES "ReadModel"."Species" ("SpeciesId") ON DELETE CASCADE,
    CONSTRAINT "FK_Pokedex_Trainers_TrainerId" FOREIGN KEY ("TrainerId") REFERENCES "ReadModel"."Trainers" ("TrainerId") ON DELETE CASCADE
);

CREATE TABLE "ReadModel"."Pokemon" (
    "PokemonId" integer GENERATED BY DEFAULT AS IDENTITY,
    "SpeciesId" integer NOT NULL,
    "AbilityId" integer NOT NULL,
    "Level" smallint NOT NULL,
    "Experience" integer NOT NULL DEFAULT 0,
    "Friendship" smallint NOT NULL DEFAULT 0,
    "RemainingHatchSteps" integer NOT NULL DEFAULT 0,
    "Gender" integer NOT NULL DEFAULT 0,
    "Nature" character varying(10) NOT NULL,
    "Characteristic" integer NOT NULL,
    "Surname" character varying(100) NULL,
    "Description" text NULL,
    "IndividualValues" character varying(100) NULL,
    "EffortValues" character varying(100) NULL,
    "Statistics" character varying(100) NULL,
    "CurrentHitPoints" integer NOT NULL DEFAULT 0,
    "StatusCondition" integer NULL,
    "HeldItemId" integer NULL,
    "BallId" integer NULL,
    "MetAtLevel" smallint NULL,
    "MetLocation" character varying(100) NULL,
    "MetOn" timestamp with time zone NULL,
    "CurrentTrainerId" integer NULL,
    "OriginalTrainerId" integer NULL,
    "Notes" text NULL,
    "Reference" character varying(2048) NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedOn" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedOn" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Pokemon" PRIMARY KEY ("PokemonId"),
    CONSTRAINT "FK_Pokemon_Abilities_AbilityId" FOREIGN KEY ("AbilityId") REFERENCES "ReadModel"."Abilities" ("AbilityId") ON DELETE CASCADE,
    CONSTRAINT "FK_Pokemon_Items_BallId" FOREIGN KEY ("BallId") REFERENCES "ReadModel"."Items" ("ItemId"),
    CONSTRAINT "FK_Pokemon_Items_HeldItemId" FOREIGN KEY ("HeldItemId") REFERENCES "ReadModel"."Items" ("ItemId"),
    CONSTRAINT "FK_Pokemon_Species_SpeciesId" FOREIGN KEY ("SpeciesId") REFERENCES "ReadModel"."Species" ("SpeciesId") ON DELETE CASCADE,
    CONSTRAINT "FK_Pokemon_Trainers_CurrentTrainerId" FOREIGN KEY ("CurrentTrainerId") REFERENCES "ReadModel"."Trainers" ("TrainerId"),
    CONSTRAINT "FK_Pokemon_Trainers_OriginalTrainerId" FOREIGN KEY ("OriginalTrainerId") REFERENCES "ReadModel"."Trainers" ("TrainerId")
);

CREATE TABLE "ReadModel"."PokemonMoves" (
    "PokemonId" integer NOT NULL,
    "MoveId" integer NOT NULL,
    "Position" smallint NOT NULL,
    "RemainingPowerPoints" smallint NOT NULL,
    CONSTRAINT "PK_PokemonMoves" PRIMARY KEY ("PokemonId", "MoveId"),
    CONSTRAINT "FK_PokemonMoves_Moves_MoveId" FOREIGN KEY ("MoveId") REFERENCES "ReadModel"."Moves" ("MoveId") ON DELETE CASCADE,
    CONSTRAINT "FK_PokemonMoves_Pokemon_PokemonId" FOREIGN KEY ("PokemonId") REFERENCES "ReadModel"."Pokemon" ("PokemonId") ON DELETE CASCADE
);

CREATE TABLE "ReadModel"."PokemonPositions" (
    "PokemonId" integer NOT NULL,
    "TrainerId" integer NOT NULL,
    "Position" smallint NOT NULL DEFAULT 0,
    "Box" smallint NOT NULL DEFAULT 0,
    CONSTRAINT "PK_PokemonPositions" PRIMARY KEY ("PokemonId"),
    CONSTRAINT "FK_PokemonPositions_Pokemon_PokemonId" FOREIGN KEY ("PokemonId") REFERENCES "ReadModel"."Pokemon" ("PokemonId") ON DELETE CASCADE,
    CONSTRAINT "FK_PokemonPositions_Trainers_TrainerId" FOREIGN KEY ("TrainerId") REFERENCES "ReadModel"."Trainers" ("TrainerId") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_Abilities_Id" ON "ReadModel"."Abilities" ("Id");

CREATE INDEX "IX_Abilities_Name" ON "ReadModel"."Abilities" ("Name");

CREATE INDEX "IX_Evolutions_EvolvedSpeciesId" ON "ReadModel"."Evolutions" ("EvolvedSpeciesId");

CREATE INDEX "IX_Evolutions_ItemId" ON "ReadModel"."Evolutions" ("ItemId");

CREATE INDEX "IX_Evolutions_MoveId" ON "ReadModel"."Evolutions" ("MoveId");

CREATE INDEX "IX_Inventory_ItemId" ON "ReadModel"."Inventory" ("ItemId");

CREATE UNIQUE INDEX "IX_Items_Id" ON "ReadModel"."Items" ("Id");

CREATE INDEX "IX_Items_Name" ON "ReadModel"."Items" ("Name");

CREATE UNIQUE INDEX "IX_Moves_Id" ON "ReadModel"."Moves" ("Id");

CREATE INDEX "IX_Moves_Name" ON "ReadModel"."Moves" ("Name");

CREATE INDEX "IX_Pokedex_HasCaught" ON "ReadModel"."Pokedex" ("HasCaught");

CREATE INDEX "IX_Pokedex_SpeciesId" ON "ReadModel"."Pokedex" ("SpeciesId");

CREATE INDEX "IX_Pokemon_AbilityId" ON "ReadModel"."Pokemon" ("AbilityId");

CREATE INDEX "IX_Pokemon_BallId" ON "ReadModel"."Pokemon" ("BallId");

CREATE INDEX "IX_Pokemon_CurrentTrainerId" ON "ReadModel"."Pokemon" ("CurrentTrainerId");

CREATE INDEX "IX_Pokemon_Gender" ON "ReadModel"."Pokemon" ("Gender");

CREATE INDEX "IX_Pokemon_HeldItemId" ON "ReadModel"."Pokemon" ("HeldItemId");

CREATE UNIQUE INDEX "IX_Pokemon_Id" ON "ReadModel"."Pokemon" ("Id");

CREATE INDEX "IX_Pokemon_OriginalTrainerId" ON "ReadModel"."Pokemon" ("OriginalTrainerId");

CREATE INDEX "IX_Pokemon_SpeciesId" ON "ReadModel"."Pokemon" ("SpeciesId");

CREATE INDEX "IX_Pokemon_Surname" ON "ReadModel"."Pokemon" ("Surname");

CREATE INDEX "IX_PokemonMoves_MoveId" ON "ReadModel"."PokemonMoves" ("MoveId");

CREATE UNIQUE INDEX "IX_PokemonMoves_PokemonId_Position" ON "ReadModel"."PokemonMoves" ("PokemonId", "Position");

CREATE UNIQUE INDEX "IX_PokemonPositions_TrainerId_Position_Box" ON "ReadModel"."PokemonPositions" ("TrainerId", "Position", "Box");

CREATE UNIQUE INDEX "IX_RegionalSpecies_Region_Number" ON "ReadModel"."RegionalSpecies" ("Region", "Number");

CREATE INDEX "IX_Species_Category" ON "ReadModel"."Species" ("Category");

CREATE UNIQUE INDEX "IX_Species_Id" ON "ReadModel"."Species" ("Id");

CREATE INDEX "IX_Species_Name" ON "ReadModel"."Species" ("Name");

CREATE UNIQUE INDEX "IX_Species_Number" ON "ReadModel"."Species" ("Number");

CREATE INDEX "IX_Species_PrimaryType" ON "ReadModel"."Species" ("PrimaryType");

CREATE INDEX "IX_Species_SecondaryType" ON "ReadModel"."Species" ("SecondaryType");

CREATE INDEX "IX_SpeciesAbilities_AbilityId" ON "ReadModel"."SpeciesAbilities" ("AbilityId");

CREATE INDEX "IX_Trainers_Gender" ON "ReadModel"."Trainers" ("Gender");

CREATE UNIQUE INDEX "IX_Trainers_Id" ON "ReadModel"."Trainers" ("Id");

CREATE INDEX "IX_Trainers_Name" ON "ReadModel"."Trainers" ("Name");

CREATE INDEX "IX_Trainers_Region" ON "ReadModel"."Trainers" ("Region");

CREATE UNIQUE INDEX "IX_Trainers_Region_Number_Name" ON "ReadModel"."Trainers" ("Region", "Number", "Name");

CREATE INDEX "IX_Trainers_UserId" ON "ReadModel"."Trainers" ("UserId");

CREATE UNIQUE INDEX "IX_Users_Id" ON "ReadModel"."Users" ("Id");

CREATE UNIQUE INDEX "IX_Users_Username" ON "ReadModel"."Users" ("Username");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221011025231_Release_0.1.0', '6.0.9');

COMMIT;
