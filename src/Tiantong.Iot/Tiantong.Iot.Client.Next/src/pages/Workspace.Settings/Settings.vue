<template>
  <div class="p-4 flex flex-col text-md rounded-2xl last:mb-0 flex-auto">
    <p class="mb-4 text-2xl">
      系统设置
    </p>

    <SettingsRow>
      1234
    </SettingsRow>

    <SettingsRow>
      1234
    </SettingsRow>

    <SettingsRow>
      1234
    </SettingsRow>

    <SettingsRow>
      <div>
        <p class="mb-1">
          清理日志
        </p>
        <p>
          日志数据量过大将影响系统性能，若有需要可根据以下条件清理进行清理。
        </p>
      </div>
      <div class="flex-auto"></div>
      <div>
        asdfg
      </div>
    </SettingsRow>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import SettingsRow from "./SettingsRow.vue";

export default defineComponent({
  components: {
    SettingsRow,
  },

  setup() {
    return {

    };
  }
});
</script>
