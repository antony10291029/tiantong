<template>
  <div
    class="is-flex is-flex-column"
  >
    <div
      class="tabs"
      style="margin-bottom: 0; font-size: 1.125rem; flex-shrink: 0"
    >
      <ul>
        <router-link
          custom
          v-for="(tab, key) in tabs" :key="key"
          :to="{ name: tab.route }"
          v-slot="{ navigate, isActive }"
        >
          <li
            v-class:is-active="isActive"
            @click="navigate"
          >
            <a>{{tab.text}}</a>
          </li>
        </router-link>
      </ul>
    </div>

    <router-view
      class="is-flex-auto"
      style="overflow-y: auto; overflow-x: auto"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";

export default defineComponent({
  name: "Lifters",

  props: {
    tabs: {
      type: Object as PropType<Array<any>>,
      default: () => ([])
    }
  },
});
</script>
