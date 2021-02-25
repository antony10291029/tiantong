<template>
  <div
    class="is-flex is-flex-column"
    style="width: 100px"
  >
    <div class="is-flex-auto"></div>

    <div class="is-flex is-centered">
      <PalletCode :code="code" />
    </div>

    <div
      class="has-background-primary is-flex is-vcentered is-centered has-text-white"
      style="height: 30px; width: 100px"
    >
      {{text}}
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue'
import PalletCode from './PalletCode.vue'

export default Vue.extend({
  name: 'LifterPlatform',

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

  computed: {
    hasPalletCode () {
      return parseInt(this.code) && (this.code as any).length == 6
    }
  }
})
</script>
