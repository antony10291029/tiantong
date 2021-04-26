<template>
  <div v-if="config">
    <div class="box">
      <p class="title is-5">
        详细信息
      </p>

      <hr>

      <ConfigForm :config="data" />

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <p class="label" style="width: 80px"></p>

        <p class="is-flex-auto">
          <AsyncButton
            class="button is-info is-small"
            :disabled="!isChanged"
            :handler="handleUpdate"
          >
            更新
          </AsyncButton>
          <AsyncButton
            style="margin-left: 0.5rem"
            class="button is-danger is-light is-small"
            :handler="handleDelete"
          >
            删除
          </AsyncButton>
        </p>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { useConfirm } from "@midos/vue-ui";
import { computed, defineComponent, PropType } from "vue";
import { useRouter } from "vue-router";
import { useCopy } from "../../hooks/use-copy";
import { useMidosCenterHttp } from "../../services/midos-center-http";
import ConfigForm from "./Form.vue";

export default defineComponent({
  name: "Config",

  components: {
    ConfigForm
  },

  props: {
    configKey: {
      type: String,
      required: true
    },

    configs: {
      type: Object as PropType<any[]>,
      required: true,
    }
  },

  setup(props, { emit }) {
    const router = useRouter();
    const confirm = useConfirm();
    const http = useMidosCenterHttp();

    const config = computed(() => props.configs
      .find(item => item.key === props.configKey)
    );
    const { data, isChanged } = useCopy(config);

    async function handleUpdate() {
      await http.post("/midos/configs/update", [data.value]);
      emit("refresh");
    }

    function handleDelete() {
      confirm.open({
        title: "删除配置",
        content: "删除后将无法恢复",
        handler: async () => {
          await http.post("/midos/configs/delete", { keys: [props.configKey] });
          emit("refresh");
          router.push({ name: "MidosCenterConfigs" });
        }
      });
    }

    return {
      data,
      config,
      isChanged,
      handleUpdate,
      handleDelete,
    };
  }
});
</script>
