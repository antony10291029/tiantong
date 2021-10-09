<template>
  <div
    class="
      fixed
      bg-dark-900 z-0
      text-dark-300
      flex flex-col
      h-screen w-screen
      overflow-hidden
    "
  >
    <TheNavbar
      :isMenuShow="isMenuShow"
      :toggleMenu="toggleMenu"
    />

    <div class="flex flex-row flex-auto w-full overflow-hidden">
      <TheSidebar
        :isMenuShow="isMenuShow"
        :toggleMenu="toggleMenu"
      />

      <router-view />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import TheNavbar from "./Navbar.vue";
import TheSidebar from "./Sidebar.vue";

export default defineComponent({
  components: {
    TheNavbar,
    TheSidebar
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
