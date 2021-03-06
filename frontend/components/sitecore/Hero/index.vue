<template>
  <div class="box">
    <h3 v-if="fields.title">
      <ScText :field="fields.title" />
    </h3>
    <p v-if="fields.description">
      <ScRichText :field="fields.description" />
    </p>
    <ScLink v-if="fields.link" :field="fields.link" />
  </div>
</template>

<script>
import { Link, Text, RichText } from '@sitecore-jss/sitecore-jss-vue'
export default {
  name: 'Hero',
  components: {
    ScLink: Link,
    ScText: Text,
    ScRichText: RichText
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

</style>
