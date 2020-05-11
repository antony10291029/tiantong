<template>
  <div
    class="is-bordered"
    style="padding: 1.25rem; border-top: none"
  >
    <div class="field" style="width: 320px">
      <label class="label">推送名</label>
      <div class="control">
        <input
          v-model="pusher.name"
          type="text" class="input"
        >
      </div>
    </div>

    <div class="field" style="width: 320px">
      <label class="label">URL</label>
      <div class="control">
        <input
          v-model="pusher.url"
          type="text" class="input"
        >
      </div>
    </div>

    <div class="field" style="width: 320px">
      <label class="label">条件</label>
      <div class="control">
        <input
          v-model="pusher.when_opt"
          type="text" class="input"
        >
      </div>
    </div>

    <div class="field" style="width: 320px">
      <label class="label">条件值</label>
      <div class="control">
        <input
          v-model="pusher.when_value"
          type="text" class="input"
        >
      </div>
    </div>

    <div class="field" style="width: 320px">
      <label class="label">字段名</label>
      <div class="control">
        <input
          v-model="pusher.value_key"
          type="text" class="input"
        >
      </div>
    </div>

    <div class="field" style="width: 480px">
      <label class="label">附加数据</label>
      <div class="control">
        <Textarea v-model="pusher.data"></Textarea>
      </div>
    </div>

    <div class="class">
      <label class="label">
        <Checkbox v-model="pusher.to_string">转为字符串</Checkbox>
      </label>
      <div class="control">

      </div>
    </div>

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
