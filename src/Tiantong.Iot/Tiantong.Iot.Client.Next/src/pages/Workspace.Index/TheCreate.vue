<template>
  <div
    v-if="isCreateShow"
    class="absolute flex items-center"
  >
    <input
      ref="input"
      type="text"
      placeholder="请输入设备名称......"
      class="
        transform relative
        w-48 px-3 py-1.5
        z-50 text-grey-200
        placeholder-gray-500
        outline-none ring-1 ring-transparent
        bg-dark-800 hover:bg-dark-700 focus:bg-dark-700
      "
      :value="inputValue"
      @input="handleInput"
    >

    <button
      class="
        cursor-pointer z-50
        ml-2 flex px-4 py-1.5
        items-center justify-center
        ring-2 ring-transparent bg-primary-600
        hover:bg-primary-700
        active:bg-primary-800
      "
      @click="handleSubmit"
    >
      创建
    </button>

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
import axios from "../../services/http-client";

export default defineComponent({
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
      await axios.post("/plcs/create", {
        name: inputValue.value
      });
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
