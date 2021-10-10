<template>
  <div
    :class="`
      container
      bg-dark-800
      h-full
      flex-shrink-0 shadow
      border-r border-dark-900
      overflow-y-auto overflow-x-hidden
      sm:block sm:w-72
      ${ isMenuShow ? 'w-screen' : 'hidden' }
    `"
  >
    <div class="p-4 py-3 hidden sm:flex items-center select-none">
      <img class="w-6" src="../../assets/logo.svg">
      <p class="text-grey-200 text-2xl ml-3">LinkFlow</p>
    </div>

    <p class="mb-4 sm:hidden"></p>

    <ul class="flex flex-col space-y-1 flex-auto">
      <li>
        <SidebarMenu
          route="/"
          text="工作台"
          @click="toggleMenu(false)"
        >
          <span
            class="
              px-2 py-0.5 ml-auto
              text-xs font-medium tracking-wide
              text-link-500 bg-primary-50 rounded-full
            "
          >
            PLC
          </span>
        </SidebarMenu>
      </li>
      <li>
        <SidebarMenu
          route="/workspace/settings"
          icon="settings"
          text="系统设置"
          @click="toggleMenu(false)"
        />
      </li>
      <div class="flex-auto"></div>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import SidebarMenu from "./SidebarMenu.vue";

export default defineComponent({
  components: {
    SidebarMenu
  },

  props: {
    isMenuShow: {
      type: Boolean,
      required: true
    },

    toggleMenu: {
      type: Function as PropType<(value?: boolean) => void>,
      required: true
    }
  },

  setup() {
    return {

    };
  }
});
</script>
