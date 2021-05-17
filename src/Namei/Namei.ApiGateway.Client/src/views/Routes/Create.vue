<template>
  <div>
    <a @click="isShow = true">添加</a>

    <teleport to="body">

      <div
        v-if="isShow"
        class="modal is-active"
      >
        <div class="modal-background" />

        <div class="modal-card">
          <div class="modal-card-head">
            <p class="modal-card-title">
              添加路由
            </p>
            <button
              @click="isShow = false"
              class="delete"
            />
          </div>

          <div class="modal-card-body">
            <TheForm :entity="params" />
          </div>

          <div class="modal-card-foot">
            <AsyncButton
              class="button is-success"
              :handler="handleSubmit"
            >
              提交
            </AsyncButton>
          </div>
        </div>
      </div>
    </teleport>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useRouteRepository } from "../../domain/repositories";
import { Route } from "../../domain/entities";
import TheForm from "./Form.vue";

export default defineComponent({
  components: {
    TheForm
  },

  setup(props, { emit }) {
    const repository = useRouteRepository();
    const isShow = ref(false);
    const params = ref(new Route());

    async function handleSubmit() {
      await repository.add(params.value);
      isShow.value = false;
      emit("refresh");
    }

    return {
      isShow,
      params,
      handleSubmit
    };
  }
});
</script>
