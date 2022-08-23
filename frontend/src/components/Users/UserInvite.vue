<template>
  <b-container>
    <h1 v-t="'users.invite.title'" />
    <b-alert dismissible variant="warning" v-model="emailConflict">
      <strong v-t="'users.invite.failed'" />
      {{ $t('users.invite.emailConflict') }}
    </b-alert>
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <email-field placeholder="users.invite.emailPlaceholder" required validate v-model="email" />
        <icon-submit :disabled="!email || loading" icon="envelope" :loading="loading" text="users.invite.submit" variant="primary" />
      </b-form>
    </validation-observer>
  </b-container>
</template>

<script>
import EmailField from './EmailField.vue'
import { inviteUser } from '@/api/users'

export default {
  name: 'UserInvite',
  components: {
    EmailField
  },
  data() {
    return {
      email: null,
      emailConflict: false,
      loading: false
    }
  },
  computed: {
    payload() {
      return {
        email: this.email,
        locale: this.$i18n.locale
      }
    }
  },
  methods: {
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.emailConflict = false
        try {
          if (await this.$refs.form.validate()) {
            await inviteUser(this.payload)
            this.toast('success', 'users.invite.success')
            this.$refs.form.reset()
          }
        } catch (e) {
          const { data, status } = e
          if (status === 409 && data?.field === 'Email') {
            this.emailConflict = true
          } else {
            this.handleError(e)
          }
        } finally {
          this.email = null
          this.loading = false
        }
      }
    }
  }
}
</script>
