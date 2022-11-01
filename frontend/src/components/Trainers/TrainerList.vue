<template>
  <b-container>
    <h1 v-t="'trainers.title'" />
    <div class="my-2">
      <icon-button class="mx-1" :disabled="loading" icon="sync-alt" :loading="loading" text="actions.refresh" variant="primary" @click="refresh()" />
      <icon-button class="mx-1" href="/create-trainer" icon="plus" text="actions.create" variant="success" />
    </div>
    <b-row>
      <gender-select class="col" v-model="gender" />
      <region-select class="col" v-model="region" />
      <user-select class="col" v-model="userId" />
    </b-row>
    <b-row>
      <search-field class="col" v-model="search" />
      <sort-select class="col" :desc="desc" :options="sortOptions" v-model="sort" @desc="desc = $event" />
      <count-select class="col" v-model="count" />
    </b-row>
    <template v-if="trainers.length">
      <table id="table" class="table table-striped">
        <thead>
          <tr>
            <th scope="col" />
            <th scope="col" v-t="'name.label'" />
            <th scope="col" v-t="'users.select.label'" />
            <th scope="col" v-t="'trainers.number'" />
            <th scope="col" v-t="'regions.select.label'" />
            <th scope="col" v-t="'updated'" />
            <th scope="col" />
          </tr>
        </thead>
        <tbody>
          <tr v-for="trainer in trainers" :key="trainer.id">
            <td>
              <b-link :href="`/trainers/${trainer.id}`"><trainer-icon :trainer="trainer" /></b-link>
            </td>
            <td>
              <b-link :href="`/trainers/${trainer.id}`"><gender-icon :gender="trainer.gender" /> {{ trainer.name }}</b-link>
            </td>
            <td>
              <template v-if="trainer.user">
                <b-link :href="`/users/${trainer.user.id}`" target="_blank"><user-avatar :user="trainer.user" /></b-link>
                {{ ' ' }}
                <b-link :href="`/users/${trainer.user.id}`" target="_blank">{{ trainer.user.name }} <font-awesome-icon icon="external-link-alt" /></b-link>
              </template>
              <template v-else>&mdash;</template>
            </td>
            <td v-text="trainer.number" />
            <td v-text="trainer.region ? trainer.region.name : '—'" />
            <td><status-cell :actor="trainer.updatedBy || trainer.createdBy" :date="trainer.updatedOn || trainer.createdOn" /></td>
            <td>
              <icon-button disabled icon="trash-alt" text="actions.delete" variant="danger" v-b-modal="`delete_${trainer.id}`" />
              <delete-modal
                confirm="trainers.delete.confirm"
                :displayName="trainer.name"
                :id="`delete_${trainer.id}`"
                :loading="loading"
                title="trainers.delete.title"
                @ok="onDelete(trainer, $event)"
              />
            </td>
          </tr>
        </tbody>
      </table>
      <b-pagination v-model="page" :total-rows="total" :per-page="count" aria-controls="table" />
    </template>
    <p v-else v-t="'trainers.empty'" />
  </b-container>
</template>

<script>
import GenderSelect from './GenderSelect.vue'
import RegionSelect from '@/components/Regions/RegionSelect.vue'
import TrainerIcon from './TrainerIcon.vue'
import UserAvatar from '@/components/Users/UserAvatar.vue'
import UserSelect from '@/components/Users/UserSelect.vue'
import { deleteTrainer, getTrainers } from '@/api/trainers'

export default {
  name: 'TrainerList',
  components: {
    GenderSelect,
    RegionSelect,
    TrainerIcon,
    UserAvatar,
    UserSelect
  },
  data() {
    return {
      count: 10,
      desc: false,
      gender: null,
      loading: false,
      page: 1,
      region: null,
      search: null,
      sort: 'Name',
      total: 0,
      trainers: [],
      userId: null
    }
  },
  computed: {
    params() {
      return {
        gender: this.gender,
        regionId: this.region?.id ?? null,
        search: this.search,
        userId: this.userId,
        sort: this.sort,
        desc: this.desc,
        index: (this.page - 1) * this.count,
        count: this.count
      }
    },
    sortOptions() {
      return this.orderBy(
        Object.entries(this.$i18n.t('trainers.sort.options')).map(([value, text]) => ({ text, value })),
        'text'
      )
    }
  },
  methods: {
    async onDelete({ id }, callback = null) {
      if (!this.loading) {
        this.loading = true
        let refresh = false
        try {
          await deleteTrainer(id)
          refresh = true
          this.toast('success', 'trainers.delete.success')
          if (typeof callback === 'function') {
            callback()
          }
        } catch (e) {
          this.handleError(e)
        } finally {
          this.loading = false
        }
        if (refresh) {
          await this.refresh()
        }
      }
      if (callback) {
        callback()
      }
    },
    async refresh(params = null) {
      if (!this.loading) {
        this.loading = true
        try {
          const { data } = await getTrainers(params ?? this.params)
          this.trainers = data.items
          this.total = data.total
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
        if (
          newValue?.index &&
          oldValue &&
          (newValue.search !== oldValue.search ||
            newValue.gender !== oldValue.gender ||
            (newValue.region?.id ?? null) !== (oldValue.region?.id ?? null) ||
            newValue.userId !== oldValue.userId ||
            newValue.count !== oldValue.count)
        ) {
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
