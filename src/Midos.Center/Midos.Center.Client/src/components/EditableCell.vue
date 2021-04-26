<template>
  <td
    ref="td"
    v-if="!readonly"
    v-class:is-hoverable="hoverable"
    contenteditable
    v-text="value"
    @focus="handleFocus"
    @blur="handleChange"
    v-class:is-textarea="type === 'textarea'"
    @keypress.enter="type !== 'textarea' && handleChange($event)"
  />
  <td
    v-else
    v-text="value"
  />
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

export default defineComponent({
  name: "EditableCell",

  props: {
    value: {
      default: ""
    },

    type: {
      type: String,
      default: "text"
    },

    default: {
      default: ""
    },

    readonly: {
      type: Boolean,
      default: false
    },

    hoverable: {
      type: Boolean,
      default: true
    }
  },

  setup(props, { emit }) {
    const isFocused = ref(false);

    function getDefaultValue () {
      if (props.default !== "") {
        return props.default;
      }
      switch (props.type) {
        case "number": return 0;
        default: return "";
      }
    }

    function getValue (event: any) {
      let value = event.target.innerText;

      if (props.type !== "textarea") {
        value = value.trim();
      }

      if (value === "") {
        value = getDefaultValue();
      }

      if (props.type === "number") {
        value = parseFloat(value);

        if (Number.isNaN(value)) {
          value = 0;
        }
      }

      return event.target.innerText = value;
    }

    function handleFocus (event: any) {
      emit("focus", event);

      // isFocused.value = true;

      // const range = document.createRange() as any;
      // const selection = window.getSelection() as any;

      // range.selectNodeContents(event.target);

      // selection.removeAllRanges();
      // selection.addRange(range);
    }

    function handleChange (event: any) {
      const value = getValue(event);

      if (value !== props.value) {
        emit("update:value", value);
      }
    }

    return {
      isFocused,
      handleFocus,
      handleChange
    };
  }
});
</script>
