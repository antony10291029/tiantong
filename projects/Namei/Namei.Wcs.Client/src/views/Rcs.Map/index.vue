<template>
  <div>
    <div class="box" style="margin: 1.25rem">
      <p class="title is-4">
        地图绑定
      </p>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          操作类型
        </label>

        <div
          class="field has-addons"
          style="width: 320px"
        >
          <div
            v-for="operation in operations"
            :key="operation.type"
            class="control"
          >
            <a
              v-class:is-focused="params.operation == operation.type"
              @click="params.operation = operation.type"
              class="button is-small"
              style="width: 120px"
            >
              {{operation.text}}
            </a>
          </div>
        </div>
      </div>

      <hr>

      <template  v-if="params.operation === 'unbind'">

        <div
          class="is-flex is-vcentered"
          style="padding: 0.75rem 0"
        >
          <label class="label" style="width: 120px">
            解绑方式
          </label>

          <div
            class="field has-addons"
            style="width: 320px; min-height: 40px"
          >
            <div
              v-for="method in unbindMethods"
              :key="method.type"
              class="control"
            >
              <a
                v-class:is-focused="params.method == method.type"
                @click="params.method = method.type"
                class="button is-small"
                style="width: 120px"
              >
                {{method.text}}
              </a>
            </div>
          </div>
        </div>
      </template>

      <template v-else-if="params.operation === 'bind'">
        <div
          class="is-flex is-vcentered"
          style="padding: 0.75rem 0"
        >
          <label class="label" style="width: 120px">
            托盘代码
          </label>

          <Input
            v-model:value="params.podCode"
            style="max-width: 400px"
          />

        </div>
      </template>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          位置代码
        </label>

        <Input
          v-model:value="params.locationCode"
          style="max-width: 400px"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <div style="width: 120px">

        </div>

        <AsyncButton
          :handler="handleSubmit"
          class="button is-info is-small"
        >
          提交
        </AsyncButton>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useConfirm, useNotify } from "@midos/vue-ui";
import { useRcsExtHttp } from "../../services/rcs-ext-http";

const operations = [
  { type: "unbind", text: "解绑" },
  { type: "bind", text: "绑定" },
];

const unbindMethods = [
  { type: "location", text: "位置解绑" },
  { type: "area", text: "区域解绑" },
];

export default defineComponent({
  name: "NameiWcsRcsMap",

  setup() {
    const http = useRcsExtHttp();
    const confirm = useConfirm();
    const notify = useNotify();

    const params = ref({
      operation: "unbind",
      method: "location",
      locationCode: "",
      podCode: "",
    });

    async function handleBind() {
      const result = await http.post("/public/rcs/bindPodAndBerth", {
        locationCode: params.value.locationCode,
        podCode: params.value.podCode
      });

      if (result.status === "1") {
        notify.success("绑定成功");
      } else {
        notify.danger(result.message);
      }
    }

    async function handleUnbind() {
      const result = await http.post("/public/rcs/unbindPodAndBerth", {
        method: params.value.method,
        locationCode: params.value.locationCode
      });

      if (result.status === "1") {
        notify.success("绑定成功");
      } else {
        notify.danger(result.message);
      }
    }

    async function handleSubmit() {
      if (params.value.operation === "bind") {
        await handleBind();
      } else if (params.value.operation === "unbind") {
        if (params.value.method === "location") {
          await handleUnbind();
        } else if (params.value.method === "area") {
          confirm.open({
            title: "区域解绑确认",
            content: "该操作将解绑输入位置所在的整个区域",
            width: "360px",
            handler: handleUnbind
          });
        }
      }
    }

    return {
      operations,
      unbindMethods,
      params,
      handleSubmit,
    };
  }
});
</script>
