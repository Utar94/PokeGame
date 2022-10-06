<template>
  <b-container>
    <h1 v-t="'users.recoverPassword.title'" />
    <b-alert v-if="success" show variant="success">
      <strong v-t="'users.recoverPassword.success'" />
      <br />
      {{ $t('users.recoverPassword.close') }}
    </b-alert>
    <template v-else>
      <b-alert show variant="info">{{ $t('users.recoverPassword.info') }}</b-alert>
      <validation-observer ref="form">
        <b-form @submit.prevent="submit">
          <username-field required v-model="username" />
          <icon-submit :disabled="!hasChanges || loading" icon="paper-plane" :loading="loading" text="users.recoverPassword.submit" variant="primary" />
        </b-form>
      </validation-observer>
    </template>
  </b-container>
</template>

<script>
import UsernameField from '@/components/Users/UsernameField.vue'
import { recoverPassword } from '@/api/account'

export default {
  name: 'RecoverPassword',
  components: {
    UsernameField
  },
  data() {
    return {
      loading: false,
      success: false,
      username: null
    }
  },
  computed: {
    hasChanges() {
      return Boolean(this.username)
    },
    payload() {
      return {
        locale: this.$i18n.locale,
        username: this.username
      }
    }
  },
  methods: {
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            await recoverPassword(this.payload)
            this.success = true
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  }
}
</script>
