<template>
  <textarea
    :value="value"
    :rows="dynamicRows"
    @input="updateValue"
    v-bind="$attrs"
    class="textarea"
  ></textarea>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";

export default defineComponent({
  name: "TextArea",

  props: {
    rows: {
      type: Number,
      default: 4
    },

    value: {
      type: [String],
      required: true
    },

    default: {
      type: [String],
      default: ""
    }
  },

  setup(props, { emit }) {
    const dynamicRows = ref(props.rows);

    function asyncRows(value: string) {
      const rows = value.split(/\r*\n/).length;

      if (rows > props.rows) {
        dynamicRows.value = rows;
      }
    }

    function updateValue(event: any) {
      let { value } = event.target;

      if (value === "") {
        value = props.default;
      }

      emit("update:value", value);
      asyncRows(value);
    }

    onMounted(() => asyncRows(props.value));

    return {
      dynamicRows,
      updateValue
    };
  }
});
</script>
