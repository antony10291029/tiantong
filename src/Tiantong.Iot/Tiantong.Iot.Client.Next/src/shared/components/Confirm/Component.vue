<template>
  <transition leave-active-class="duration-300">
    <div
      v-show="isShow"
      :class="`
        fixed z-50 inset-0
        overflow-y-auto
      `"
    >
      <div
        class="
          w-screen h-screen
          p-4 text-center
          flex items-center justify-center
        "
      >
        <transition name="ease-out-overlay">
          <div
            v-show="isShow"
            class="fixed inset-0 bg-dark-900 bg-opacity-80 transition-opacity"
          />
        </transition>

        <transition name="ease-out-modal">
          <div
            v-show="isShow"
            class="
              transform transition-all
              bg-dark-700 px-6 py-6 w-96
              inline-block align-bottom text-gray-200
              rounded text-left overflow-hidden
              z-50 m-0
            "
          >
            <div class="flex items-start mb-6">
              <div class="text-left">
                <h3 class="text-xl leading-6 mb-4">
                  {{ title }}
                </h3>
                <p>
                  {{ content }}
                </p>
              </div>
            </div>

            <div class="flex flex-wrap-reverse">
              <div class="flex-auto" />
              <button
                type="button"
                class="
                  inline-flex justify-center
                  rounded border border-transparent shadow-sm
                  px-4 py-1.5 font-medium bg-dark-600
                  focus:outline-none hover:bg-dark-500
                  focus:ring-red-500 ml-3 w-auto text-sm
                "
                @click="close"
              >
                取消
              </button>
              <button
                type="button"
                class="
                  inline-flex justify-center
                  rounded
                  shadow-sm px-4 py-1.5 font-medium
                  bg-danger-500 outline-none
                  mb-0
                  hover:bg-danger-600 focus:outline-none
                  focus:ring-indigo-500 mt-0 ml-3 w-auto text-sm
                "
                @click="handleConfirm"
              >
                确认
              </button>
            </div>
          </div>
        </transition>
      </div>
    </div>
  </transition>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { Options } from "./interface";

export default defineComponent({
  setup() {
    let callback = () => {};
    const isShow = ref(false);
    const title = ref("");
    const content = ref("");
    const close = () => isShow.value = false;
    const handleConfirm = async () => {
      await callback();
      isShow.value = false;
    };
    const open = (options: Options) => {
      isShow.value = true;
      title.value = options.title;
      content.value = options.content;
      callback = options.callback;
    };

    return {
      isShow,
      title,
      content,
      open,
      close,
      handleConfirm,
    };
  }
});
</script>

<style lang="postcss">
.badge {
  @apply inline-block bg-gray-200 rounded-full px-3 py-1 text-sm font-semibold text-gray-700;
  &:hover {
    @apply bg-gray-300;
  }
}

.ease-out-overlay-enter-active,
.ease-out-overlay-leave-active {
  @apply opacity-100 duration-300;
}

.ease-out-overlay-enter, .ease-out-overlay-leave-to {
  @apply opacity-0 duration-200;
}

.ease-out-modal-enter-active,
.ease-out-modal-leave-active {
  @apply opacity-100 translate-y-0 scale-100 duration-300;
}

.ease-out-modal-enter, .ease-out-modal-leave-to {
  @apply ease-in opacity-0 translate-y-4 scale-95 duration-200;
}
</style>
