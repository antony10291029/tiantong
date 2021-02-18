<template>
  <div
    class="panel-block"
    style="height: 60px"
  >
    <div class="label"># {{door.id}}</div>

    <div class="is-flex-auto"></div>

    <span
      v-if="door.isError"
      class="tag is-danger is-light"
      style="margin-right: 0.5rem"
    >
      <span>异常</span>
    </span>

    <a
      v-if="door.isForceOpened"
      class="tag is-light is-danger"
      style="margin-right: 0.5rem"
      @click="handleSetForceOpened(false)"
    >
      <span class="icon">
        <i class="iconfont namei-wcs-press"></i>
      </span>
    </a>

    <a
      v-else
      class="tag is-light"
      style="margin-right: 0.5rem"
      @click="handleSetForceOpened(true)"
    >
      <span class="icon">
        <i class="iconfont namei-wcs-press"></i>
      </span>
    </a>

    <span
      class="tag is-light"
      style="margin-right: 0.5rem"
      v-class:is-info="isRequesting"
    >
      <span v-if="isRequesting">请求中</span>
      <span v-else>无请求</span>
    </span>

    <a
      v-if="isOpened"
      @click="handleClose"
      class="tag is-light is-info"
    >
      关门
    </a>

    <a
      v-else
      @click="handleOpen"
      class="tag is-light"
      style="margin-right: 0.5rem"
    >
      开门
    </a>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, PropType } from "vue";
import { useConfirm } from "@midos/vue-ui";
import { useWcsHttp } from "@/services/wcs-http";

export default defineComponent({
  name: "Door",

  props: {
    door: {
      type: Object as PropType<any>,
      required: true
    }
  },

  setup(props) {
    const http = useWcsHttp();
    const confirm = useConfirm();

    const isRequesting = computed(() => (props.door as any)?.requestingTasks.length);
    const isOpened = computed(() => (props.door as any).isOpened);

    async function handleOpen() {
      confirm.open({
        title: "开门",
        content: "手动执行关门指令",
        handler: async () => await http.post("/doors/control", {
          door_id: (props.door as any)?.id,
          command: "open"
        })
      });
    }

    async function handleClose() {
      confirm.open({
        title: "关门",
        content: "手动执行关门指令",
        handler: async () => await http.post("/doors/control", {
          door_id: (props.door as any).door?.id,
          command: "close"
        })
      });
    }

    async function handleSetForceOpened(value: boolean) {
      confirm.open({
        title: value ? "设置常开" : "关闭常开",
        content: value
          ? "设置常开后，将默认放行 AGC 通过"
          : "关闭常开后，AGC 根据正常逻辑通行",
        handler: async () => await http.post("/doors/force-opened/set", {
          doorId: (props.door as any)?.id,
          value
        })
      });
    }

    return {
      confirm,
      isRequesting,
      isOpened,
      handleOpen,
      handleClose,
      handleSetForceOpened
    };
  }
});
</script>
