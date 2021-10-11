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
    <template v-if="plcConfig">
      <TheNavbar
        :plcConfig="plcConfig"
        :isMenuShow="isMenuShow"
        :toggleMenu="toggleMenu"
      >
        <template #title>
          <span>{{plcConfig.name}}</span>
        </template>
      </TheNavbar>

      <div class="flex flex-row flex-auto w-full overflow-hidden">
        <TheSidebar
          :plcId="plcId"
          :plcConfig="plcConfig"
          :isMenuShow="isMenuShow"
          :toggleMenu="toggleMenu"
        >
          <template #title>
            <span>{{plcConfig.name}}</span>
          </template>
        </TheSidebar>

        <div class="is-flex-auto w-full">
          <router-view
            :plcId="plcId"
            :plcConfig="plcConfig"
            @updated="getPlc"
          />
        </div>
      </div>
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import TheNavbar from "./Navbar.vue";
import TheSidebar from "./Sidebar.vue";
import { PlcConfig, PlcConfigContext } from "../../domain";

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
    const plcConfig = ref<PlcConfig>();

    function toggleMenu(value?: boolean) {
      isMenuShow.value = value ?? !isMenuShow.value;
    }

    const getPlc = async () => {
      const response = await PlcConfigContext.getById(props.plcId);

      plcConfig.value = response.data;
    };

    onMounted(getPlc);

    return {
      isMenuShow,
      toggleMenu,
      plcConfig,
      getPlc
    };
  }
});
</script>
