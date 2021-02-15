<template>
  <label
    :class="labelClass"
    :style="labelStyle"
  >
    <span
      class="is-unselectable"
      style="cursor: pointer"
      @click="handleClick"
    >
      <span
        :class="spanClass"
        :style="spanStyle"
      >
        <i
          :class="'iconfont vue-bulma-checkbox-' + iconName"
          :style="iconStyle"
        />
      </span>
      <span class="is-unselectable" style="cursor: pointer">
        <slot></slot>
      </span>
    </span>
  </label>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    size: String,
    inline: Boolean,
    fontSize: String,
    disabled: Boolean,
    value: [Boolean, String]
  },
  computed: {
    iconName(): string {
      switch (this.value) {
        case true:
          return "checked";
        case false:
          return "unchecked";
        case "minus":
          return "minus";
        default:
          return "";
      }
    },

    labelClass(): string[] {
      const result = [] as string[];

      if (this.disabled) {
        result.push("has-text-grey-light");
      }

      return result;
    },

    labelStyle(): any {
      return {
        "margin-left": "-0.25rem",
        display: this.inline ? "inline" : "block"
      };
    },

    spanClass(): any {
      let color = "info";

      if (this.disabled) color = "grey-lighter";
      else if (this.value === false) color = "grey-light";

      return [
        "icon",
        `is-${this.size}`,
        `has-text-${color}`
      ];
    },

    spanStyle () {
      const style = {} as any;

      if (this.fontSize) {
        style.width = style.height = this.fontSize;
      }

      return style;
    },

    iconStyle () {
      const result = {} as any;
      if (this.fontSize) {
        result["font-size"] = this.fontSize;
      }

      return result;
    }
  },
  methods: {
    handleClick () {
      if (!this.disabled) {
        this.$emit("update:value", Boolean(!this.value));
      }

      this.$emit("click");
    }
  }
});
</script>
