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
      <i class="iconfont icon-plc w-6 text-primary-500 text-xl"></i>
      <p class="text-grey-200 text-xl ml-3">
        <slot name="title"></slot>
      </p>
    </div>

    <p class="mb-4 sm:hidden"></p>

    <ul class="flex flex-col space-y-1 flex-auto">
      <li>
        <SidebarMenu
          :route="{ name: 'PlcSettings', params: { plcId } }"
          icon="settings"
          text="配置管理"
          @click="toggleMenu(false)"
        />
      </li>
      <li>
        <SidebarMenu
          route="/"
          text="返回工作台"
          icon="back"
          @click="toggleMenu(false)"
        />
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import SidebarMenu from "./SidebarMenu.vue";
import { PlcConfig } from "../../domain";

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
    },

    plcId: {
      type: Number,
      required: true
    },

    plcConfig: {
      type: Object as PropType<PlcConfig>,
      required: true
    }
  },

  setup() {
    return {

    };
  }
});
</script>
