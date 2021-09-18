<template>
  <td
    v-if="tag === 'input'"
    class="input"
    v-text="valueDate"
    @click="handleClick"
    style="cursor: pointer"
    ref="el"
  />
  <td
    v-else
    v-text="valueDate"
    class="is-clickable"
    @click="handleClick"
    ref="el"
  />
</template>

<script lang="ts">
import { defineComponent, ref, computed, onMounted, onBeforeUnmount } from "vue";
import Flatpickr from "flatpickr";
// eslint-disable-next-line import/extensions
import zh from "flatpickr/dist/l10n/zh.js";

const DateTime = {
  minValue: "0001-01-01T00:00:00",

  get now () {
    return new Date().toISOString();
  },

  getDate (dateTime: string) {
    return dateTime.substring(0, 10);
  }
};

export default defineComponent({
  name: "DatePicker",

  props: {
    value: {
      type: String,
      default: ""
    },

    clearable: {
      type: Boolean,
      default: true
    },

    readonly: {
      type: Boolean,
      default: false
    },

    nullable: {
      type: Boolean,
      default: false
    },

    tag: {
      type: String,
      default: "input"
    },

    locale: {
      type: String,
      default: "zh"
    },

    initial: {
      type: String,
      default: "min"
    }
  },

  setup(props, { emit }) {
    let datepicker = null as any;
    const el = ref<any>(null);

    const valueDate = computed(() => {
      const val = props.value.split("T")[0];

      if (val === "0001-01-01") {
        return "";
      }
      return val;
    });

    function handleInput (value: string) {
      emit("update:value", value);
    }

    function dateUpdated (selectedDates: any, value: any) {
      if (props.clearable && valueDate.value === value) {
        handleInput(DateTime.minValue);
      } else {
        handleInput(`${value}T00:00:00`);
      }
    }

    function handleValueChange (value: string) {
      if (datepicker) {
        datepicker.setDate(props.value === DateTime.minValue ? "" : value);
      }
    }

    function handleClick () {
      if (!datepicker) {
        const config = {} as any;

        config.onValueUpdate = dateUpdated;
        config.clickOpens = !props.readonly;

        if (props.locale === "zh") {
          config.locale = zh.zh;
        }

        datepicker = Flatpickr(el.value, config);
        handleValueChange(props.value);
        !props.readonly && datepicker.open();
      }
    }

    onMounted(() => {
      if (props.value === "") {
        if (props.initial === "min") {
          handleInput(DateTime.minValue);
        } else {
          handleInput(DateTime.now);
        }
      }
    });

    onBeforeUnmount(() => {
      if (datepicker) {
        datepicker.destroy();
        datepicker = null;
      }
    });

    return {
      el,
      valueDate,
      handleClick
    };
  }
});
</script>

<style lang="sass">
@import '~flatpickr/dist/flatpickr.min.css'
@import '~flatpickr/dist/themes/material_blue.css'

.flatpickr-calendar
  margin-top: 0.375rem

.dayContainer
  padding: 0.5rem 0.25rem 0.25rem 0.25rem

span.flatpickr-day
  margin-bottom: 0.125rem
</style>
