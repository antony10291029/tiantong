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
        {{ index }}
      </span>
    </td>

    <td class="text-left">
      <div class="flex items-center">
        <span
          class="
            px-2 inline-flex
            leading-5 font-semibold
            rounded-full bg-success-300
            text-success-800 text-xs
          "
        >
          运行中
        </span>
      </div>
    </td>

    <td class="whitespace-nowrap text-left">
      <div class="flex items-center">
        <div class="font-medium">
          {{ plc.name }}
        </div>
      </div>
    </td>

    <td class="hidden lg:table-cell">
      <div>
        {{ plc.host }}:{{ plc.port }}
      </div>
    </td>

    <td class="text-center">
      <router-link
        custom
        :to="`/plcs/${plc.id}`"
        #="{ navigate }"
      >
        <a
          @click="navigate"
          class="text-link-500 hover:text-gray-300 cursor-pointer"
        >
          管理
        </a>
      </router-link>

      <a class="hidden sm:inline text-link-500 hover:text-gray-300 cursor-pointer ml-2">
        停止
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
import { useConfirm } from "../../shared/Confirm";
import { PlcConfig, PlcConfigContext } from "../../domain";

export default defineComponent({
  props: {
    index: {
      type: Number,
      required: true
    },

    plc: {
      type: Object as PropType<PlcConfig>,
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
          await PlcConfigContext.removeById(props.plc.id);
          emit("deleted");
        }
      });
    };

    return {
      handleDelete
    };
  }
});
</script>
