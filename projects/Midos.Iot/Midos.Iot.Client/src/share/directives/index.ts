/* eslint-disable no-unused-expressions */
import { Plugin } from "vue";
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
  install (app) {
    app.directive("class", klass);
    app.directive("style", style);
    app.directive("active", active);
    app.directive("loading", loading);
    app.directive("clickoutside", clickoutside as any);
  }
} as Plugin;
