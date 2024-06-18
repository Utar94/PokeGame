<script setup lang="ts">
import { TarButton, TarModal } from "logitar-vue3-ui";
import { computed, ref } from "vue";
import { nanoid } from "nanoid";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";
import { useRouter } from "vue-router";

import UniqueNameInput from "@/components/shared/UniqueNameInput.vue";
import type { ApiError, PropertyError } from "@/types/api";
import type { CreateRegionPayload, Region } from "@/types/regions";
import { createRegion } from "@/api/regions";
import { useToastStore } from "@/stores/toast";

const router = useRouter();
const toasts = useToastStore();
const { t } = useI18n();

const props = withDefaults(
  defineProps<{
    close?: string;
    id?: string;
  }>(),
  {
    close: "actions.close",
    id: () => nanoid(),
  },
);

const modalRef = ref<InstanceType<typeof TarModal> | null>(null);
const payload = ref<CreateRegionPayload>({ uniqueName: "" });

const modalId = computed<string>(() => `delete-modal_${props.id}`);

function hide(): void {
  modalRef.value?.hide();
}

const emit = defineEmits<{
  (e: "error", value: unknown): void;
}>();

const { handleSubmit, isSubmitting, resetForm } = useForm();
function onCancel(): void {
  resetForm();
  hide();
}
const onSubmit = handleSubmit(async () => {
  try {
    const createdRegion: Region = await createRegion(payload.value);
    hide();
    toasts.success("regions.created");
    router.push({ name: "RegionEdit", params: { id: createdRegion.id } });
  } catch (e: unknown) {
    const { status, data } = e as ApiError;
    if (status === 409 && (data as PropertyError)?.code === "UniqueNameAlreadyUsed") {
      toasts.warning("uniqueName.alreadyUsed");
    } else {
      emit("error", e);
    }
  }
});
</script>

<template>
  <span>
    <TarButton icon="fas fa-plus" :text="t('actions.create')" variant="success" data-bs-toggle="modal" :data-bs-target="`#${modalId}`" />
    <TarModal :close="t(close)" :id="modalId" ref="modalRef" :title="t('regions.title.new')">
      <form @submit.prevent="onSubmit">
        <UniqueNameInput required v-model="payload.uniqueName" />
      </form>
      <template #footer>
        <TarButton icon="fas fa-ban" :text="t('actions.cancel')" variant="secondary" @click="onCancel" />
        <TarButton
          :disabled="isSubmitting"
          icon="fas fa-plus"
          :loading="isSubmitting"
          :status="t('loading')"
          :text="t('actions.create')"
          variant="success"
          @click="onSubmit"
        />
      </template>
    </TarModal>
  </span>
</template>
