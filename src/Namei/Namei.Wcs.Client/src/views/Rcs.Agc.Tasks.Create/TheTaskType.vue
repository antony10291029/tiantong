<template>
  <div class="field">
    <label class="label">任务类型</label>
    <div class="control">
      <Input
        :value="value"
        @update:value="updateValue"
      />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "TyeTaskType",

  props: {
    value: {
      type: String,
    }
  },

  setup(props, { emit }) {
    function updateValue(value: string) {
      emit("update:value", value);
    }

    return {
      updateValue
    };
  }
});
</script>
