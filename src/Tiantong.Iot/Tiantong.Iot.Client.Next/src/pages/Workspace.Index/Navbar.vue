<template>
  <div
    class="mx-4 mb-2 flex border-b items-center"
    style="border-color: #353535; height: 50px"
  >
    <p class="nav-item">
      首页
    </p>

    <p class="nav-item px-2">
      >
    </p>

    <p class="nav-item">
      企业管理
    </p>

    <div class="flex-auto"></div>

    <p
      :class="`nav-tab ${item === currentItem && 'active'}`"
      v-for="item in items" :key="item"
      @click="currentItem = item"
    >
      {{item}}
    </p>

    <div class="flex-auto"></div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

export default defineComponent({
  setup() {
    const items = ["整体情况", "任务管理", "部门日历", "统计报表"];
    const currentItem = ref("任务管理");

    return {
      items,
      currentItem,
    };
  }
});
</script>

<style lang="postcss">
.nav-item, .nav-tab {
  @apply relative;
  @apply flex items-center h-full box-border;
  @apply border-b border-transparent;
  bottom: -1px;
}

.nav-tab {
  @apply px-3 mx-1;
  @apply cursor-pointer hover:text-gray-300;
  @apply justify-self-center
}

.nav-tab.active {
  @apply border-dark-300 text-dark-300
}
</style>
