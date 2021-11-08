<template>
  <div class="flex flex-col flex-auto">
    <TheNavbar />

    <div class="p-4 flex-auto overflow-auto">

      <div class="mb-6 text-dark-300 flex items-center">
        <TheSearch v-model:value="query" />

        <TheCreate @created="getDataSource" />
      </div>

      <div
        class="
          flex-auto
          w-full h-auto rounded
          divide-y divide-dark-600
        "
      >
        <table class="table divide-y-2 divide-dark-600 w-full z-0 text-sm">
          <tbody class="divide-y divide-dark-800">
            <ListItem
              v-for="(plc, key) in listData" :key="plc.id"
              :index="key + 1"
              :plc="plc"
              @deleted="getDataSource"
            />
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, computed } from "vue";
import { PlcConfig, PlcConfigContext } from "../../domain";
import ListItem from "./ListItem.vue";
import TheSearch from "./Search.vue";
import TheCreate from "./TheCreate.vue";
import TheNavbar from "./Navbar.vue";

export default defineComponent({
  components: {
    TheNavbar,
    ListItem,
    TheSearch,
    TheCreate,
  },

  setup() {
    const data = ref<PlcConfig[]>([]);
    const query = ref("");
    const listData = computed(() => {
      if (query.value === "") {
        return data.value;
      }

      return data.value.filter(plc => plc.name.includes(query.value));
    });

    async function getDataSource() {
      const response = await PlcConfigContext.toArray();

      data.value = response.data;
    }

    onMounted(getDataSource);

    return {
      query,
      listData,
      getDataSource,
    };
  }
});
</script>
