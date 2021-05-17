<template>
  <div>
    <Input
      readonly
      :value="endpoints.values[value]?.name ?? ''"
      @click="isShow = true"
    />

    <div
      v-if="isShow"
      class="modal is-active"
    >
      <div class="modal-background" />

      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">
            服务选择
          </p>
          <button
            @click="isShow = false"
            class="delete"
          />
        </header>

        <section class="modal-card-body">
          <table class="table is-clickable is-hoverable is-bordered is-fullwidth">
            <thead>
              <th style="width: 1px">#</th>
              <th>名称</th>
              <th>URL</th>
              <th style="width: 3rem"></th>
            </thead>
            <tbody>
              <DataMap
                :dataMap="endpoints"
                v-slot="{ value, index }"
              >
                <tr @click="endpoint = value">
                  <td>{{index + 1}}</td>
                  <td>{{value.name}}</td>
                  <td>{{value.url}}</td>
                  <td
                    class="is-centered is-vcentered"
                    style="padding: 0"
                  >
                    <span
                      v-if="endpoint?.id === value.id"
                      class="icon has-text-success"
                    >
                      <i class="icon-api-gateway icon-api-gateway-tick" />
                    </span>
                  </td>
                </tr>
              </DataMap>
            </tbody>
          </table>
        </section>

        <footer class="modal-card-foot">
          <a
            class="button is-success"
            @click="handleSelect"
          >
            选择
          </a>
        </footer>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import { DataMap } from "@midos/seed-work";
import { useService } from "@midos/vue-ui";
import { EndpointRepository } from "../../domain/repositories";
import { Endpoint } from "../../domain/entities";

export default defineComponent({
  props: {
    value: {
      type: Number,
      required: true
    }
  },

  setup(props, { emit }) {
    const repository = useService(EndpointRepository);
    const endpoints = ref(new DataMap<Endpoint>());
    const endpoint = ref<any>(null);
    const isShow = ref(false);

    async function getEndpoints() {
      endpoints.value = await repository.query({ query: "" });

      if (endpoint.value != null) {
        return;
      }

      if (props.value !== 0) {
        endpoint.value = endpoints.value.values[props.value];
      }
    }

    function handleSelect() {
      emit("update:value", endpoint.value.id);
      isShow.value = false;
    }

    onMounted(getEndpoints);

    return {
      endpoints,
      endpoint,
      isShow,
      handleSelect
    };
  }
});
</script>
