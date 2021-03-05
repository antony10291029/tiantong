<template>
  <div>
    <div class="box s" style="margin: 1.25rem">
      <p class="title is-5">
        提升机任务 - 创建
      </p>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          设备
        </label>

        <div
          class="field has-addons"
          style="width: 320px"
        >
          <div
            v-for="lifter in lifters"
            :key="lifter.id"
            class="control"
          >
            <a
              v-class:is-focused="params.lifterId == lifter.id"
              @click="params.lifterId = lifter.id"
              class="button is-small"
              style="width: 100px"
            >
              {{`${lifter.id} # ${lifter.text}`}}
            </a>
          </div>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          起点楼层
        </label>

        <div
          class="field has-addons"
          style="width: 320px"
        >
          <div
            v-for="floor in ['1', '2', '3', '4']"
            :key="floor"
            class="control"
          >
            <a
              v-class:is-focused="params.floor === floor"
              @click="params.floor = floor"
              class="button is-small"
              style="width: 100px"
            >
              {{`${floor} 楼`}}
            </a>
          </div>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          终点楼层
        </label>

        <div
          class="field has-addons"
          style="width: 320px"
        >
          <div
            v-for="floor in ['1', '2', '3', '4']"
            :key="floor"
            class="control"
          >
            <a
              v-class:is-focused="params.destination === floor"
              @click="params.destination = floor"
              class="button is-small"
              style="width: 100px"
            >
              {{`${floor} 楼`}}
            </a>
          </div>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          托盘条码
        </label>

        <Input
          v-model:value="params.barcode"
          style="max-width: 400px"
        />
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 0.75rem 0"
      >
        <label class="label" style="width: 120px">
          任务代码
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
        <div style="width: 120px">

        </div>

        <AsyncButton
          :handler="handleSubmit"
          class="button is-info is-small"
          style="width: 120px"
        >
          提交
        </AsyncButton>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useWcsHttp } from "../../services/wcs-http";

const lifters = [
  { text: "货梯", id: "1" },
  { text: "提升机", id: "2" },
  { text: "提升机", id: "3" }
];

export default defineComponent({
  name: "Lifter.Tasks",

  setup() {
    const http = useWcsHttp();
    const params = ref({
      lifterId: "1",
      floor: "1",
      destination: "1",
      barcode: "000000",
      taskCode: "000000"
    });

    async function handleSubmit() {
      await http.post("/lifter-tasks/create", {
        method: "deliver",
        operator: "wcs",
        liftCode: params.value.lifterId,
        floor: params.value.floor,
        destination: params.value.destination,
        barcode: params.value.barcode,
        taskCode: params.value.taskCode
      });
    }

    return {
      params,
      lifters,
      handleSubmit,
    };
  }
});
</script>
