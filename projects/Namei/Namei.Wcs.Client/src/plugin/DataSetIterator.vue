<template>
  <template v-if="tag">
    <component
      v-for="key in dataSet.keys"
      :is="tag"
      :key="key"
    >
      <slot :entity="dataSet.entities[key]" />
    </component>
  </template>
  <template v-else>
    <template
      v-for="key in dataSet.keys"
      :key="key"
    >
      <slot :entity="dataSet.entities[key]" />
    </template>
  </template>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "DataSetIterator",

  props: {
    dataSet: {
      type: Object,
      required: true
    },

    tag: {
      type: String
    }
  }
});
</script>
