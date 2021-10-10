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
        :plcId="plcId"
        :isMenuShow="isMenuShow"
        :toggleMenu="toggleMenu"
      />

      <div class="is-flex-auto">
        <router-view :plcId="plcId" />
      </div>
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

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const isMenuShow = ref(false);
    const plc = ref<any>(null);

    function toggleMenu(value?: boolean) {
      isMenuShow.value = value ?? !isMenuShow.value;
    }

    return {
      isMenuShow,
      toggleMenu,
      plc,
    };
  }
});
</script>
