<template>
  <section style="overflow: auto">
    <div class="box" style="margin: 1.25rem; padding: 0">
      <PlcForm :plc="plc">
        <template #header>
          <p class="title is-4">
            添加 PLC
          </p>

          <hr>
        </template>
        <template #footer>
          <hr>

          <div
            class="is-flex"
            style="padding: 0.75rem 0"
          >
            <div style="width: 100px"></div>
            <AsyncButton
              :handler="handleSubmit"
              class="button is-info is-small"
            >
              提交
            </AsyncButton>
          </div>
        </template>
      </PlcForm>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import PlcForm from "@/components/PlcForm.vue";
import { Plc } from "@/entities/Plc";
import { useIotHttp } from "@/services/iot-http-client";
import { useRouter } from "vue-router";

export default defineComponent({
  name: "PlcCreate",

  components: {
    PlcForm
  },

  setup(props, { emit }) {
    const plc = ref(new Plc());
    const http = useIotHttp();
    const router = useRouter();

    async function handleSubmit () {
      const result = await http.post("/plcs/create", plc.value);
      const { id } = result;

      emit("refresh");
      router.push({
        name: "IotPlcsPlc",
        params: { plcId: id }
      });
    }

    return {
      plc,
      handleSubmit
    };
  }
});
</script>
