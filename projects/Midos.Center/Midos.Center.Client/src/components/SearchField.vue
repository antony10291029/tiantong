<template>
  <div>
    <div class="field has-addons">
      <slot></slot>
      <div class="control" :style="{ width }">
        <input
          v-model.lazy="value"
          type="text" class="input"
          :placeholder="placeholder"
          @keypress.enter="handleEnter"
        >
      </div>
      <div class="control">
        <a
          v-loading="isPending"
          @click="handleSearch"
          class="button is-info"
        >
          <span class="icon">
            <i class="icon-midos-center icon-midos-center-search"></i>
          </span>
        </a>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

export default defineComponent({
  name: "SearchFeidl",

  props: {
    isPending: {
      type: Boolean,
      required: true
    },

    placeholder: {
      type: String,
      default: ""
    },

    width: {
      type: String,
      default: "160px"
    }
  },

  setup(props, { emit }) {
    const value = ref("");

    function handleSearch () {
      emit("search", value.value);
    }

    function handleEnter (event: any) {
      value.value = event.target.value;
      handleSearch();
    }

    return {
      value,
      handleEnter,
      handleSearch,
    };
  }
});
</script>
