<template>
  <input
    :value="value"
    v-bind="$attrs"
    class="input"
    @input="updateValue"
  >
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Input",

  props: {
    value: {
      type: [String, Number],
      required: true
    },

    type: {
      type: String,
      default: "text",
    },

    default: {
      default: ""
    }
  },

  setup(props, { emit }) {
    function updateValue(event: any) {
      let { value } = event.target;

      if (props.type === "int" || props.type === "integer") {
        value = parseInt(value, 10);
      }

      if (props.type === "number" || props.type === "float" || props.type === "double") {
        value = parseFloat(value);
      }

      if (Number.isNaN(value)) {
        value = props.default === "" ? 0 : props.default;
      }

      if (value === "") {
        value = props.default;
      }

      emit("update:value", value);
    }

    return {
      updateValue
    };
  }
});
</script>
