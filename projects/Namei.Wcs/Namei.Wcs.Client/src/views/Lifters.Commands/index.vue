<template>
  <div style="padding: 1.25rem; overflow: auto">
    <div class="columns">
      <div class="column is-narrow">
        <div class="panel" style="width: 280px">
          <div class="panel-heading">
            设备名称
          </div>

          <a
            v-for="lifter in lifters" :key="lifter.id"
            @click="lifterId = lifter.id"
            class="panel-block"
            v-active="lifter.id === lifterId"
          >
            {{lifter.text}}
          </a>
        </div>
      </div>

      <div class="column">
        <LifterView :lifterId="lifterId" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import LifterView from "./LifterView.vue";

export default defineComponent({
  name: "LifterCommands",

  components: {
    LifterView
  },

  setup() {
    const lifterId = ref(1);

    return {
      lifterId,
      lifters: [
        { id: 1, text: "1 号梯（改造货梯）" },
        { id: 2, text: "2 号梯（提升机）" },
        { id: 3, text: "3 号梯（提升机）" }
      ]
    };
  }
});
</script>
