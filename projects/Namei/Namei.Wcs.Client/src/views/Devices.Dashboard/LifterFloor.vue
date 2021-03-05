<template>
  <div
    class="is-flex"
    style="height: 110px"
  >
    <LifterPlatform
      :code="floorState.palletCodeB"
      text="B 段"
    />

    <LifterPlatform
      :code="floorState.palletCodeA"
      text="A 段"
    />

    <div
      class="is-flex is-flex-column"
    >
      <div class="is-flex-auto"></div>

      <div
        class="is-flex is-flex-column"
        style="margin-left: 0.5rem"
      >
        <div class="is-flex-auto"></div>

        <div class="is-flex is-centered">
          <a
            class="tag"
            v-class:is-success="isExported"
            style="margin-right: 0.125rem"
            @click="handleTaken"
          >
            取货
          </a>
          <a
            class="tag"
            v-class:is-success="isImportAllowed"
            @click="handleImported"
          >
            放货
          </a>
        </div>

        <div class="is-flex is-centered is-vcentered" style="height: 30px">
          <span
            v-if="!isAgcRequesting"
            class="tag is-light"
          >
            AGC 请求通过
          </span>
          <a
            v-else
            @click="handleOpenDoor"
            class="tag is-info"
          >
            AGC 请求通过
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, PropType } from "vue";
import { useConfirm } from "@midos/vue-ui";
import { useWcsHttp } from "../../services/wcs-http";
import LifterPlatform from "./LifterPlatform.vue";
import { Door } from "./_interfaces";

export default defineComponent({
  name: "LifterFloor",

  components: {
    LifterPlatform
  },

  props: {
    lifterId: {
      type: String,
      required: true
    },
    floor: {
      type: Number,
      required: true
    },
    floorState: {
      type: Object,
      required: true
    },
    door: {
      type: Object as PropType<Door>,
      required: true
    }
  },

  setup(props) {
    const http = useWcsHttp();
    const confirm = useConfirm();

    const isAgcRequesting = computed(() => props.door.requestingTasks.length);
    const isImportAllowed = computed(() => props.floorState.isImportAllowed);
    const isExported = computed(() => props.floorState.isExported);

    function handleOpenDoor() {
      confirm.open({
        title: "确认",
        content: "确认后将直接允许 AGC 通过",
        handler: () => http.post("/doors/control", {
          command: "open",
          door_id: props.door.id,
        }),
      });
    }

    function handleImported() {
      confirm.open({
        title: "放货",
        content: "发送放货完成信号",
        handler: () => http.post("/lifters/imported", {
          lifterId: props.lifterId,
          floor: props.floor.toString()
        })
      });
    }

    function handleTaken() {
      confirm.open({
        title: "取货",
        content: "发送取货完成信号",
        handler: () => http.post("/lifters/taken", {
          lifterId: props.lifterId,
          floor: props.floor.toString()
        })
      });
    }

    return {
      isAgcRequesting,
      isImportAllowed,
      isExported,
      handleOpenDoor,
      handleImported,
      handleTaken,
    };
  },
});
</script>
