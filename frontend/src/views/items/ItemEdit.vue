<script setup lang="ts">
import { computed, inject, onMounted, ref } from "vue";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";
import { useRoute, useRouter } from "vue-router";

import AppBackButton from "@/components/shared/AppBackButton.vue";
import AppDelete from "@/components/shared/AppDelete.vue";
import AppSaveButton from "@/components/shared/AppSaveButton.vue";
import DescriptionTextarea from "@/components/shared/DescriptionTextarea.vue";
import DisplayNameInput from "@/components/shared/DisplayNameInput.vue";
import ItemCategorySelect from "@/components/items/ItemCategorySelect.vue";
import ItemIcon from "@/components/items/ItemIcon.vue";
import MedicinePropertiesEdit from "@/components/items/MedicinePropertiesEdit.vue";
import NotesTextarea from "@/components/shared/NotesTextarea.vue";
import PictureInput from "@/components/shared/PictureInput.vue";
import PokeBallPropertiesEdit from "@/components/items/PokeBallPropertiesEdit.vue";
import PriceInput from "@/components/items/PriceInput.vue";
import ReferenceInput from "@/components/shared/ReferenceInput.vue";
import StatusDetail from "@/components/shared/StatusDetail.vue";
import UniqueNameInput from "@/components/shared/UniqueNameInput.vue";
import type { ApiError } from "@/types/api";
import type { Item, MedicineProperties, PokeBallProperties, ReplaceItemPayload } from "@/types/items";
import { deleteItem, readItem, replaceItem } from "@/api/items";
import { formatItem } from "@/helpers/displayUtils";
import { handleErrorKey } from "@/inject/App";
import { useToastStore } from "@/stores/toast";

const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();
const toasts = useToastStore();
const { t } = useI18n();

const defaultMedicineProperties: MedicineProperties = {
  hitPointHealing: 0,
  isHitPointPercentage: false,
  doesReviveFainted: false,
  removeStatusCondition: "",
  removeAllStatusConditions: false,
  restorePowerPoints: 0,
  isPowerPointPercentage: false,
  restoreAllMoves: false,
  friendshipPenalty: 0,
};

const description = ref<string>("");
const displayName = ref<string>("");
const isDeleting = ref<boolean>(false);
const item = ref<Item>();
const medicine = ref<MedicineProperties>({ ...defaultMedicineProperties });
const notes = ref<string>("");
const picture = ref<string>("");
const pokeBall = ref<PokeBallProperties>({ catchRateModifier: 0 });
const price = ref<number>(0);
const reference = ref<string>("");
const uniqueName = ref<string>("");

const formatted = computed<string>(() => (item.value ? formatItem(item.value) : ""));
const hasChanges = computed<boolean>(
  () =>
    Boolean(item.value) &&
    (price.value !== (item.value?.price ?? 0) ||
      uniqueName.value !== (item.value?.uniqueName ?? "") ||
      displayName.value !== (item.value?.displayName ?? "") ||
      description.value !== (item.value?.description ?? "") ||
      picture.value !== (item.value?.picture ?? "") ||
      reference.value !== (item.value?.reference ?? "") ||
      notes.value !== (item.value?.notes ?? "") ||
      hasMedicineChanges.value ||
      hasPokeBallChanges.value),
);
const hasMedicineChanges = computed<boolean>(
  () =>
    item.value?.category === "Medicine" &&
    ((medicine.value.hitPointHealing ?? 0) !== (item.value?.medicine?.hitPointHealing ?? 0) ||
      medicine.value.isHitPointPercentage !== (item.value.medicine?.isHitPointPercentage ?? false) ||
      medicine.value.doesReviveFainted !== (item.value.medicine?.doesReviveFainted ?? false) ||
      (medicine.value.removeStatusCondition ?? "") !== (item.value.medicine?.removeStatusCondition ?? "") ||
      medicine.value.removeAllStatusConditions !== (item.value.medicine?.removeAllStatusConditions ?? false) ||
      (medicine.value.restorePowerPoints ?? 0) !== (item.value.medicine?.restorePowerPoints ?? 0) ||
      medicine.value.isPowerPointPercentage !== (item.value.medicine?.isPowerPointPercentage ?? false) ||
      medicine.value.restoreAllMoves !== (item.value.medicine?.restoreAllMoves ?? false) ||
      (medicine.value.friendshipPenalty ?? 0) !== (item.value.medicine?.friendshipPenalty ?? 0)),
);
const hasPokeBallChanges = computed<boolean>(
  () => item.value?.category === "PokeBall" && (pokeBall.value.catchRateModifier ?? 0) !== (item.value.pokeBall?.catchRateModifier ?? 0),
);

