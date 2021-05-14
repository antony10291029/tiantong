<template>
  <div class="field">
    <p class="label">名称</p>

    <p class="control">
      <Input v-model:value="entity.name" />
    </p>
  </div>

  <div class="field">
    <p class="label">路由</p>

    <div class="control">
      <Input v-model:value="entity.path" />
    </div>
  </div>

  <div class="field">
    <p class="label">服务</p>

    <div class="control">
      <EndpointSelector v-model:value="entity.endpointId" />
    </div>
  </div>

  <div class="field">
    <p class="label">服务路由</p>

    <div class="control">
      <Input v-model:value="entity.endpointPath" />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import EndpointSelector from "./EndpointSelector.vue";

export default defineComponent({
  components: {
    EndpointSelector
  },

  props: {
    entity: {
      type: Object,
      required: true
    }
  },

  setup() {
    return {

    };
  }
});
</script>
