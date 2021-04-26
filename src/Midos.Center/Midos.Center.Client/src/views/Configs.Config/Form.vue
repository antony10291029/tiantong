<template>
  <div
    class="is-flex is-vcentered"
    style="padding: 0.75rem 0; max-width: 800px"
  >
    <p
      class="label"
      style="width: 80px;"
    >
      Key
    </p>

    <p class="is-flex-auto">
      <Input
        v-model:value="config.key"
        :readonly="type !== 'create'"
      />
    </p>
  </div>

  <hr>

  <div
    class="is-flex is-vcentered"
    style="padding: 0.75rem 0; max-width: 800px"
  >
    <p
      class="label"
      style="width: 80px; align-self: start"
    >
      Value
    </p>

    <p class="is-flex-auto">
      <Textarea v-model:value="config.value" />
    </p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "ConfigForm",

  props: {
    type: {
      type: String,
      default: "update"
    },
    config: {
      type: Object,
      required: true
    }
  }
});
</script>
