<template>
  <div
    class="is-flex"
    style="height: 100px"
  >
    <LifterPlatform
      :code="floorState.palletCodeB"
      text="B 段"
    />

    <LifterPlatform
      :code="floorState.palletCodeA"
      text="A 段"
      @barcode-click="handleBarcodeClick"
    />

    <div class="is-flex is-flex-column">
      <div class="is-flex-auto"></div>

      <div
        class="is-flex is-flex-column"
        style="margin-left: 0.25rem"
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
            style="width: 100%"
          >
            AGC 请求中
          </span>
          <a
            v-else
            @click="handleOpenDoor"
            class="tag is-info"
            style="width: 100%"
          >
            AGC 请求中
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
import type { Door } from "./_interfaces";

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

    function handleExported() {
      confirm.open({
        title: "请求取货",
        content: "发送请求取货指令",
        handler: () => http.post("/lifters/exported", {
          lifterId: props.lifterId,
          floor: props.floor.toString()
        })
      });
    }

    function handleScanned() {
      confirm.open({
        title: "任务查询",
        content: "重新查询电梯任务并写入电梯中",
        handler: () => http.post("/lifters/scanned", {
          lifterId: props.lifterId,
          floor: props.floor.toString()
        })
      });
    }

    function handleBarcodeClick() {
      if (props.floorState.destination === props.floor.toString()) {
        handleExported();
      } else {
        handleScanned();
      }
    }

    return {
      isAgcRequesting,
      isImportAllowed,
      isExported,
      handleOpenDoor,
      handleImported,
      handleTaken,
      handleBarcodeClick
    };
  },
});
</script>
