<template>
  <section v-if="plc">
    <div class="box is-paddingless" style="margin: 1.25rem;">
      <PlcForm
        :plc="plc"
        :handler="getPlc"
      >
        <template #header>
          <p class="title is-4">PLC 设置</p>

          <hr>
        </template>

        <template #footer>
          <hr>

          <div
            class="is-flex"
            style="padding: 0.75rem 0"
          >
            <div style="width: 100px"></div>

            <div class="field" style="width: 480px">
              <div class="control is-flex">
                <AsyncButton
                  :handler="handleSave"
                  :disabled="!isChanged"
                  class="button is-info is-small"
                  style="margin-right: 0.5rem"
                >
                  保存
                </AsyncButton>

                <AsyncButton
                  :handler="handleTest"
                  :disabled="isChanged"
                  class="button is-success is-small"
                  style="margin-right: 0.5rem"
                >
                  连接测试
                </AsyncButton>

                <span class="is-flex-auto"></span>

                <AsyncButton
                  :handler="handleDelete"
                  class="button is-danger is-light is-small"
                >
                  删除
                </AsyncButton>
              </div>
            </div>
          </div>
        </template>
      </PlcForm>
    </div>
  </section>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from "vue";
import PlcForm from "@/components/PlcForm.vue";
import cloneDeep from "lodash/cloneDeep";
import isEqual from "lodash/isEqual";
import { Plc } from "@/entities";
import { useIotHttp } from "@/services/iot-http-client";
import { useConfirm } from "@midos/vue-ui";
import { useRouter } from "vue-router";

export default defineComponent({
  name: "PlcConfig",

  components: {
    PlcForm
  },

  props: {
    plcId: {
      type: Number,
      required: true
    }
  },

  setup(props, { emit }) {
    const confirm = useConfirm();
    const http = useIotHttp();
    const router = useRouter();
    const plc = ref(new Plc());
    const sourceData = ref(new Plc());
    const isChanged = computed(() => !isEqual(plc.value, sourceData.value));

    async function getPlc () {
      const result = await http.post("/plcs/find", {
        plc_id: props.plcId
      });

      sourceData.value = result;
      plc.value = cloneDeep(sourceData.value);
    }

    async function handleSave () {
      await http.post("/plcs/update", plc.value);
      await getPlc();

      emit("refresh");
    }

    async function handleTest () {
      await http.post("/plc-workers/test", {
        plc_id: props.plcId
      });
    }

    function handleDelete () {
      confirm.open({
        title: "提示",
        content: "删除后设备将无法恢复",
        handler: async () => {
          await http.post("/plcs/delete", {
            plc_id: props.plcId
          });
          emit("refresh");
          router.push({ name: "IotPlcs" });
        }
      });
    }

    return {
      plc,
      sourceData,
      isChanged,
      getPlc,
      handleSave,
      handleTest,
      handleDelete,
    };
  }
});
</script>
