<template>
  <b-container>
    <h1 v-t="'users.resetPassword.title'" />
    <b-alert v-if="success" show variant="success">
      <strong v-t="'users.resetPassword.success'" />
      <br />
      {{ $t('users.resetPassword.signIn') }}
      <br />
      <icon-button href="/user/sign-in" icon="sign-in-alt" text="users.signIn.submit" variant="primary" />
    </b-alert>
    <template v-else>
      <b-alert show variant="info">{{ $t('users.resetPassword.info') }}</b-alert>
      <validation-observer ref="form">
        <b-form @submit.prevent="submit">
          <b-row>
            <password-field class="col" required validate v-model="password" />
            <password-field
              class="col"
              id="confirm"
              label="users.password.confirm.label"
              placeholder="users.password.confirm.placeholder"
              required
              :rules="{ confirmed: 'password' }"
              v-model="passwordConfirmation"
            />
          </b-row>
          <icon-submit :disabled="!hasChanges || loading" icon="key" :loading="loading" text="users.resetPassword.submit" variant="primary" />
        </b-form>
      </validation-observer>
    </template>
  </b-container>
</template>

<script>
import PasswordField from '@/components/Users/PasswordField.vue'
import { resetPassword } from '@/api/account'

export default {
  name: 'ResetPassword',
  components: {
    PasswordField
  },
  props: {
    token: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      loading: false,
      password: null,
      passwordConfirmation: null,
      success: false
    }
  },
  computed: {
    hasChanges() {
      return Boolean(this.password) || Boolean(this.passwordConfirmation)
    },
    payload() {
      return {
        token: this.token,
        password: this.password
      }
    }
  },
  methods: {
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            await resetPassword(this.payload)
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
