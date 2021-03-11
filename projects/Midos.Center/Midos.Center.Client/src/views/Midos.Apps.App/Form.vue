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
        v-model:value="app.key"
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
      style="width: 80px;"
    >
      名称
    </p>

    <p class="is-flex-auto">
      <Input v-model:value="app.name" />
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
      Url
    </p>

    <p class="is-flex-auto">
      <Textarea v-model:value="app.url" />
    </p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "AppForm",

  props: {
    type: {
      type: String,
      default: "update"
    },
    app: {
      type: Object,
      required: true
    }
  }
});
</script>
