<template>
  <div>
    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        推送名
      </label>
      <input
        v-model="pusher.name"
        type="text" class="input"
        style="width: 320px"
      >
    </div>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        编号
      </label>
      <input
        v-model="pusher.number"
        type="text" class="input"
        style="width: 320px"
      >
    </div>

    <hr>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        URL
      </label>
        <input
          v-model="pusher.url"
          type="text" class="input"
          style="width: 320px"
        >
    </div>

    <hr>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        字段名
      </label>
      <input
        v-model="pusher.field"
        type="text" class="input"
        style="width: 320px"
      >
    </div>

    <hr>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px; align-self: start">
        Header
      </label>

      <div style="width: 480px">
        <Textarea v-model="pusher.header"></Textarea>
      </div>
    </div>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px; align-self: start">
        Body
      </label>

      <div style="width: 480px">
        <Textarea v-model="pusher.body"></Textarea>
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

      <Switcher v-model="pusher.to_string"></Switcher>
    </div>

    <hr>

    <slot name="footer" />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import { HttpPusher } from '@/entities'

@Component({
  name: 'PlcStateHttpPusher'
})
export default class extends Vue {
  @Prop({ required: true })
  pusher!: HttpPusher
}
</script>
