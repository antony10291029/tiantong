<template>
  <AsyncLoader
    :handler="getApps"
    class="columns" style="padding: 1.25rem"
  >
    <div class="column is-narrow">
      <nav class="panel" style="width: 320px">
        <p class="panel-heading is-flex">
          <span>应用列表</span>
          <span class="is-flex-auto"></span>
          <router-link
            :to="{ name: 'MidosCenterAppsCreate' }"
          >
            添加
          </router-link>
        </p>

        <router-link
          class="panel-block"
          v-for="app in apps" :key="app.id"
          style="padding: 0.25rem 0.75rem"
          :to="{
            name: 'MidosCenterAppsApp',
            params: { id: app.id }
          }"
        >
          <div
            class="is-flex is-flex-column"
            style="padding: 0.25rem"
          >
            <span>{{app.name}}</span>
            <span class="has-text-grey">{{app.url}}</span>
          </div>
        </router-link>
      </nav>
    </div>

    <router-view
      class="column"
      :key="$route.params.id"
      :apps="apps"
      @refresh="getApps"
    />
  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useMidosCenterApi } from "../../services/midos-center-api";

export default defineComponent({
  name: "Apps",

  setup() {
    const api = useMidosCenterApi();
    const apps = ref<any[]>([]);

    async function getApps() {
      const result = await api.getApps();

      apps.value = result;
    }

    return {
      apps,
      getApps
    };
  }
});
</script>
