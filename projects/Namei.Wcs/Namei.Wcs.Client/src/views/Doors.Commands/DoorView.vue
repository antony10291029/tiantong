<template>
  <div class="box">
    <p class="title is-size-6">
      指令面板
    </p>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 1.25rem 0"
    >
      <p class="label" style="width: 120px">
        控制指令
      </p>

      <div class="buttons">
        <a
          @click="publishMessage('requested.open')"
          class="button is-info"
        >
          请求开门
        </a>

        <a
          @click="publishMessage('requested.close')"
          class="button is-info"
        >
          请求关门
        </a>

        <a
          @click="publishMessage('opened')"
          class="button is-info"
        >
          开门完毕
        </a>

        <a
          @click="publishMessage('closed')"
          class="button is-info"
        >
          关门完毕
        </a>
      </div>
    </div>

    <hr>

  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useWcsHttp } from "@/services/wcs-http";

export default defineComponent({
  name: "DoorCommandsDashboard",

  props: {
    doorId: {
      type: Number,
      required: true
    }
  },

  setup(props) {
    const http = useWcsHttp();

    async function publishMessage(message: string) {
      await http.post("/test/doors/publish-message", {
        door_id: props.doorId,
        message,
      });
    }

    return {
      publishMessage
    };
  }
});
</script>
