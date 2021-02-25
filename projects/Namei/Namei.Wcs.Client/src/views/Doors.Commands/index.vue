<template>
  <AsyncLoader
    :handler="getDoors"
    style="padding: 1.25rem"
  >
    <div class="columns">
      <div class="column is-narrow">
        <div class="panel" style="width: 280px">
          <div class="panel-heading">
            设备名称
          </div>

          <a
            v-for="door in doors" :key="door.id"
            @click="doorId = door.id"
            class="panel-block"
            v-active="door.id === doorId"
          >
            {{door.type}} - {{door.id}}
          </a>
        </div>
      </div>

      <div class="column">
        <DoorView :doorId="doorId" />
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useWcsHttp } from "../../services/wcs-http";
import DoorView from "./DoorView.vue";

export default defineComponent({
  name: "DoorCommands",

  components: {
    DoorView
  },

  setup() {
    const http = useWcsHttp();

    const doorId = ref("101");
    const doors = ref<any[]>([]);

    async function getDoors() {
      const result = await http.post("/doors/states");

      doors.value = result;
    }

    return {
      doors,
      doorId,
      getDoors
    };
  }
});
</script>
