<template>
  <div>
    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        任务模板
      </label>

      <Input
        v-model:value="params.taskTyp"
        style="max-width: 400px"
      />
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px; align-self: start">
        参数
      </label>

      <div
        class="is-flex-auto"
        style="max-width: 400px"
      >
        <table class="table is-bordered is-fullwidth">
          <thead>
            <th style="width: 120px">类型</th>
            <th>位置编号</th>
            <th style="width: 1px">
              <a @click="addPosition">
                <span class="icon">
                  <i class="icon-namei-wcs icon-namei-wcs-plus"></i>
                </span>
              </a>
            </th>
          </thead>
          <tbody v-if="params.positionCodePath.length">
            <tr
              v-for="(position, index) in params.positionCodePath"
              :key="index"
            >
              <EditableCell v-model:value="position.type" />
              <EditableCell v-model:value="position.positionCode" />
              <td>
                <a
                  class="icon has-text-danger"
                  @click="removePosition(index)"
                >
                  <i class="icon-namei-wcs icon-namei-wcs-close"></i>
                </a>
              </td>
            </tr>
          </tbody>
          <tbody v-else>
            <tr>
              <td class="is-centered" colspan="3">
                无
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        货架编号
      </label>

      <Input
        v-model:value="params.podCode"
        style="max-width: 400px"
      />
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        优先级
      </label>

      <Input
        v-model:value="params.priority"
        style="max-width: 400px"
      />
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        任务编号
      </label>

      <Input
        v-model:value="params.taskCode"
        style="max-width: 400px"
      />
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">
        AGV 编号
      </label>

      <Input
        v-model:value="params.agvCode"
        style="max-width: 400px"
      />
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 120px">

      </label>

      <AsyncButton
        :handler="handleSubmit"
        class="button is-info is-small"
      >
        提交
      </AsyncButton>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { DateTime } from "@midos/vue-ui";
import { useWcsHttp } from "../../services/wcs-http";
import EditableCell from "../../components/EditableCell.vue";

export default defineComponent({
  name: "Rcs.Tasks.Create",

  components: {
    EditableCell
  },

  setup(_, { emit }) {
    const http = useWcsHttp();
    const params = ref({
      reqCode: "",
      taskTyp: "",
      podCode: "",
      priority: "",
      taskCode: "",
      agvCode: "",
      positionCodePath: [] as any[],
    });

    function addPosition() {
      params.value.positionCodePath.push({
        positionCode: "",
        type: "00"
      });
    }

    function removePosition(index: number) {
      params.value.positionCodePath.splice(index, 1);
    }

    async function handleSubmit() {
      const result = await http.post("/rcs/tasks/create", params.value);

      emit("log", `[${DateTime.now}]: ${JSON.stringify(result, null, 4)}`);
    }

    return {
      params,
      addPosition,
      removePosition,
      handleSubmit
    };
  }
});
</script>
