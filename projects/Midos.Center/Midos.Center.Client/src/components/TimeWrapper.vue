<template>
  <component
    :is="tag"
    :class="text === '无' && 'is-italic has-text-grey-light'"
  >
    {{text}}
  </component>
</template>

<script lang="ts">
import { defineComponent, computed } from "vue";
import { DateTime } from "@midos/vue-ui";

export default defineComponent({
  name: "TimeWrapper",

  props: {
    value: {
      type: String,
      required: true
    },

    tag: {
      type: String,
      default: "span"
    },

    default: {
      type: String,
      default: "无"
    }
  },

  setup(props) {
    const isMinValue = computed(() =>
      props.value === DateTime.minValue
    );
    const text = computed(() => {
      return isMinValue.value
        ? props.default
        : props.value.substr(0, 10);
    });

    return {
      text,
      isMinValue
    };
  }
});
</script>
