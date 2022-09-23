<template>
  <font-awesome-icon :class="{ 'text-success': success, 'text-danger': !success }" :icon="success ? 'check' : 'times'" />
</template>

<script>
export default {
  name: 'EvolutionCondition',
  props: {
    success: {
      type: Boolean,
      required: true
    }
  }
}
</script>
