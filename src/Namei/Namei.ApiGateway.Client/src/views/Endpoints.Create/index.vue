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
import { useService } from "@midos/vue-ui";
import { defineComponent, ref } from "vue";
import { Endpoint } from "../../domain/entities";
import { EndpointRepository } from "../../domain/repositories";
import TheForm from "./Form.vue";

export default defineComponent({
  components: {
    TheForm
  },

  setup(props, { emit }) {
    const repository = useService(EndpointRepository);
    const entity = ref(new Endpoint());

    async function handleSubmit() {
      await repository.add(entity.value);
      emit("refresh");
    }

    return {
      entity,
      handleSubmit
    };
  }
});
</script>
