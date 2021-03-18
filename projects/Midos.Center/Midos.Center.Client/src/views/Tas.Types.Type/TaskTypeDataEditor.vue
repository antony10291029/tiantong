<template>
  <div>
    <table class="table is-bordered is-fullwidth">
      <thead>
        <th style="width: 200px">Key</th>
        <th>Value</th>
        <th style="width: 1px">
          <a @click="addItem">
            <span class="icon">
              <i class="icon-midos-center icon-midos-center-plus"></i>
            </span>
          </a>
        </th>
      </thead>
      <tbody v-if="list.length">
        <tr v-for="(item, index) in list" :key="index">
          <EditableCell v-model:value="item.key" />
          <EditableCell v-model:value="item.value" />
          <td>
          <a @click="removeItem(index)">
            <span class="icon has-text-danger">
              <i class="icon-midos-center icon-midos-center-close"></i>
            </span>
          </a>
          </td>
        </tr>
      </tbody>
      <tbody v-else>
        <td class="is-centered" colspan="3">无</td>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watch } from "vue";
import EditableCell from "../../components/EditableCell.vue";

export default defineComponent({
  name: "TaskTypeDataEditor",

  components: {
    EditableCell
  },

  props: {
    value: {
      type: Object,
      required: true
    }
  },

  setup(props, { emit }) {
    const list = ref(Object.keys(props.value).map(
      key => ({ key, value: props.value[key] })
    ));

    function update() {
      const obj = {} as any;

      list.value.forEach(item => obj[item.key] = item.value);

      emit("update:value", obj);
    }

    function addItem() {
      const index = list.value.length + 1;
      const item = {
        key: `key_${index}`,
        value: `value_${index}`
      };

      list.value.push(item);
    }

    function removeItem(index: number) {
      list.value.splice(index, 1);
    }

    watch(
      () => list,
      () => update(),
      { deep: true }
    );

    return {
      list,
      addItem,
      removeItem,
      update
    };
  }
});
</script>
