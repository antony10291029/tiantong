<template>
  <div
    class="
      fixed p-4 top-0 right-0 z-0
      flex flex-col items-end bg-transparent
      w-full sm:w-auto
    "
  >
    <Notify
      v-for="notify in data"
      :key="notify.index"
      :type="notify.type"
      :content="notify.content"
      :duration="notify.duration"
      @close="handleClose(notify)"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { NotifyParams, OpenNotifyParams } from "./interface";
import Notify from "./Notify.vue";

interface NotifyDTO {
  index: number
  type: string
  content: string
  duration: number
}

export default defineComponent({
  components: {
    Notify
  },

  setup() {
    let count = 0;
    const data = ref<NotifyDTO[]>([]);
    const handleClose = (notify: NotifyDTO) => {
      data.value.splice(data.value.indexOf(notify), 1);
    };
    const open = ({ type, content, duration = 3000 }: OpenNotifyParams) => {
      data.value.push({
        index: count++,
        type,
        content,
        duration
      });
    };
    const makeType = (type: string) => (params: NotifyParams) => open({ ...params, type });

    return {
      data,
      handleClose,
      open,
      info: makeType("info"),
      link: makeType("link"),
      success: makeType("success"),
      warning: makeType("warning"),
      danger: makeType("danger"),
    };
  }
});
</script>
