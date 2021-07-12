<template>
  <div class="field has-addons">
    <slot></slot>
    <div class="control" :style="{ width }">
      <input
        v-model.lazy="inputValue"
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
          <i class="icon-midos icon-midos-search"></i>
        </span>
      </a>
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
      default: false
    },

    placeholder: {
      type: String,
      default: ""
    },

    width: {
      type: String,
      default: "160px"
    },

    value: {
      type: String,
      default: ""
    },
  },

  setup(props, { emit }) {
    const inputValue = ref(props.value);

    function handleSearch () {
      emit("search", inputValue.value);
    }

    function handleEnter (event: any) {
      inputValue.value = event.target.value;
      handleSearch();
    }

    return {
      inputValue,
      handleEnter,
      handleSearch,
    };
  }
});
</script>
