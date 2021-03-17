<template>
  <div>
    <div class="box" style="margin: 1.25rem">
      <div class="columns">
        <div class="column is-narrow" style="margin-right: 1rem;">
          <p class="title is-4">
            任务管理
          </p>

          <hr>

          <div
            class="is-flex is-vcentered"
            style="padding: 0.75rem 0;"
          >
            <label class="label" style="width: 120px">
              操作类型
            </label>

            <div
              class="field has-addons"
              style="width: 400px"
            >
              <div
                v-for="operation in operations"
                :key="operation.type"
                class="control"
              >
                <a
                  v-class:is-focused="currentOperation === operation.type"
                  @click="currentOperation = operation.type"
                  class="button is-small"
                  style="width: 100px"
                >
                  {{operation.text}}
                </a>
              </div>
            </div>
          </div>

          <hr>

          <CreateTask
            v-show="currentOperation === 'create'"
            @log="handleLog"
          />
          <ContinueTask
            v-show="currentOperation === 'continue'"
            @log="handleLog"
          />
          <CancelTask
            v-show="currentOperation === 'cancel'"
          />
        </div>

        <div class="column">
          <Textarea
            class="is-family-monospace"
            readonly
            style="height: 800px"
            :value="log"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import CreateTask from "./CreateTask.vue";
import ContinueTask from "./ContinueTask.vue";
import CancelTask from "./CancelTask.vue";

const operations = [
  { type: "create", text: "下发任务" },
  { type: "continue", text: "继续任务" },
  { type: "cancel", text: "取消任务" },
];

export default defineComponent({
  name: "NameiWcsRcsMap",

  components: {
    CreateTask,
    ContinueTask,
    CancelTask
  },

  setup() {
    const currentOperation = ref("create");
    const log = ref("");

    function handleLog(text: string) {
      log.value = `${text}\n${log.value}`;
    }

    return {
      log,
      operations,
      currentOperation,
      handleLog
    };
  }
});
</script>
