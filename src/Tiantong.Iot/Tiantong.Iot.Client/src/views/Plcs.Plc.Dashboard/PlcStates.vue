<template>
  <AsyncLoader
    class="is-flex-auto box"
    :handler="getStates"
  >
    <Table
      colspan="5"
      class="table is-centered is-nowrap is-fullwidth"
    >
      <template #head>
        <thead>
          <th>数据名</th>
          <th>地址</th>
          <th>类型</th>
          <th>数据</th>
          <th>操作</th>
        </thead>
      </template>
      <template #body>
        <tbody v-if="states.length">
          <tr v-for="state in states" :key="state.id">
            <td>{{state.name}}</td>
            <td>{{state.address}}</td>
            <td>
              {{state.type + (state.type === 'string' ? `(${state.length * 2})` : '')}}
            </td>
            <td>{{currentValues[state.name]}}</td>
            <PlcStateSetValue
              v-if="isRunning"
              :plc="plc"
              :state="state"
              :type="state.type"
              style="width: 1px"
            />
            <td v-else></td>
          </tr>
        </tbody>
      </template>
    </Table>
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref, toRefs } from "vue";
import { useIotHttp } from "../../services/iot-http-client";
import { useInterval } from "../../hooks/use-interval";
import PlcStateSetValue from "./PlcStateSetValue.vue";

export default defineComponent({
  name: "PlcDashboardStates",

  components: {
    PlcStateSetValue
  },

  props: {
    plc: {
      type: Object,
      required: true
    },

    plcId: {
      type: Number,
      required: true
    },

    isRunning: {
      type: Boolean,
      required: true
    },

    isDataWatchOpen: {
      type: Boolean,
      required: true
    }
  },

  setup(props) {
    const http = useIotHttp();
    const isPending = ref(false);
    const states = ref([] as any[]);
    const currentValues = ref({} as any);

    const getStates = async () => {
      const data = await http.dataArray("plcs/states/all", {
        plc_id: props.plcId
      });

      states.value = data;
    };

    const getCurrentValues = async () => {
      if (isPending.value === true) {
        return;
      }

      isPending.value = true;

      try {
        const response = await http.post("/plc-states/all-values", {
          plc: props.plc.name
        });

        currentValues.value = response;
      } finally {
        isPending.value = false;
      }
    };

    useInterval(getCurrentValues, toRefs(props).isRunning);

    return {
      isPending,
      states,
      currentValues,
      getStates,
      getCurrentValues
    };
  }
});
</script>
