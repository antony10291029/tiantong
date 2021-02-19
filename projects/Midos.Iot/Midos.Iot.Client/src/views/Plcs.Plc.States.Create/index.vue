<template>
  <div class="box is-paddingless">
    <div class="tabs mb-0">
      <ul>
        <li class="is-active">
          <a>创建数据点</a>
        </li>
      </ul>
    </div>

    <div style="padding: 1.25rem">
      <PlcStateForm :state="state" />

      <div class="is-flex" style="padding: 0.75rem 0">
        <div style="width: 100px"></div>

        <AsyncButton
          class="button is-info is-small"
          style="margin-right: 0.5rem"
          :handler="handleSubmit"
        >
          提交
        </AsyncButton>

        <a
          @click="$router.go(-1)"
          class="button is-info is-light is-small"
        >
          返回
        </a>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import { PlcState } from "../../entities";
import { useIotHttp } from "../../services/iot-http-client";
import PlcStateForm from "../../components/PlcStateForm.vue";

export default defineComponent({
  name: "PlcStateCreate",

  components: {
    PlcStateForm
  },

  props: {
    plcId: {
      type: Number,
      required: true,
    }
  },

  setup(props, { emit }) {
    const http = useIotHttp();
    const state = ref(new PlcState());

    async function handleSubmit() {
      const result = await http.post("/plcs/states/create", state.value);
      const { id } = result;

      emit("refresh", id);
    }

    onMounted(() => state.value.plc_id = props.plcId);

    return {
      state,
      handleSubmit,
    };
  }
});
</script>
