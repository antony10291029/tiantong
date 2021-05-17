<template>
  <td>
    <a @click="isShow = true">详情</a>

    <teleport to="body">
      <div
        v-if="isShow"
        class="modal is-active"
      >
        <div class="modal-background" />

        <div class="modal-card">
          <header class="modal-card-head">
            <p class="modal-card-title">
              路由详情
            </p>
            <button
              @click="isShow = false"
              class="delete"
            />
          </header>

          <section class="modal-card-body">
            <TheForm :entity="data" />
          </section>

          <footer class="modal-card-foot is-flex">
            <AsyncButton
              class="button is-success"
              :handler="handleUpdate"
              :disabled="!isChanged"
            >
              更新
            </AsyncButton>

            <span class="is-flex-auto"></span>

            <a
              class="button is-danger"
              @click="handleDelete"
            >
              删除
            </a>
          </footer>
        </div>
      </div>
    </teleport>
  </td>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, toRefs } from "vue";
import { useConfirm, useService } from "@midos/vue-ui";
import { useCopy } from "../../hooks/use-copy";
import { Route } from "../../domain/entities";
import { RouteRepository } from "../../domain/repositories";
import TheForm from "./Form.vue";

export default defineComponent({
  components: {
    TheForm
  },

  props: {
    route: {
      type: Object as PropType<Route>,
      required: true
    }
  },

  setup(props, { emit }) {
    const repository = useService(RouteRepository);
    const confirm = useConfirm();
    const isShow = ref(false);
    const { route } = toRefs(props);
    const { data, isChanged } = useCopy<any>(route);

    async function handleUpdate() {
      await repository.update(data.value);
      isShow.value = false;
      emit("refresh");
    }

    function handleDelete() {
      confirm.open({
        title: "提示",
        content: "删除后将无法恢复",
        handler: async () => {
          await repository.remove(data.value.id);
          isShow.value = false;
          emit("refresh");
        }
      });
    }

    return {
      data,
      isShow,
      isChanged,
      handleDelete,
      handleUpdate,
    };
  }
});
</script>
