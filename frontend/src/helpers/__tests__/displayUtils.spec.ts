import { nanoid } from "nanoid";
import { describe, it, expect } from "vitest";

import { formatAbility } from "../displayUtils";
import type { Ability } from "@/types/abilities";
import type { Actor } from "@/types/actor";

const actor: Actor = {
  id: nanoid(),
  type: "User",
  isDeleted: false,
  displayName: "Alex (Master) Clark)",
  emailAddress: "master@pokegame.ca",
  pictureUrl: "https://cloudflare-ipfs.com/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/41.jpg",
};
const now: string = new Date().toISOString();

describe("formatAbility", () => {
  it.concurrent("should format correctly the ability with display name", () => {
    const ability: Ability = {
      id: nanoid(),
      version: 1,
      createdBy: actor,
      createdOn: now,
      updatedBy: actor,
      updatedOn: now,
      uniqueName: "AirLock",
      displayName: "Air Lock",
    };
    expect(formatAbility(ability)).toBe("Air Lock (AirLock)");
  });

  it.concurrent("should format correctly the ability without display name", () => {
    const ability: Ability = {
      id: nanoid(),
      version: 1,
      createdBy: actor,
      createdOn: now,
      updatedBy: actor,
      updatedOn: now,
      uniqueName: "AirLock",
      displayName: undefined,
    };
    expect(formatAbility(ability)).toBe("AirLock");
  });
});
