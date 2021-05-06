<template>
  <div>
    <div class="field">
      <p class="label">Key</p>

      <div class="control">
        <Input v-model:value="params.key" />
      </div>
    </div>

    <div class="field">
      <p class="label">名称</p>

      <div class="control">
        <Input v-model:value="params.name" />
      </div>
    </div>

    <div class="field">
      <p class="label">Webhook</p>

      <div class="control">
        <Input v-model:value="params.webhook" />
      </div>
    </div>

    <div class="field">
      <p class="label">任务方法</p>

      <div class="control">
        <table class="table is-fullwidth is-bordered is-hoverable">
          <tbody>
            <tr
              v-for="method in methods" :key="method.key"
              @click="params.method = method.key"
              style="cursor: pointer"
            >
              <td style="width: 1px">
                <Radio :value="params.method === method.key" />
              </td>
              <td>{{method.text}}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div class="field">
      <p class="label">是否启用</p>

      <div class="control">
        <table class="table is-fullwidth is-bordered is-hoverable">
          <tbody>
            <tr
              v-for="value in [true, false]"
              :key="value.toString()"
              style="cursor: pointer"
              @click="params.isEnabled = value"
            >
              <td style="width: 1px">
                <Radio :value="value ? params.isEnabled : !params.isEnabled"/>
              </td>
              <td v-if="value">启用</td>
              <td v-else>禁用</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref } from "vue";

const methods = [
  { key: "wcs.move", text: "移动" },
  { key: "wcs.lift", text: "举起" },
  { key: "wcs.put", text: "放下" },
  { key: "wcs.carry", text: "搬运" },
  { key: "wcs.move.lock", text: "移动（锁定）" },
  { key: "wcs.put.lock", text: "放下（锁定）" },
  { key: "wcs.carry.lock", text: "搬运（锁定）" },
];

interface Params {
  key: string;
  name: string;
  method: string;
  webhook: string;
  isEnabled: boolean;
}

export default defineComponent({
  props: {
    params: {
      type: Object as PropType<Params>,
      required: true
    }
  },

  setup(props) {
    const isShow = ref(false);

    if (props.params.method === "") {
      props.params.method = methods[0].key;
    }

    return {
      isShow,
      methods,
    };
  }
});
</script>
