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
import { useMidosCenterApi } from "../../services/midos-center-api";

export default defineComponent({
  name: "Configs",

  setup() {
    const api = useMidosCenterApi();
    const configs = ref<any[]>([]);

    async function getConfigs() {
      const result = await api.getConfigs();

      configs.value = result;
    }

    return {
      configs,
      getConfigs
    };
  }
});
</script>
