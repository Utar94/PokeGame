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
import NotesTextarea from "@/components/shared/NotesTextarea.vue";
import ReferenceInput from "@/components/shared/ReferenceInput.vue";
import StatusDetail from "@/components/shared/StatusDetail.vue";
import UniqueNameInput from "@/components/shared/UniqueNameInput.vue";
import type { ApiError } from "@/types/api";
import type { Region, ReplaceRegionPayload } from "@/types/regions";
import { deleteRegion, readRegion, replaceRegion } from "@/api/regions";
import { formatRegion } from "@/helpers/displayUtils";
import { handleErrorKey } from "@/inject/App";
import { useToastStore } from "@/stores/toast";

const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();
const toasts = useToastStore();
const { t } = useI18n();

const description = ref<string>("");
const displayName = ref<string>("");
const isDeleting = ref<boolean>(false);
const region = ref<Region>();
const notes = ref<string>("");
const reference = ref<string>("");
const uniqueName = ref<string>("");

const formatted = computed<string>(() => (region.value ? formatRegion(region.value) : ""));
const hasChanges = computed<boolean>(
  () =>
    Boolean(region.value) &&
    (uniqueName.value !== (region.value?.uniqueName ?? "") ||
      displayName.value !== (region.value?.displayName ?? "") ||
      description.value !== (region.value?.description ?? "") ||
      reference.value !== (region.value?.reference ?? "") ||
      notes.value !== (region.value?.notes ?? "")),
);

async function onDelete(hideModal: () => void): Promise<void> {
  if (region.value && !isDeleting.value) {
    isDeleting.value = true;
    try {
      await deleteRegion(region.value.id);
      hideModal();
      toasts.success("regions.deleted");
      router.push({ name: "RegionList" });
    } catch (e: unknown) {
      handleError(e);
    } finally {
      isDeleting.value = false;
    }
  }
}

function setModel(model: Region): void {
  region.value = model;
  description.value = model.description ?? "";
  displayName.value = model.displayName ?? "";
  notes.value = model.notes ?? "";
  reference.value = model.reference ?? "";
  uniqueName.value = model.uniqueName;
}

const { handleSubmit, isSubmitting } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    if (region.value) {
      const payload: ReplaceRegionPayload = {
        uniqueName: uniqueName.value,
        displayName: displayName.value,
        description: description.value,
        reference: reference.value,
        notes: notes.value,
      };
      const updatedRegion: Region = await replaceRegion(region.value.id, payload, region.value.version);
      setModel(updatedRegion);
      toasts.success("regions.updated");
    }
  } catch (e: unknown) {
    handleError(e);
  }
});

onMounted(async () => {
  try {
    const id = route.params.id.toString();
    const region = await readRegion(id);
    setModel(region);
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
    <template v-if="region">
      <h1>{{ formatted }}</h1>
      <StatusDetail :aggregate="region" />
      <form @submit.prevent="onSubmit">
        <div class="mb-3">
          <AppSaveButton class="me-1" :disabled="isSubmitting || !hasChanges" exists :loading="isSubmitting" />
          <AppBackButton class="mx-1" :has-changes="hasChanges" />
          <AppDelete
            v-if="region"
            class="ms-1"
            confirm="regions.delete.confirm"
            :displayName="formatted"
            :loading="isDeleting"
            title="regions.delete.title"
            @confirmed="onDelete"
          />
        </div>
        <h3>{{ t("gameData") }}</h3>
        <div class="row">
          <UniqueNameInput class="col-lg-6" required v-model="uniqueName" />
          <DisplayNameInput class="col-lg-6" v-model="displayName" />
        </div>
        <DescriptionTextarea v-model="description" />
        <h3>{{ t("metadata") }}</h3>
        <ReferenceInput v-model="reference" />
        <NotesTextarea v-model="notes" />
      </form>
    </template>
  </main>
</template>
