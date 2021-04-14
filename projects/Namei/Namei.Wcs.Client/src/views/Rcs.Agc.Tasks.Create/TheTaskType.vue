<template>
  <div class="field">
    <label class="label">任务类型</label>
    <div class="field has-addons control">
      <div
        v-for="operation in operations"
        :key="operation.value"
        class="control"
      >
        <a
          v-class:is-focused="value == operation.value"
          @click="updateValue(operation.value)"
          class="button is-small"
        >
          {{operation.text}}
        </a>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

const operations = [
  { text: "平层移动", value: "wcs.move" },
  { text: "转移货架", value: "wcs.carry" },
  { text: "举起货架", value: "wcs.lift" },
  { text: "放下货架", value: "wcs.put" },
];

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

    if (!props.value) {
      updateValue(operations[0].value);
    }

    return {
      operations,
      updateValue
    };
  }
});
</script>
