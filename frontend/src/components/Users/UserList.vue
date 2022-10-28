<template>
  <div>
    <b-container>
      <h1 v-t="'users.title'" />
      <div class="my-2">
        <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
        <icon-button class="mx-1" href="/users/invite" icon="envelope" text="users.invite.submit" variant="success" />
        <icon-button
          class="mx-1"
          :disabled="loading"
          icon="cloud-download-alt"
          :loading="loading"
          text="users.synchronize.label"
          variant="warning"
          @click="synchronize"
        />
      </div>
      <b-row>
        <search-field class="col" v-model="search" />
        <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
        <count-select class="col" v-model="count" />
      </b-row>
      <p v-if="!users.length" v-t="'users.empty'" />
    </b-container>
    <b-container v-if="users.length" fluid>
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" v-t="'users.username.label'" />
            <th scope="col" v-t="'users.fullName'" />
            <th scope="col" v-t="'users.email.label'" />
            <th scope="col" v-t="'users.phone.label'" />
            <th scope="col" v-t="'users.passwordChangedOn'" />
            <th scope="col" v-t="'users.signedInOn'" />
            <th scope="col" v-t="'updated'" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.id">
            <td>
              <b-link :href="`/users/${user.id}`" target="_blank" class="mx-1"><user-avatar :user="user" /></b-link>
              <b-link :href="`/users/${user.id}`" target="_blank">{{ user.username }} <font-awesome-icon icon="external-link-alt" /></b-link>
            </td>
            <td v-text="user.fullName || '—'" />
            <td>
              {{ user.email || '—' }}
              <b-badge v-if="user.isEmailConfirmed" variant="info">{{ $t('users.email.confirmed') }}</b-badge>
            </td>
            <td>
              {{ user.phoneNumber || '—' }}
              <b-badge v-if="user.isPhoneNumberConfirmed" variant="info">{{ $t('users.phone.confirmed') }}</b-badge>
            </td>
            <td>{{ user.passwordChangedOn ? $d(new Date(user.passwordChangedOn), 'medium') : '—' }}</td>
            <td>{{ user.signedInOn ? $d(new Date(user.signedInOn), 'medium') : '—' }}</td>
            <td><status-cell :actor="user.updatedBy" :date="user.updatedOn" /></td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </b-container>
  </div>
</template>

<script>
import UserAvatar from './UserAvatar.vue'
import { getUsers, synchronizeUsers } from '@/api/users'

export default {
  name: 'UserList',
  components: {
    UserAvatar
  },
  data() {
    return {
      count: 10,
      desc: false,
      isConfirmed: true,
      isDisabled: false,
      loading: false,
      page: 1,
      search: null,
      sort: 'Username',
      total: 0,
      users: []
    }
  },
  computed: {
    params() {
      return {
        isConfirmed: this.isConfirmed,
        isDisabled: this.isDisabled,
        search: this.search,
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('users.sort.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    },
    yesNoOptions() {
      return [
        { text: this.$i18n.t('yes'), value: 'true' },
        { text: this.$i18n.t('no'), value: 'false' }
      ]
    }
  },
  methods: {
    async refresh(params = null) {
      if (!this.loading) {
        this.loading = true
        try {
          const { data } = await getUsers(params ?? this.params)
          this.users = data.items
          this.total = data.total
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    },
    async synchronize() {
      if (!this.loading) {
        this.loading = true
        try {
          await synchronizeUsers()
          this.toast('success', 'users.synchronize.success')
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
      }
    }
  },
  watch: {
    params: {
      deep: true,
      immediate: true,
      async handler(newValue, oldValue) {
        if (newValue?.index && oldValue && (newValue.search !== oldValue.search || newValue.count !== oldValue.count)) {
          this.page = 1
          await this.refresh()
        } else {
          await this.refresh(newValue)
        }
      }
    }
  }
}
</script>
