<template>
  <h1>Test title {{ fields.title.value }}</h1>
</template>

<script>
export default {
  name: 'TitleMain',
  props: {
    fields: {
      type: Object
    }
  }
}
</script>

<style scoped>

</style>
