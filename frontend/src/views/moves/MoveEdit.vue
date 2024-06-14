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
import type { Move, ReplaceMovePayload } from "@/types/moves";
import { formatMove } from "@/helpers/displayUtils";
import { handleErrorKey } from "@/inject/App";
import { readMove, replaceMove } from "@/api/moves";
import { useToastStore } from "@/stores/toast";

const handleError = inject(handleErrorKey) as (e: unknown) => void;
const route = useRoute();
const router = useRouter();
const toasts = useToastStore();
const { t } = useI18n();

const description = ref<string>("");
const displayName = ref<string>("");
const isDeleting = ref<boolean>(false);
const move = ref<Move>();
const notes = ref<string>("");
const reference = ref<string>("");
const uniqueName = ref<string>("");

const formatted = computed<string>(() => (move.value ? formatMove(move.value) : ""));
const hasChanges = computed<boolean>(
  () =>
    Boolean(move.value) &&
    (uniqueName.value !== (move.value?.uniqueName ?? "") ||
      displayName.value !== (move.value?.displayName ?? "") ||
      description.value !== (move.value?.description ?? "") ||
      reference.value !== (move.value?.reference ?? "") ||
      notes.value !== (move.value?.notes ?? "")),
);

async function onDelete(hideModal: () => void): Promise<void> {
  if (move.value && !isDeleting.value) {
    isDeleting.value = true;
    try {
      alert("Deleting moves is not yet implemented."); // await deleteMove(move.value.id); // TODO(fpion): delete move
      hideModal();
      toasts.success("moves.deleted");
      router.push({ name: "MoveList" });
    } catch (e: unknown) {
      handleError(e);
    } finally {
      isDeleting.value = false;
    }
  }
}

function setModel(model: Move): void {
  move.value = model;
  description.value = model.description ?? "";
  displayName.value = model.displayName ?? "";
  notes.value = model.notes ?? "";
  reference.value = model.reference ?? "";
  uniqueName.value = model.uniqueName;
}

const { handleSubmit, isSubmitting } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    if (move.value) {
      const payload: ReplaceMovePayload = {
        uniqueName: uniqueName.value,
        displayName: displayName.value,
        description: description.value,
        accuracy: undefined, // TODO(fpion): implement
        power: undefined, // TODO(fpion): implement
        powerPoints: 0, // TODO(fpion): implement
        statisticChanges: [], // TODO(fpion): implement
        statusConditions: [], // TODO(fpion): implement
        reference: reference.value,
        notes: notes.value,
      };
      const updatedMove: Move = await replaceMove(move.value.id, payload, move.value.version);
      setModel(updatedMove);
      toasts.success("moves.updated");
    }
  } catch (e: unknown) {
    handleError(e);
  }
});

onMounted(async () => {
  try {
    const id = route.params.id.toString();
    const move = await readMove(id);
    setModel(move);
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
    <template v-if="move">
      <h1>{{ formatted }}</h1>
      <StatusDetail :aggregate="move" />
      <form @submit.prevent="onSubmit">
        <div class="mb-3">
          <AppSaveButton class="me-1" :disabled="isSubmitting || !hasChanges" exists :loading="isSubmitting" />
          <AppBackButton class="mx-1" :has-changes="hasChanges" />
          <AppDelete
            v-if="move"
            class="ms-1"
            confirm="moves.delete.confirm"
            :displayName="formatted"
            :loading="isDeleting"
            title="moves.delete.title"
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
