/* eslint-disable no-unused-expressions */
import clickoutside from "./clickoutside";

function active (el: any, binding: any) {
  binding.value
    ? el.classList.add("is-active")
    : el.classList.remove("is-active");
}

function loading (el: any, binnding: any) {
  binnding.value
    ? el.classList.add("is-loading")
    : el.classList.remove("is-loading");
}

function klass (el: any, binding: any) {
  if (binding.arg) {
    binding.value
      ? el.classList.add(binding.arg)
      : el.classList.remove(binding.arg);
  } else {
    binding.value
      ? el.classList.add(binding.value)
      : el.classList.remove(binding.value);
  }
}

function style (el: any, binding: any) {
  el.style[binding.arg] = binding.value;
}

export default {
  install (Vue: any) {
    Vue.directive("class", klass);
    Vue.directive("style", style);
    Vue.directive("active", active);
    Vue.directive("loading", loading);
    Vue.directive("clickoutside", clickoutside);
  }
};
