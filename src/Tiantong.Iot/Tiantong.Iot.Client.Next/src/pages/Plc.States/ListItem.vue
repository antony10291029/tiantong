<template>
  <tr class="hover:bg-dark-800">
    <td>
      <span
        class="
          flex items-center justify-center
          rounded-full text-md
          bg-primary-600 w-8 h-8
        "
      >
        {{ index + 1 }}
      </span>
    </td>

    <td class="hidden sm:table-cell whitespace-nowrap text-left">
      {{ state.number }}
    </td>

    <td class="whitespace-nowrap text-left">
      {{ state.address }}
    </td>

    <td class="hidden sm:table-cell whitespace-nowrap text-left">
      {{ state.type }}
    </td>

    <td class="whitespace-nowrap text-left">
      {{ state.name }}
    </td>

    <td class="text-center">
      <a class="text-link-500 hover:text-gray-300 cursor-pointer">
        修改
      </a>

      <a
        class="hidden sm:inline text-link-500 hover:text-gray-300 cursor-pointer ml-2"
        @click="handleDelete"
      >
        删除
      </a>
    </td>
  </tr>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { useConfirm } from "../../shared/components/Confirm";
import { PlcStateConfig } from "../../domain";

export default defineComponent({
  props: {
    index: {
      type: Number,
      required: true
    },

    state: {
      type: Object as PropType<PlcStateConfig>,
      required: true
    }
  },

  emits: ["deleted"],

  setup(props, { emit }) {
    const confirm = useConfirm();
    const handleDelete = async () => {
      confirm({
        title: "提示",
        content: "删除后设备将无法恢复",
        callback: async () => {

        }
      });
    };

    return {
      handleDelete
    };
  }
});
</script>
