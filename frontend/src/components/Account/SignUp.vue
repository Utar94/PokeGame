<template>
  <b-container>
    <h1 v-t="'users.signUp.title'" />
    <b-alert show variant="info">
      {{ $t('users.signUp.info') }}
      <br />
      <strong v-text="email" />
    </b-alert>
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-row>
          <first-name-field class="col" required validate v-model="firstName" />
          <last-name-field class="col" required validate v-model="lastName" />
        </b-row>
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
        <icon-submit :disabled="!hasChanges || loading" icon="user" :loading="loading" text="users.signUp.submit" variant="primary" />
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import FirstNameField from '@/components/Users/FirstNameField.vue'
import LastNameField from '@/components/Users/LastNameField.vue'
import PasswordField from '@/components/Users/PasswordField.vue'
import { signUp } from '@/api/account'

export default {
  name: 'SignUp',
  components: {
    FirstNameField,
    LastNameField,
    PasswordField
  },
  props: {
    email: {
      type: String,
      required: true
    },
    token: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      firstName: null,
      lastName: null,
      loading: false,
      password: null,
      passwordConfirmation: null
    }
  },
  computed: {
    hasChanges() {
      return Boolean(this.firstName) || Boolean(this.lastName) || Boolean(this.password) || Boolean(this.passwordConfirmation)
    },
    payload() {
      return {
        token: this.token,
        password: this.password,
        firstName: this.firstName,
        lastName: this.lastName,
        locale: this.$i18n.locale
      }
    }
  },
  methods: {
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            await signUp(this.payload)
            window.location.replace('/user/profile')
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
