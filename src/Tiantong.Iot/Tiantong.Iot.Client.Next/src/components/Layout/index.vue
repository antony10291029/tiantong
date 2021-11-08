<template>
  <div
    class="
      fixed
      bg-dark-900 z-0
      text-gray-400
      flex flex-col
      h-screen w-screen
      overflow-hidden
    "
  >
    <!-- <TheNavbar
      :icon="icon"
      :isMenuShow="isMenuShow"
      :toggleMenu="toggleMenu"
    >
      <template #title>
        <slot name="title"></slot>
      </template>
    </TheNavbar> -->

    <div class="flex flex-row flex-auto w-full overflow-hidden">
      <TheSidebar
        :icon="icon"
        :isMenuShow="isMenuShow"
      >
        <template #title>
          <slot name="title"></slot>
        </template>

        <slot name="menu" :toggleMenu="toggleMenu"></slot>
      </TheSidebar>

      <slot name="body"></slot>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import TheSidebar from "./Sidebar.vue";

export default defineComponent({
  components: {
    TheSidebar
  },

  props: {
    icon: {
      type: String,
      default: "logo"
    },
  },

  setup() {
    const isMenuShow = ref(false);

    function toggleMenu(value?: boolean) {
      isMenuShow.value = value ?? !isMenuShow.value;
    }

    return {
      isMenuShow,
      toggleMenu,
    };
  }
});
</script>
