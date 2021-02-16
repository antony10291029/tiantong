<template>
  <AsyncLoader
    class="is-flex"
    :handler="getPlcs"
  >
    <aside
      class="menu is-unselectable"
      style="height: 100%; overflow-y: auto; min-width: 260px"
    >
      <ul class="menu-list">
        <li>
          <router-link
            :to="`${baseURL}/plcs`"
            active-class="none"
            exact-active-class="is-active"
          >
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <i class="iconfont icon-manage"></i>
            </span>
            <span>服务管理</span>
          </router-link>
        </li>
        <li v-for="id in plcs.result" :key="id">
          <router-link :to="`${baseURL}/plcs/${id}`">
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <span class="iconfont icon-device"></span>
            </span>
            <span>{{plcs.data[id].name}}</span>
          </router-link>
        </li>
        <li>
          <router-link :to="`${baseURL}/plcs/create`">
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <i class="iconfont icon-add"></i>
            </span>
            <span>添加设备</span>
          </router-link>
        </li>
      </ul>
    </aside>

    <router-view
      :key="currentPlc?.id"
      :plc="currentPlc"
      :baseURL="`${baseURL}/plcs`"
      class="is-flex-auto"
      @refresh="getPlcs"
    />
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "vue";
import { useRoute } from "vue-router";
import { useIotHttp } from "@/services/iot-http-client";

export default defineComponent({
  name: "PlcList",

  setup() {
    const route = useRoute();
    const http = useIotHttp();

    const plcs = ref({
      result: [] as any,
      data: {} as any
    });

    const currentPlc = computed(() =>
      route.params.plcId && plcs.value.data[route.params.plcId as string]
    );

    const getPlcs = async () => {
      const response = await http.dataArray("/plcs/all");
      const result = [] as any;
      const data = {} as any;

      response.forEach(plc => {
        result.push(plc.id);
        data[plc.id] = plc;
      });

      plcs.value.data = data;
      plcs.value.result = result;
    };

    return {
      baseURL: "/iot",
      plcs,
      currentPlc,
      getPlcs,
    };
  }
});
</script>
