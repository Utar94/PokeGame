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
import type { Ability, ReplaceAbilityPayload } from "@/types/abilities";
import type { ApiError } from "@/types/api";
import { deleteAbility, readAbility, replaceAbility } from "@/api/abilities";
import { formatAbility } from "@/helpers/displayUtils";
import { handleErrorKey } from "@/inject/App";
import { useToastStore } from "@/stores/toast";

const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();
const toasts = useToastStore();
const { t } = useI18n();

const ability = ref<Ability>();
const description = ref<string>("");
const displayName = ref<string>("");
const isDeleting = ref<boolean>(false);
const notes = ref<string>("");
const reference = ref<string>("");
const uniqueName = ref<string>("");

const formatted = computed<string>(() => (ability.value ? formatAbility(ability.value) : ""));
const hasChanges = computed<boolean>(
  () =>
    Boolean(ability.value) &&
    (uniqueName.value !== (ability.value?.uniqueName ?? "") ||
      displayName.value !== (ability.value?.displayName ?? "") ||
      description.value !== (ability.value?.description ?? "") ||
      reference.value !== (ability.value?.reference ?? "") ||
      notes.value !== (ability.value?.notes ?? "")),
);

async function onDelete(hideModal: () => void): Promise<void> {
  if (ability.value && !isDeleting.value) {
    isDeleting.value = true;
    try {
      await deleteAbility(ability.value.id);
      hideModal();
      toasts.success("abilities.deleted");
      router.push({ name: "AbilityList" });
    } catch (e: unknown) {
      handleError(e);
    } finally {
      isDeleting.value = false;
    }
  }
}

function setModel(model: Ability): void {
  ability.value = model;
  description.value = model.description ?? "";
  displayName.value = model.displayName ?? "";
  notes.value = model.notes ?? "";
  reference.value = model.reference ?? "";
  uniqueName.value = model.uniqueName;
}

const { handleSubmit, isSubmitting } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    if (ability.value) {
      const payload: ReplaceAbilityPayload = {
        uniqueName: uniqueName.value,
        displayName: displayName.value,
        description: description.value,
        reference: reference.value,
        notes: notes.value,
      };
      const updatedAbility: Ability = await replaceAbility(ability.value.id, payload, ability.value.version);
      setModel(updatedAbility);
      toasts.success("abilities.updated");
    }
  } catch (e: unknown) {
    handleError(e);
  }
});

onMounted(async () => {
  try {
    const id = route.params.id.toString();
    const ability = await readAbility(id);
    setModel(ability);
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
    <template v-if="ability">
      <h1>{{ formatted }}</h1>
      <StatusDetail :aggregate="ability" />
      <form @submit.prevent="onSubmit">
        <div class="mb-3">
          <AppSaveButton class="me-1" :disabled="isSubmitting || !hasChanges" exists :loading="isSubmitting" />
          <AppBackButton class="mx-1" :has-changes="hasChanges" />
          <AppDelete
            v-if="ability"
            class="ms-1"
            confirm="abilities.delete.confirm"
            :displayName="formatted"
            :loading="isDeleting"
            title="abilities.delete.title"
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
