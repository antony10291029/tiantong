<template>
  <div class="column">
    <div class="box">
      <p class="title is-5">
        服务详情
      </p>

      <TheForm :entity="data">
        <template #footer>
          <AsyncButton
            :handler="handleUpdate"
            class="button is-info"
            :disabled="!isChanged"
          >
            更新
          </AsyncButton>

          <span class="is-flex-auto"></span>

          <a
            @click="handleDelete"
            class="button is-danger is-light"
          >
            删除
          </a>
        </template>
      </TheForm>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";
import { useRouter } from "vue-router";
import { useConfirm } from "@midos/vue-ui";
import { useEndpointRepository } from "../../domain/repositories";
import { useCopy } from "../../hooks/use-copy";
import TheForm from "../Endpoints.Create/Form.vue";

export default defineComponent({
  components: {
    TheForm
  },

  props: {
    id: {
      type: Number,
      required: true
    },

    endpoints: {
      type: Object,
      required: true
    }
  },

  setup(props, { emit }) {
    const router = useRouter();
    const confirm = useConfirm();
    const repository = useEndpointRepository();
    const entity = computed(() => props.endpoints[props.id]);
    const { data, isChanged } = useCopy(entity);

    async function handleUpdate() {
      await repository.update(data.value);
      emit("refresh");
    }

    async function handleDelete() {
      confirm.open({
        title: "提示",
        content: "删除后数据将无法恢复",
        handler: async () => {
          await repository.remove(data.value.id);
          emit("refresh");
          router.push({ name: "ApiGatewayEndpoints" });
        }
      });
    }

    return {
      data,
      isChanged,
      handleUpdate,
      handleDelete
    };
  }
});
</script>
