<template>
  <div>
    <h1 class="title">
      <ScText :field="fields.title" />
    </h1>
    <h2 class="subtitle">
      <ScText :field="fields.subtitle" />
    </h2>
  </div>
</template>

<script>
import { Text } from '@sitecore-jss/sitecore-jss-vue'
export default {
  name: 'TitleMain',
  components: {
    ScText: Text
  },
  props: {
    fields: {
      type: Object,
      default: () => ({})
    }
  }
}
</script>

<style scoped>
.title {
  font-family:
    'Quicksand',
    'Source Sans Pro',
    -apple-system,
    BlinkMacSystemFont,
    'Segoe UI',
    Roboto,
    'Helvetica Neue',
    Arial,
    sans-serif;
  display: block;
  font-weight: 300;
  font-size: 100px;
  color: #35495e;
  letter-spacing: 1px;
}

.subtitle {
  font-weight: 300;
  font-size: 38px;
  color: #526488;
  word-spacing: 5px;
  padding-bottom: 15px;
}
</style>
