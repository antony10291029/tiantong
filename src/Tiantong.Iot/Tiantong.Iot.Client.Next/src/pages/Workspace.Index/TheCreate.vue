<template>
  <div
    v-if="isCreateShow"
    class="absolute flex items-center"
  >
    <div class="w-48">
      <Input
        class="z-50"
        placeholder="请输入设备名称......"
        :value="inputValue"
        @input="handleInput"
      />
    </div>

    <Button
      class="z-50 ml-2"
      @click="handleSubmit"
    >
      创建
    </Button>

    <button
      class="
        cursor-pointer z-50
        ml-2 flex px-2 py-1.5
        hover:text-gray-50
      "
      @click="isCreateShow = false"
    >
      <i class="iconfont icon-close" />
    </button>

    <div class="fixed z-30 inset-0 bg-dark-900 bg-opacity-90 transition-opacity" />
  </div>

  <button
    v-if="!isCreateShow"
    class="
      px-3 py-1.5 ml-2
      cursor-pointer
      hover:text-white
    "
    @click="handleClick"
  >
    <i class="iconfont icon-plus" />
  </button>
</template>

<script lang="ts">
import { defineComponent, ref, nextTick } from "vue";
import { PlcConfigContext } from "../../domain";
import Button from "../../shared/components/Button/index.vue";
import Input from "../../shared/components/Input/index.vue";

export default defineComponent({
  components: {
    Button,
    Input,
  },

  emits: [
    "created"
  ],

  setup(props, { emit }) {
    const inputValue = ref("");
    const isCreateShow = ref(false);
    const input = ref<HTMLInputElement>();
    const handleInput = (event: any) => inputValue.value = event.target.value;
    const handleClick = () => {
      isCreateShow.value = true;
      nextTick(() => input.value?.focus());
    };
    const handleSubmit = async () => {
      await PlcConfigContext.addByName(inputValue.value);
      emit("created");
      isCreateShow.value = false;
    };

    return {
      input,
      inputValue,
      isCreateShow,
      handleInput,
      handleClick,
      handleSubmit,
    };
  }
});
</script>
