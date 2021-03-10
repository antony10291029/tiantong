<template>
  <div
    class="is-flex is-flex-column is-flex-auto"
    style="min-width: 100px"
  >
    <div class="is-flex-auto"></div>

    <div class="is-flex is-centered">
      <PalletCode :code="code" @barcode-click="$emit('barcode-click')" />
    </div>

    <div
      class="has-background-primary is-flex is-vcentered is-centered has-text-white"
      style="height: 30px; min-width: 40px;"
    >
      {{text}}
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";
import PalletCode from "./PalletCode.vue";

export default defineComponent({
  name: "LifterPlatform",

  components: {
    PalletCode
  },

  props: {
    code: {
      type: String,
      required: true
    },
    text: {
      type: String,
      required: true
    },
  },

  setup(props) {
    const hasPalletCode = computed(() => {
      return parseInt(props.code, 10) && (props.code as any).length === 6;
    });

    return {
      hasPalletCode
    };
  }
});
</script>
