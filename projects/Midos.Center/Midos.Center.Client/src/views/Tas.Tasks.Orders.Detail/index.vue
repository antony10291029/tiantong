<template>
  <div class="modal is-active">
    <div
      class="modal-background"
      @click="close"
    />

    <div class="modal-card" style="width: 500px">
      <div class="modal-card-head">
        <p class="modal-card-title">
          任务详情
        </p>
      </div>

      <div class="modal-card-body">
        <div class="field">
          <label class="label">
            Key
          </label>
          <div class="control">
            <Input :value="taskType.key" readonly />
          </div>
        </div>

        <div class="field">
          <label class="label">
            任务名
          </label>
          <div class="control">
            <Input :value="taskType.name" readonly />
          </div>
        </div>

        <div class="field">
          <label class="label">
            数据
          </label>

          <div class="control">
            <Textarea :value="orderData" readonly />
          </div>
        </div>
      </div>

      <div class="modal-card-foot">
        <a
          @click="close"
          class="button is-info"
        >
          确认
        </a>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from "vue";
import { useRouter } from "vue-router";

export default defineComponent({
  name: "Task.Tasks.Orders.Detail",

  props: {
    taskType: {
      type: Object,
      required: true
    },

    taskOrder: {
      type: Object,
      required: true
    }
  },

  setup(props) {
    const router = useRouter();
    const orderData = computed(() => JSON.stringify(
      props.taskOrder.data, null, 4
    ));

    function close() {
      router.push({
        name: "TasOrders",
        params: { typeId: props.taskType.id }
      });
    }

    return {
      orderData,
      close
    };
  }
});
</script>
