<template>
  <template v-if="tag">
    <component
      v-for="key in dataMap.keys"
      :is="tag"
      :key="key"
    >
      <slot :entity="dataMap.entities[key]" />
    </component>
  </template>
  <template v-else>
    <template
      v-for="key in dataMap.keys"
      :key="key"
    >
      <slot :entity="dataMap.entities[key]" />
    </template>
  </template>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "DataSetIterator",

  props: {
    dataMap: {
      type: Object,
      required: true
    },

    tag: {
      type: String
    }
  }
});
</script>
