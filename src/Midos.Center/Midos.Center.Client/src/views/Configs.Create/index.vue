<template>
  <div>
    <div class="box">
      <p class="title is-5">
        添加配置
      </p>

      <hr>

      <ConfigForm type="create" :config="config"/>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <p class="label" style="width: 80px"></p>

        <p class="is-flex-auto">
          <AsyncButton
            class="button is-info is-small"
            @click="handleCreate"
          >
            创建
          </AsyncButton>
        </p>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useRouter } from "vue-router";
import { useMidosCenterHttp } from "../../services/midos-center-http";
import ConfigForm from "../Configs.Config/Form.vue";

export default defineComponent({
  name: "Create",

  components: {
    ConfigForm
  },

  setup(props, { emit }) {
    const http = useMidosCenterHttp();
    const router = useRouter();

    const config = ref({ key: "", value: "" });

    async function handleCreate() {
      await http.post("/midos/configs/create", [config.value]);
      emit("refresh");
      router.push({
        name: "MidosCenterConfigsConfig",
        params: { configKey: config.value.key }
      });
    }

    return {
      config,
      handleCreate
    };
  }
});
</script>
