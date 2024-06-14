<script setup lang="ts">
import { TarAlert, TarButton } from "logitar-vue3-ui";
import { ref } from "vue";
import { useForm } from "vee-validate";
import { useI18n } from "vue-i18n";

import AppInput from "@/components/shared/AppInput.vue";
import EmailAddressInput from "@/components/users/EmailAddressInput.vue";
import PasswordInput from "@/components/users/PasswordInput.vue";
import type { ApiError, Error } from "@/types/api";
import type { Credentials, SignInPayload, SignInResponse } from "@/types/account";
import { signIn } from "@/api/account";

const { locale, t } = useI18n();
const defaultEmailAddress: string | undefined = import.meta.env.VITE_APP_DEFAULT_EMAIL;

const credentials = ref<Credentials>({ emailAddress: defaultEmailAddress ?? "" });
const invalidCredentials = ref<boolean>(false);
const isPasswordRequired = ref<boolean>(false);
const passwordRef = ref<InstanceType<typeof AppInput> | null>(null);

const emit = defineEmits<{
  (e: "error", value: unknown): void;
  (e: "response", value: SignInResponse): void;
}>();

const { handleSubmit, isSubmitting } = useForm();
const onSubmit = handleSubmit(async () => {
  try {
    invalidCredentials.value = false;
    const payload: SignInPayload = {
      locale: locale.value,
      credentials: credentials.value,
    };
    const response: SignInResponse = await signIn(payload);
    if (response.isPasswordRequired) {
      isPasswordRequired.value = true;
    } else {
      emit("response", response);
    }
  } catch (e: unknown) {
    const { data, status } = e as ApiError;
    if (status === 400 && (data as Error)?.code === "InvalidCredentials") {
      invalidCredentials.value = true;
      credentials.value.password = "";
      passwordRef.value?.focus();
    } else {
      emit("error", e);
    }
  }
});
</script>

<template>
  <div>
    <form @submit.prevent="onSubmit">
      <h1>{{ t("users.signIn.title") }}</h1>
      <TarAlert :close="t('actions.close')" dismissible variant="warning" v-model="invalidCredentials">
        <strong>{{ t("users.signIn.failed") }}</strong> {{ t("users.signIn.invalidCredentials") }}
      </TarAlert>
      <EmailAddressInput required v-model="credentials.emailAddress" />
      <PasswordInput v-if="isPasswordRequired" required ref="passwordRef" v-model="credentials.password" />
      <TarButton
        :disabled="isSubmitting"
        icon="fas fa-arrow-right-to-bracket"
        :loading="isSubmitting"
        :status="t('loading')"
        :text="t('users.signIn.submit')"
        type="submit"
      />
    </form>
  </div>
</template>
