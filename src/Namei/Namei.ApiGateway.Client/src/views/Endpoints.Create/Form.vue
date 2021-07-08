<template>
  <hr>

  <div
    class="is-flex is-vcentered"
    style="padding: 0.75rem 0; max-width: 600px"
  >
    <p
      class="label"
      style="width: 100px;"
    >
      名称
    </p>

    <p class="is-flex-auto">
      <Input v-model:value="entity.name" />
    </p>
  </div>

  <hr>

  <div
    class="is-flex is-vcentered"
    style="padding: 0.75rem 0; max-width: 600px"
  >
    <p
      class="label"
      style="width: 100px;"
    >
      URL
    </p>

    <p class="is-flex-auto">
      <Input v-model:value="entity.url" />
    </p>
  </div>

  <hr>

  <div
    class="is-flex is-vcentered"
    style="padding: 0.75rem 0; max-width: 600px"
  >
    <p
      class="label"
      style="width: 100px;"
    >
      URL(Test)
    </p>

    <p class="is-flex-auto">
      <Input v-model:value="entity.urlStaging" />
    </p>
  </div>

  <hr>

  <div
    class="is-flex is-vcentered"
    style="padding: 0.75rem 0; max-width: 600px"
  >
    <p style="width: 100px"></p>
    <p class="is-flex-auto is-flex">
      <slot name="footer" />
    </p>
  </div>

</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
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
