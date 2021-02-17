<template>
  <div>
    <HttpPusherForm :pusher="pusher">
      <template #footer>
        <div class="is-flex" style="padding: 0.75rem 0">
          <div style="width: 100px"></div>
          <AsyncButton
            :handler="handleSubmit"
            class="button is-info is-small"
            style="margin-right: 0.5rem"
          >
            提交
          </AsyncButton>
          <a
            @click="$emit('close')"
            class="button is-info is-light is-small"
          >
            关闭
          </a>
        </div>

        <div class="has-border-bottom" style="margin: 1.25rem -1.25rem"></div>
      </template>
    </HttpPusherForm>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import HttpPusherForm from "@/components/HttpPusherForm.vue";
import { HttpPusher } from "@/entities";
import { useIotHttp } from "@/services/iot-http-client";

export default defineComponent({
  name: "PlcStateHttpPusherCreate",

  components: {
    HttpPusherForm
  },

  props: {
    stateId: {
      type: Number,
      required: true
    }
  },

  setup(props, { emit }) {
    const http = useIotHttp();
    const pusher = ref(new HttpPusher());

    async function handleSubmit() {
      await http.post("/plcs/states/http-pushers/create", {
        state_id: props.stateId,
        pusher: pusher.value
      });

      emit("created");
    }

    return {
      pusher,
      handleSubmit,
    };
  }
});
</script>
