<template>
  <div>
    <h3 v-t="'users.information.personal'" />
    <table class="table table-striped">
      <tbody>
        <tr v-if="profile.fullName">
          <th scope="row" v-t="'users.fullName'" />
          <td v-text="profile.fullName" />
        </tr>
        <tr v-if="profile.email">
          <th scope="row" v-t="'users.email.label'" />
          <td>
            {{ profile.email }}
            <b-badge v-if="profile.isEmailConfirmed" variant="info">{{ $t('users.email.confirmed') }}</b-badge>
          </td>
        </tr>
        <tr v-if="profile.phoneNumber">
          <th scope="row" v-t="'users.phone.label'" />
          <td>
            {{ profile.phoneNumber }}
            <b-badge v-if="profile.isPhoneNumberConfirmed" variant="info">{{ $t('users.phone.confirmed') }}</b-badge>
          </td>
        </tr>
        <tr>
          <th scope="row" v-t="'users.createdOn'" />
          <td>{{ $d(new Date(profile.createdOn), 'medium') }}</td>
        </tr>
        <tr v-if="profile.updatedOn">
          <th scope="row" v-t="'users.updatedOn'" />
          <td>{{ $d(new Date(profile.updatedOn), 'medium') }}</td>
        </tr>
        <tr v-if="profile.signedInOn">
          <th scope="row" v-t="'users.signedInOn'" />
          <td>{{ $d(new Date(profile.signedInOn), 'medium') }}</td>
        </tr>
      </tbody>
    </table>
    <validation-observer ref="form">
      <b-form @submit.prevent="submit">
        <b-row>
          <first-name-field class="col" validate v-model="firstName" />
          <last-name-field class="col" validate v-model="lastName" />
        </b-row>
        <b-row>
          <locale-select class="col" v-model="locale" />
          <picture-field class="col" validate v-model="picture" />
        </b-row>
        <div class="mb-2">
          <icon-submit :disabled="!hasChanges || loading" icon="save" :loading="loading" text="actions.save" variant="primary" />
        </div>
      </b-form>
    </validation-observer>
  </div>
</template>

<script>
import FirstNameField from '@/components/Users/FirstNameField.vue'
import LastNameField from '@/components/Users/LastNameField.vue'
import PictureField from '@/components/Users/PictureField.vue'
import { saveProfile } from '@/api/account'

export default {
  name: 'PersonalInformation',
  components: {
    FirstNameField,
    LastNameField,
    PictureField
  },
  props: {
    profile: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      firstName: null,
      lastName: null,
      loading: false,
      locale: null,
      middleName: null,
      picture: null
    }
  },
  computed: {
    hasChanges() {
      return (
        (this.firstName ?? '') !== (this.profile.firstName ?? '') ||
        (this.lastName ?? '') !== (this.profile.lastName ?? '') ||
        this.locale !== this.profile.locale ||
        (this.picture ?? '') !== (this.profile.picture ?? '')
      )
    },
    payload() {
      return {
        firstName: this.firstName,
        lastName: this.lastName,
        middleName: this.middleName,
        locale: this.locale,
        picture: this.picture || null
      }
    }
  },
  methods: {
    setModel(profile) {
      this.firstName = profile.firstName
      this.lastName = profile.lastName
      this.locale = profile.locale
      this.middleName = profile.middleName
      this.picture = profile.picture
    },
    async submit() {
      if (!this.loading) {
        this.loading = true
        try {
          if (await this.$refs.form.validate()) {
            const { data } = await saveProfile(this.payload)
            this.setModel(data)
            this.toast('success', 'users.profile.updated')
            this.$refs.form.reset()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    profile: {
      deep: true,
      immediate: true,
      handler(profile) {
        this.setModel(profile)
      }
    }
  }
}
</script>
