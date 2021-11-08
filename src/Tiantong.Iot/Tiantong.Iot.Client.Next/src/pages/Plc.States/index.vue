<template>
  <div class="p-4 flex-auto overflow-auto">
    <p class="mb-4 text-2xl  mt-2">
      数据定义
    </p>

    <div class="mb-6 text-dark-300 flex items-center">
      <SearchField :value="query" />
    </div>

    <div
      class="
        flex-auto
        w-full h-auto rounded
        divide-y divide-dark-600
      "
    >
      <table class="table divide-y-2 divide-dark-600 w-full z-0 text-sm whitespace-nowrap">
        <tbody class="divide-y divide-dark-800">
          <ListItem
            v-for="(state, index) in dataSource" :key="state.id"
            :state="state"
            :index="index"
          />
        </tbody>
      </table>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from "vue";
import SearchField from "../../components/SearchField.vue";
import { PlcStateConfig, PlcStateConfigContext } from "../../domain";
import ListItem from "./ListItem.vue";

export default defineComponent({
  components: {
    SearchField,
    ListItem
  },

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const query = ref("");
    const dataSource = ref<PlcStateConfig[]>([]);
    const getDataSource = async () => {
      const result = await PlcStateConfigContext.toArray(props.plcId);

      dataSource.value = result.data;
    };

    onMounted(getDataSource);

    return {
      query,
      dataSource,
      getDataSource,
    };
  }
});
</script>
