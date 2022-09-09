<template>
  <font-awesome-icon :icon="icon" />
</template>

<script>
export default {
  name: 'GenderIcon',
  props: {
    gender: {
      type: String,
      required: true
    }
  },
  computed: {
    icon() {
      switch (this.gender) {
        case 'Male':
          return 'mars'
        case 'Female':
          return 'venus'
        default:
          return 'neuter'
      }
    }
  }
}
</script>
