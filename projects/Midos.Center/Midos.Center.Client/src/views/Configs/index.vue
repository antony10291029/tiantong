<template>
  <AsyncLoader
    :handler="getConfigs"
    class="columns" style="padding: 1.25rem"
  >
    <div class="column is-narrow">
      <nav class="panel" style="width: 320px">
        <p class="panel-heading is-flex">
          <span>配置中心</span>
          <span class="is-flex-auto"></span>
          <router-link
            :to="{ name: 'MidosCenterConfigsCreate' }"
          >
            添加
          </router-link>
        </p>

        <router-link
          class="panel-block"
          v-for="config in configs" :key="config.key"
          style="padding: 0.25rem 0.75rem"
          :to="{
            name: 'MidosCenterConfigsConfig',
            params: { configKey: config.key }
          }"
        >
          <div
            class="is-flex is-flex-column"
            style="padding: 0.25rem"
          >
            <span>{{config.key}}</span>
            <span class="has-text-grey">{{config.value}}</span>
          </div>
        </router-link>
      </nav>
    </div>

    <router-view
      class="column"
      :key="$route.params.configKey"
      :configs="configs"
      @refresh="getConfigs"
    />
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useMidosCenterHttp } from "../../services/midos-center-http";

export default defineComponent({
  name: "Configs",

  setup() {
    const http = useMidosCenterHttp();
    const configs = ref<any[]>([]);

    async function getConfigs() {
      const result = await http.dataArray("/midos/configs");

      configs.value = result;
    }

    return {
      configs,
      getConfigs
    };
  }
});
</script>
