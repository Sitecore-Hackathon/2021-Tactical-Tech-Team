<template>
  <div class="box">
    <h3 v-if="fields.title" class="title">
      <ScText :field="fields.title" />
    </h3>
    <p v-if="fields.description" class="description">
      <ScRichText :field="fields.description" />
    </p>
    <ScLink v-if="fields.link" :field="fields.link" class="button--grey button-small" />
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
.box {
  text-align: left;
  margin: 25px 25px;
  padding: 15px 20px;
  box-shadow: 0 10px 20px rgba(0,0,0,0.19), 0 6px 6px rgba(0,0,0,0.23);
}

.title {
  color: #35495e;
  font-size: 28px;

}

.description {
  color: #526488;
  margin-top: 5px;
}

.button-small {
  border-color: goldenrod;
  color: goldenrod;
  margin: 15px 0;
}
</style>
