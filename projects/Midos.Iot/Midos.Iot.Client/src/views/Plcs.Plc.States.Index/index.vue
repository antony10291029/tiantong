<template>
  <AsyncLoader
    :handler="getStates"
    style="padding: 1.25rem"
  >
    <div class="columns">
      <div class="column is-narrow">
        <div
          class="box is-paddingless"
          style="min-width: 400px"
        >
          <p
            class="is-flex has-border-bottom"
            style="padding: 0.5rem 0.75rem"
          >
            <span class="label">数据点</span>
            <span class="is-flex-auto"></span>
            <router-link
              class="is-size-6"
              :to="{ name: 'IotPlcsPlcStatesCreate', params: { plcId } }"
            >
              添加
            </router-link>
          </p>

          <Table
            colspan="4"
            class="table is-fullwidth is-nowrap is-hoverable is-radius"
          >
            <template #body>
              <tbody v-if="states.length > 0">
                <tr
                  v-class:is-active="state.id === stateId"
                  v-for="state in states" :key="state.id"
                  @click="handleStateClick(state)"
                >
                    <td class="is-centered">{{state.number}}</td>
                    <td>{{state.name}}</td>
                    <td>{{state.address}}</td>
                    <td>
                      {{state.type + (state.type === 'string' ? `(${state.length * 2})` : '')}}
                    </td>
                    <td class="is-centered">
                      <span
                        class="icon"
                        v-if="state.is_collect"
                      >
                        <i class="icon-iot icon-iot-collect"></i>
                      </span>
                      <span
                        class="icon"
                        style="margin-left: 0.25rem"
                        v-if="state.is_heartbeat"
                      >
                        <i class="icon-iot icon-iot-heartbeat"></i>
                      </span>
                    </td>
                </tr>
              </tbody>
            </template>
          </Table>
        </div>
      </div>

      <div class="column">
        <router-view
          :key="`${plcId}/${stateId}`"
          :plcId="plcId"
          :stateId="stateId"
          @refresh="getStates"
          @delete="handleStateDeleted"
        />
      </div>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useIotHttp } from "../../services/iot-http-client";

export default defineComponent({
  name: "PlcStates",

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const http = useIotHttp();
    const route = useRoute();
    const router = useRouter();
    const states = ref<any[]>([]);
    const stateId = computed(() => +route.params.stateId);

    async function getStates () {
      const result = await http.dataArray("plcs/states/all", {
        plc_id: props.plcId
      });

      states.value = result;
    }

    function handleStateClick(state: any) {
      if (!state || +route.params.stateId === state.id) return;

      router.push({
        name: "IotPlcsPlcStatesStateDetail",
        params: {
          plcId: props.plcId,
          stateId: state.id
        }
      });
    }

    function handleStateDeleted() {
      router.push({
        name: "IotPlcsPlcStatesIndex",
        params: { plcId: props.plcId } }
      );

      getStates();
    }

    return {
      states,
      stateId,
      getStates,
      handleStateClick,
      handleStateDeleted,
    };
  },
});
</script>
