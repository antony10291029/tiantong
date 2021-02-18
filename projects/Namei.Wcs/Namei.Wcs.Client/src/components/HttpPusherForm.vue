<template>
  <div>
    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        推送名
      </label>
      <Input
        v-model:value="pusher.name"
        type="text"
        style="width: 320px"
      />
    </div>

    <hr>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        编号
      </label>
      <Input
        v-model:value="pusher.number"
        type="text"
        style="width: 320px"
      />
    </div>

    <hr>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        URL
      </label>
        <Input
          v-model:value="pusher.url"
          type="text"
          style="width: 320px"
        />
    </div>

    <hr>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        字段名
      </label>
      <Input
        v-model:value="pusher.field"
        type="text"
        style="width: 320px"
      />
    </div>

    <hr>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px; align-self: start">
        Header
      </label>

      <div style="width: 480px">
        <Textarea v-model:value="pusher.header" />
      </div>
    </div>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px; align-self: start">
        Body
      </label>

      <div style="width: 480px">
        <Textarea v-model:value="pusher.body" />
      </div>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px;">
        转字符串
      </label>

      <Switcher v-model:value="pusher.to_string" />
    </div>

    <hr>

    <slot name="footer" />
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { HttpPusher } from "@/entities";

export default defineComponent({
  name: "PlcStateHttpPusher",

  props: {
    pusher: {
      type: Object as PropType<HttpPusher>,
      required: true
    }
  }
});
</script>
