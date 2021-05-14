<template>
  <div>
    <Input
      readonly
      :value="endpoints.entities[value]?.name ?? ''"
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
              <DataMapIterator
                :dataMap="endpoints"
                v-slot="{ entity, index }"
              >
                <tr @click="endpoint = entity">
                  <td>{{index + 1}}</td>
                  <td>{{entity.name}}</td>
                  <td>{{entity.url}}</td>
                  <td
                    class="is-centered is-vcentered"
                    style="padding: 0"
                  >
                    <span
                      v-if="endpoint?.id === entity.id"
                      class="icon has-text-success"
                    >
                      <i class="icon-api-gateway icon-api-gateway-tick" />
                    </span>
                  </td>
                </tr>
              </DataMapIterator>
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
import { DataMap } from "@midos/core";
import { defineComponent, onMounted, ref } from "vue";
import { UseApiGatewayHttp } from "../../services/api-gateway-http";

export default defineComponent({
  props: {
    value: {
      type: Number,
      required: true
    }
  },

  setup(props, { emit }) {
    const endpoints = ref(new DataMap<any>());
    const http = UseApiGatewayHttp();
    const endpoint = ref<any>(null);
    const isShow = ref(false);

    async function getEndpoints() {
      endpoints.value = await http.getDataMap("$endpoints/all");

      if (endpoint.value != null) {
        return;
      }

      if (props.value !== 0) {
        endpoint.value = endpoints.value.entities[props.value];
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
