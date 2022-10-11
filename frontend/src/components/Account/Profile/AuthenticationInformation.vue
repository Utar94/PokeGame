<template>
  <div>
    <h3 v-t="'users.information.authentication'" />
    <table class="table table-striped">
      <tbody>
        <tr>
          <th scope="row" v-t="'users.username.label'" />
          <td v-text="profile.username" />
        </tr>
      </tbody>
    </table>
    <h5 v-t="'users.password.label'" />
    <b-alert dismissible variant="warning" v-model="invalidCredentials">
      <strong v-t="'users.password.changeFailed'" />
      {{ $t('users.password.invalidCredentials') }}
    </b-alert>
    <p v-if="profile.passwordChangeOn">{{ $t('users.password.changedOn') }} {{ $d(new Date(profile.passwordChangedOn), 'medium') }}</p>
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <password-field
          id="current"
          label="users.password.current.label"
          placeholder="users.password.current.placeholder"
          ref="current"
          required
          v-model="current"
        />
        <b-row>
          <password-field class="col" label="users.password.new.label" placeholder="users.password.new.placeholder" required validate v-model="password" />
          <password-field
            class="col"
            id="confirm"
            label="users.password.confirm.label"
            placeholder="users.password.confirm.placeholder"
            required
            :rules="{ confirmed: 'password' }"
            v-model="confirm"
          />
        </b-row>
        <icon-submit :disabled="!hasChanges || loading" icon="key" :loading="loading" text="users.password.submit" variant="primary" />
      </b-form>
    </validation-observer>
  </div>
</template>

<script>
import PasswordField from '@/components/Users/PasswordField.vue'
import { changePassword } from '@/api/account'

export default {
  name: 'AuthenticationInformation',
  components: {
    PasswordField
  },
  props: {
    profile: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      confirm: null,
      current: null,
      invalidCredentials: false,
      loading: false,
      password: null
    }
  },
  computed: {
    hasChanges() {
      return Boolean(this.current) || Boolean(this.password) || Boolean(this.confirm)
    },
    payload() {
      return {
        current: this.current,
        password: this.password
      }
    }
  },
  methods: {
    reset() {
      this.current = null
      this.password = null
      this.confirm = null
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        this.invalidCredentials = false
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await changePassword(this.payload)
            this.reset()
            this.$refs.form.reset()
            this.$emit('updated', data)
            this.toast('success', 'users.password.changed')
          }
        } catch (e) {
          this.reset()
          this.$refs.current.focus()
          const { data, status } = e
          if (status === 400 && data?.code === 'InvalidCredentials') {
            this.invalidCredentials = true
          } else {
            this.handleError(e)
          }
        } finally {
          this.loading = false
        }
      }
    }
  }
}
</script>
