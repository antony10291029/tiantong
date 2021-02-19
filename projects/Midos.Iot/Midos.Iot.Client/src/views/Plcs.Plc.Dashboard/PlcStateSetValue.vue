<template>
  <td v-if="!isShow" style="width: 200px;">
    <a @click="handleOpen">
      写入
    </a>
  </td>
  <td v-else style="padding: 0; width: 200px">
    <div
      class="background"
      @click="isShow = false"
    ></div>
    <div
      class="field has-addons"
      style="position: absolute; z-index: 99999"
    >
      <div class="control">
        <input
          ref="input"
          :value="value"
          @blur="setValue($event.target.value.trim())"
          @keypress.enter="setValue($event.target.value.trim()), handleSave()"
          type="text" class="input"
          style="height: 100%; border-radius: 0; width: 200px"
        >
      </div>
      <div class="control">
        <AsyncButton
          :handler="handleSave"
          class="button is-success"
        >
          写入
        </AsyncButton>
      </div>
    </div>
  </td>
</template>

<script lang="ts">
import { defineComponent, nextTick, ref } from "vue";
import { useIotHttp } from "../../services/iot-http-client";

export default defineComponent({
  name: "PlcStateSetValue",

  props: {
    plc: {
      type: Object,
      required: true
    },

    state: {
      type: Object,
      required: true
    },

    type: {
      type: String,
      required: true
    }
  },

  setup(props) {
    const http = useIotHttp();
    const isShow = ref(false);
    const value = ref<any>("");
    const input = ref<any>(null);

    const setValue = (val: any) => value.value = val;
    const handleOpen = () => {
      isShow.value = true;
      nextTick(() => {
        input.value.focus();
        value.value = "";
      });
    };
    const handleSave = async () => {
      await http.post("/plc-states/set", {
        plc: props.plc.name,
        state: props.state.name,
        value: value.value
      });

      isShow.value = false;
    };

    return {
      isShow,
      value,
      input,
      setValue,
      handleOpen,
      handleSave
    };
  }
});
</script>
