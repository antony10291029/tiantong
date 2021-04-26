<template>
  <div>
    <div class="box">
      <p class="title is-5">
        添加应用
      </p>

      <hr>

      <AppForm type="create" :app="app"/>

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
import AppForm from "../Midos.Apps.App/Form.vue";

export default defineComponent({
  name: "Create",

  components: {
    AppForm
  },

  setup(props, { emit }) {
    const http = useMidosCenterHttp();
    const router = useRouter();

    const app = ref({
      key: "",
      name: "",
      url: ""
    });

    async function handleCreate() {
      const result = await http.post("/midos/apps/create", app.value);
      emit("refresh");
      router.push({
        name: "MidosCenterAppsApp",
        params: { id: result.id }
      });
    }

    return {
      app,
      handleCreate
    };
  }
});
</script>
