<template>
  <AsyncLoader :handler="getState">
    <PlcStateForm
      :handler="getState"
      :state="state"
    />

    <div class="is-flex" style="padding: 0.75rem 0">
      <div style="width: 100px"></div>

      <AsyncButton
        :handler="handleSave"
        :disabled="!isChanged"
        class="button is-info is-small"
        style="margin-right: 0.5rem"
      >
        保存
      </AsyncButton>

      <AsyncButton
        :handler="handleDelete"
        class="button is-danger is-light is-small"
      >
        删除
      </AsyncButton>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import isEqual from "lodash/isEqual";
import cloneDeep from "lodash/cloneDeep";
import PlcStateForm from "@/components/PlcStateForm.vue";
import { PlcState } from "@/entities";
import { useIotHttp } from "@/services/iot-http-client";
import { useConfirm } from "@midos/vue-ui";

export default defineComponent({
  name: "StateUpdate",

  components: {
    PlcStateForm
  },

  props: {
    stateId: {
      type: Number,
      required: true
    }
  },

  setup(props, { emit }) {
    const http = useIotHttp();
    const confirm = useConfirm();
    const state = ref(new PlcState());
    const sourceData = ref(new PlcState());
    const isChanged = computed(() => !isEqual(state.value, sourceData.value));

    async function getState() {
      const result = await http.post("/plcs/states/find", {
        state_id: props.stateId
      });

      state.value = result;
      sourceData.value = cloneDeep(state.value);
    }

    async function handleSave() {
      await http.post("/plcs/states/update", state.value);
      emit("refresh");
      getState();
    }

    function handleDelete() {
      confirm.open({
        title: "提示",
        content: "删除后将无法恢复",
        handler: async () => {
          await http.post("/plcs/states/delete", {
            state_id: props.stateId
          });

          emit("delete");
        }
      });
    }

    return {
      state,
      sourceData,
      isChanged,
      getState,
      handleSave,
      handleDelete,
    };
  }
});
</script>
