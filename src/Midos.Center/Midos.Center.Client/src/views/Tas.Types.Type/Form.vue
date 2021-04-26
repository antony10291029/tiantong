<template>
  <div>
    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0; max-width: 800px"
    >
      <p
        class="label"
        style="width: 100px;"
      >
        Key
      </p>

      <p class="is-flex-auto">
        <Input
          v-model:value="taskType.key"
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
        style="width: 100px;"
      >
        名称
      </p>

      <p class="is-flex-auto">
        <Input
          v-model:value="taskType.name"
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
        style="width: 100px; align-self: start"
      >
        备注
      </p>

      <p class="is-flex-auto">
        <Textarea
          v-model:value="taskType.comment"
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
        style="width: 100px; align-self: start"
      >
        数据
      </p>

      <p class="is-flex-auto">
        <TaskTypeDataEditor v-model:value="taskType.data"/>
      </p>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0; max-width: 800px"
    >
      <p
        class="label"
        style="width: 100px; align-self: start"
      >
        子任务
      </p>

      <div class="is-flex-auto">
        <table class="table is-fullwidth is-bordered">
          <thead>
            <th style="width: 200px">Key</th>
            <th>子任务名</th>
            <th style="width: 1px">
              <a @click="addSubtype">
                <span class="icon">
                  <i class="icon-midos-center icon-midos-center-plus"></i>
                </span>
              </a>
            </th>
          </thead>

          <tbody v-if="subtypes.length">
            <tr v-for="(subtype, index) in subtypes" :key="index">
              <EditableCell
                v-model:value="subtype.key"
              />
              <SubtypeSelector
                v-model:value="subtype.subtypeId"
                :typeId="taskType.id"
                :taskTypes="$attrs.taskTypes"
              />
              <td>
                <a
                  class="icon has-text-danger"
                  @click="removeSubtype(index)"
                >
                  <i class="icon-midos-center icon-midos-center-close"></i>
                </a>
              </td>
            </tr>
          </tbody>
          <tbody v-else>
            <td class="is-centered" colspan="3">
              无
            </td>
          </tbody>
        </table>
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0; max-width: 800px"
    >
      <p
        class="label"
        style="width: 100px;"
      >
      </p>

      <slot name="foot"></slot>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent } from "vue";
import EditableCell from "../../components/EditableCell.vue";
import SubtypeSelector from "./SubtypeSelector.vue";
import TaskTypeDataEditor from "./TaskTypeDataEditor.vue";

export default defineComponent({
  name: "TaskTypeForm",

  components: {
    EditableCell,
    SubtypeSelector,
    TaskTypeDataEditor
  },

  props: {
    taskType: {
      type: Object,
      required: true
    }
  },

  setup(props) {
    const subtypes = computed(() => props.taskType.subtypes);

    function addSubtype() {
      const { length } = subtypes.value;

      subtypes.value.push({
        id: 0,
        index: length,
        key: `key_${length + 1}`,
        name: `子任务_${length + 1}`,
        typeId: props.taskType.id,
        subtypeId: 0,
      });
    }

    function removeSubtype(index: number) {
      subtypes.value.splice(index, 1);
    }

    return {
      subtypes,
      addSubtype,
      removeSubtype
    };
  }
});
</script>
