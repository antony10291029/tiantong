<template>
  <div class="app-notifications is-flex is-flex-column is-vcentered">
    <Notification
      v-for="item in items" :key="item.id"
      @close="remove(item.id)"
      v-bind="item"
    ></Notification>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import Notification from "./Notification.vue";

export default defineComponent({
  name: "Notifications",

  components: {
    Notification
  },

  setup() {
    const count = ref(1);
    const items = ref<any[]>([]);

    function open(text: string, type: string, duration: number) {
      items.value.push({ text, type, duration, id: count.value++ });
    }

    function remove(id: number) {
      const index = items.value.findIndex(item => item.id === id);
      if (index !== -1) {
        items.value.splice(index, 1);
      }
    }

    return {
      count,
      items,
      open,
      remove,
    };
  },
});
</script>
