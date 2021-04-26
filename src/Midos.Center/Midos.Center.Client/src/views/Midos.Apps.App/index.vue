<template>
  <div v-if="app">
    <div class="box">
      <p class="title is-5">
        详细信息
      </p>

      <hr>

      <AppForm :app="data" />

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
import AppForm from "./Form.vue";

export default defineComponent({
  name: "Config",

  components: {
    AppForm
  },

  props: {
    id: {
      type: Number,
      required: true
    },

    apps: {
      type: Object as PropType<any[]>,
      required: true,
    }
  },

  setup(props, { emit }) {
    const router = useRouter();
    const confirm = useConfirm();
    const http = useMidosCenterHttp();

    const app = computed(() => props.apps
      .find(item => item.id === props.id)
    );
    const { data, isChanged } = useCopy(app);

    async function handleUpdate() {
      await http.post("/midos/apps/update", data.value);
      emit("refresh");
    }

    function handleDelete() {
      confirm.open({
        title: "删除配置",
        content: "删除后将无法恢复",
        handler: async () => {
          await http.post("/midos/apps/delete", { id: app.value.id });
          emit("refresh");
          router.push({ name: "MidosCenterApps" });
        }
      });
    }

    return {
      app,
      data,
      isChanged,
      handleUpdate,
      handleDelete,
    };
  }
});
</script>
