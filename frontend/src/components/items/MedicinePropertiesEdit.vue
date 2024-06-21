<script setup lang="ts">
import { useI18n } from "vue-i18n";

import FriendshipPenaltyInput from "./FriendshipPenaltyInput.vue";
import HitPointHealingInput from "./HitPointHealingInput.vue";
import RemoveStatusConditionInput from "./RemoveStatusConditionInput.vue";
import RestorePowerPointInput from "./RestorePowerPointInput.vue";
import type { MedicineProperties } from "@/types/items";

const { t } = useI18n();

const props = withDefaults(
  defineProps<{
    modelValue?: MedicineProperties;
  }>(),
  {
    modelValue: () => ({
      hitPointHealing: 0,
      isHitPointPercentage: false,
      doesReviveFainted: false,
      removeStatusCondition: "",
      removeAllStatusConditions: false,
      restorePowerPoints: 0,
      isPowerPointPercentage: false,
      restoreAllMoves: false,
      friendshipPenalty: 0,
    }),
  },
);

const emit = defineEmits<{
  (e: "update:model-value", value: MedicineProperties): void;
}>();

function setProperty(key: keyof MedicineProperties, value: boolean | number | string | undefined): void {
  const properties: MedicineProperties = { ...props.modelValue };
  switch (key) {
    case "doesReviveFainted":
      properties.doesReviveFainted = (value as boolean) ?? false;
      break;
    case "friendshipPenalty":
      properties.friendshipPenalty = (value as number) ?? 0;
      break;
    case "hitPointHealing":
      properties.hitPointHealing = (value as number) ?? 0;
      break;
    case "isHitPointPercentage":
      properties.isHitPointPercentage = (value as boolean) ?? false;
      break;
    case "isPowerPointPercentage":
      properties.isPowerPointPercentage = (value as boolean) ?? false;
      break;
    case "removeAllStatusConditions":
      properties.removeAllStatusConditions = (value as boolean) ?? false;
      if (properties.removeAllStatusConditions) {
        properties.removeStatusCondition = "";
      }
      break;
    case "removeStatusCondition":
      properties.removeStatusCondition = (value as string) ?? "";
      break;
    case "restoreAllMoves":
      properties.restoreAllMoves = (value as boolean) ?? false;
      break;
    case "restorePowerPoints":
      properties.restorePowerPoints = (value as number) ?? false;
      break;
  }
  emit("update:model-value", properties); // TODO(fpion): there's a bug when changing a check box after any input
}
</script>

<template>
  <div>
    <h3>{{ t("items.properties.label") }}</h3>
    <div class="row">
      <HitPointHealingInput
        class="col-lg-6"
        :model-value="modelValue.hitPointHealing"
        :percentage="modelValue.isHitPointPercentage"
        :revive="modelValue.doesReviveFainted"
        @percentage="setProperty('isHitPointPercentage', $event)"
        @revive="setProperty('doesReviveFainted', $event)"
        @update:model-value="setProperty('hitPointHealing', $event)"
      />
      <RemoveStatusConditionInput
        class="col-lg-6"
        :all="modelValue.removeAllStatusConditions"
        :model-value="modelValue.removeStatusCondition"
        @all="setProperty('removeAllStatusConditions', $event)"
        @update:model-value="setProperty('removeStatusCondition', $event)"
      />
    </div>
    <div class="row">
      <RestorePowerPointInput
        class="col-lg-6"
        :all-moves="modelValue.restoreAllMoves"
        :model-value="modelValue.restorePowerPoints"
        :percentage="modelValue.isPowerPointPercentage"
        @all-moves="setProperty('restoreAllMoves', $event)"
        @percentage="setProperty('isPowerPointPercentage', $event)"
        @update:model-value="setProperty('restorePowerPoints', $event)"
      />
      <FriendshipPenaltyInput class="col-lg-6" :model-value="modelValue.friendshipPenalty" @update:model-value="setProperty('friendshipPenalty', $event)" />
    </div>
  </div>
</template>
