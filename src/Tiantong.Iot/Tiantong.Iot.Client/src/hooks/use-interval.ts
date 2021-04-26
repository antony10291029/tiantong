import { onBeforeUnmount, Ref, watchEffect } from "vue";

export function useInterval(func: () => Promise<void> | void, flag: Ref<boolean>, time = 1000) {
  let intervalId = 0;

  function start() {
    intervalId = setInterval(async () => {
      func();
    }, time);
  }

  function stop(): Promise<void> | void {
    clearInterval(intervalId);
  }

  onBeforeUnmount(stop);

  watchEffect(() => {
    if (flag.value === true) {
      start();
    } else {
      stop();
    }
  });
}
