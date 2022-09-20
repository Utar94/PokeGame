<template>
  <b-avatar v-if="trainer.picture" :alt="trainer.name" :src="trainer.picture" variant="light" />
  <b-avatar v-else :alt="trainer.name"><font-awesome-icon icon="id-card" /></b-avatar>
</template>

<script>
export default {
  name: 'TrainerIcon',
  props: {
    trainer: {
      type: Object,
      required: true
    }
  }
}
</script>
