<template>
  <td
    v-if="!isShow"
    class="is-clickable"
    @click="isShow = true"
  >
    <span v-if="text">{{text}}</span>
    <span
      v-else
      class="has-text-grey is-italic"
    >
      未选择
    </span>
  </td>
  <td v-else>
    <span v-if="text">{{text}}</span>
    <span
      v-else
      class="has-text-grey is-italic"
    >
      未选择
    </span>
    <div class="modal is-active">
      <div
        class="modal-background"
        @click="isShow = false"
      ></div>
      <div class="modal-card">
        <div class="modal-card-head">
          <p class="modal-card-title">
            选择子任务
          </p>
        </div>
        <div class="modal-card-body">
          <table class="table is-hoverable is-bordered is-fullwidth">
            <thead>
              <th>Key</th>
              <th>名称</th>
              <th style="width: 2.5rem">
              </th>
            </thead>
            <tbody>
              <tr
                v-for="type in types" :key="type.id"
                @click="currentType = type"
              >
                <td>{{type.key}}</td>
                <td>{{type.name}}</td>
                <td
                  class="is-centered is-vcentered"
                  style="padding: 0"
                >
                  <span
                    v-if="currentType.id === type.id"
                    class="icon has-text-success"
                  >
                    <i class="icon-midos-center icon-midos-center-tick" />
                  </span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="modal-card-foot">
          <a
            @click="handleSubmit"
            class="button is-info"
          >
            选择
          </a>
        </div>
      </div>
    </div>
  </td>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "vue";

export default defineComponent({
  name: "SubtypeSelector",

  props: {
    value: {
      type: Number,
      required: true
    },
    typeId: {
      type: Number,
      required: true
    },
    taskTypes: {
      type: Object,
      required: true
    }
  },

  setup(props, { emit }) {
    const isShow = ref(false);
    const currentType = ref<any>(props.value);
    const text = computed(() => (
      props.value === 0 ? undefined : (props.taskTypes.entities[props.value] as any).name)
    );
    const types = computed(() =>
      props.taskTypes.result
        .map((id: number) => props.taskTypes.entities[id])
        .filter((type: any) => type.id !== props.typeId)
    );

    function handleSubmit() {
      isShow.value = false;
      emit("update:value", currentType.value.id);
    }

    return {
      isShow,
      text,
      types,
      currentType,
      handleSubmit
    };
  }
});
</script>
