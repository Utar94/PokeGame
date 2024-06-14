<script setup lang="ts">
import { inject, onMounted, ref } from "vue";
import { useI18n } from "vue-i18n";

import ProfileHeader from "@/components/users/ProfileHeader.vue";
import type { UserProfile } from "@/types/account";
import { getProfile } from "@/api/account";
import { handleErrorKey } from "@/inject/App";

const handleError = inject(handleErrorKey) as (e: unknown) => void;
const { t } = useI18n();

const user = ref<UserProfile>();

onMounted(async () => {
  try {
    user.value = await getProfile();
  } catch (e: unknown) {
    handleError(e);
  }
});
</script>

<template>
  <main class="container">
    <template v-if="user">
      <h1>{{ t("users.profile.title") }}</h1>
      <ProfileHeader :user="user" />
    </template>
  </main>
</template>
