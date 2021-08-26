<template>
  <pre>{{text}}</pre>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { htmlDecode } from "js-htmlencode";
import xmlFormatter from "xml-formatter";

function parseJsonData(text: any) {
  try {
    if ((typeof text) === "string") {
      text = JSON.parse(text);
    }

    return JSON.stringify(text, null, 2);
  } catch {
    return null;
  }
}

function parseXmlData(text: any) {
  try {
    if (text.match("xml version=")) {
      console.log(100);

      return xmlFormatter(text, {
        indentation: "    "
      });
    }

    return null;
  } catch {
    return null;
  }
}

function decodeHtml(text: string) {
  try {
    return htmlDecode(text);
  } catch {
    return text;
  }
}

export default defineComponent({
  props: {
    value: {}
  },

  setup(props) {
    const value = decodeHtml(props.value as string);
    const text = parseJsonData(value)
      ?? parseXmlData(value)
      ?? value;

    return {
      text
    };
  }
});
</script>
