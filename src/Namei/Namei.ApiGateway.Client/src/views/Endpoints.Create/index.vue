<template>
  <div class="column">
    <div class="box">
      <p class="title is-5">
        添加服务
      </p>

      <TheForm :entity="entity">
        <template #footer>
          <AsyncButton
            class="button is-info"
            :handler="handleSubmit"
          >
            提交
          </AsyncButton>
        </template>
      </TheForm>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { UseApiGatewayHttp } from "../../services/api-gateway-http";
import TheForm from "./Form.vue";

export default defineComponent({
  components: {
    TheForm
  },

  setup(props, { emit }) {
    const http = UseApiGatewayHttp();
    const entity = ref({
      name: "",
      url: "",
    });

    async function handleSubmit() {
      await http.post("/$endpoints/add", entity.value);
      emit("refresh");
    }

    return {
      entity,
      handleSubmit
    };
  }
});
</script>