async function onDelete(hideModal: () => void): Promise<void> {
  if (item.value && !isDeleting.value) {
    isDeleting.value = true;
    try {
      await deleteItem(item.value.id);
      hideModal();
      toasts.success("items.deleted");
      router.push({ name: "ItemList" });
    } catch (e: unknown) {
      handleError(e);
    } finally {
      isDeleting.value = false;
    }
  }
}

function setModel(model: Item): void {
  item.value = model;
  description.value = model.description ?? "";
  displayName.value = model.displayName ?? "";
  notes.value = model.notes ?? "";
  picture.value = model.picture ?? "";
  price.value = model.price ?? 0;
  reference.value = model.reference ?? "";
  uniqueName.value = model.uniqueName;

  medicine.value =
    model.category === "Medicine"
      ? {
          hitPointHealing: model.medicine?.hitPointHealing ?? 0,
          isHitPointPercentage: model.medicine?.isHitPointPercentage ?? false,
          doesReviveFainted: model.medicine?.doesReviveFainted ?? false,
          removeStatusCondition: model.medicine?.removeStatusCondition ?? "",
          removeAllStatusConditions: model.medicine?.removeAllStatusConditions ?? false,
          restorePowerPoints: model.medicine?.restorePowerPoints ?? 0,
          isPowerPointPercentage: model.medicine?.isPowerPointPercentage ?? false,
          restoreAllMoves: model.medicine?.restoreAllMoves ?? false,
          friendshipPenalty: model.medicine?.friendshipPenalty ?? 0,
        }
      : { ...defaultMedicineProperties };
  pokeBall.value = model.category === "PokeBall" ? { catchRateModifier: model.pokeBall?.catchRateModifier ?? 0 } : {};
}

const { handleSubmit, isSubmitting } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    if (item.value) {
      const payload: ReplaceItemPayload = {
        price: price.value || undefined,
        uniqueName: uniqueName.value,
        displayName: displayName.value,
        description: description.value,
        picture: picture.value,
        medicine:
          item.value.category === "Medicine"
            ? {
                ...medicine.value,
                hitPointHealing: medicine.value.hitPointHealing || undefined,
                restorePowerPoints: medicine.value.restorePowerPoints || undefined,
                friendshipPenalty: medicine.value.friendshipPenalty || undefined,
              }
            : undefined,
        pokeBall: item.value.category === "PokeBall" ? { catchRateModifier: pokeBall.value.catchRateModifier || undefined } : undefined,
        reference: reference.value,
        notes: notes.value,
      };
      const updatedItem: Item = await replaceItem(item.value.id, payload, item.value.version);
      setModel(updatedItem);
      toasts.success("items.updated");
    }
  } catch (e: unknown) {
    handleError(e);
  }
});

onMounted(async () => {
  try {
    const id = route.params.id.toString();
    const item = await readItem(id);
    setModel(item);
  } catch (e: unknown) {
    const { status } = e as ApiError;
    if (status === 404) {
      router.push({ path: "/not-found" });
    } else {
      handleError(e);
    }
  }
});
</script>

<template>
  <main class="container">
    <template v-if="item">
      <h1><ItemIcon :item="item" /> {{ formatted }}</h1>
      <StatusDetail :aggregate="item" />
      <form @submit.prevent="onSubmit">
        <div class="mb-3">
          <AppSaveButton class="me-1" :disabled="isSubmitting || !hasChanges" exists :loading="isSubmitting" />
          <AppBackButton class="mx-1" :has-changes="hasChanges" />
          <AppDelete
            v-if="item"
            class="ms-1"
            confirm="items.delete.confirm"
            :displayName="formatted"
            :loading="isDeleting"
            title="items.delete.title"
            @confirmed="onDelete"
          />
        </div>
        <h3>{{ t("gameData") }}</h3>
        <div class="row">
          <ItemCategorySelect class="col-lg-6" disabled :model-value="item.category" />
          <PriceInput class="col-lg-6" v-model="price" />
        </div>
        <div class="row">
          <UniqueNameInput class="col-lg-6" required v-model="uniqueName" />
          <DisplayNameInput class="col-lg-6" v-model="displayName" />
        </div>
        <DescriptionTextarea v-model="description" />
        <PictureInput v-model="picture" />
        <MedicinePropertiesEdit v-if="item.category === 'Medicine'" v-model="medicine" />
        <PokeBallPropertiesEdit v-else-if="item.category === 'PokeBall'" v-model="pokeBall" />
        <h3>{{ t("metadata") }}</h3>
        <ReferenceInput v-model="reference" />
        <NotesTextarea v-model="notes" />
      </form>
    </template>
  </main>
</template>
