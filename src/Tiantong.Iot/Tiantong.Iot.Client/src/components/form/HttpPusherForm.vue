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

    <hr>

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
        v-model="pusher.value_key"
        type="text" class="input"
        style="width: 320px"
      >
    </div>

    <hr>

    <div class="is-flex is-vcentered" style="padding: 0.75rem 0">
      <label class="label" style="width: 100px">
        推送条件
      </label>

      <div
        class="field has-addons"
        style="width: 320px"
      >
        <div
          v-if="pusher.when_opt === ''"
          class="control is-expanded"
        >
          <div class="select is-fullwidth">
            <select v-model="pusher.when_opt">
              <option
                v-for="option in whenOptions" :key="option.value"
                :value="option.value"
              >
                {{option.text}}
              </option>
            </select>
          </div>
        </div>
        <template v-else>
          <div class="control">
            <div class="select">
              <select v-model="pusher.when_opt">
                <option
                  v-for="option in whenOptions" :key="option.value"
                  :value="option.value"
                >
                  {{option.text}}
                </option>
              </select>
            </div>
          </div>
          <div class="control is-expanded">
            <input
              v-model="pusher.when_value"
              type="text" class="input"
            >
          </div>
        </template>
      </div>
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

      <Switcher v-model="pusher.is_value_to_string"></Switcher>
    </div>

    <hr>

    <div
      class="is-flex is-vcentered"
      style="padding: 0.75rem 0"
    >
      <label class="label" style="width: 100px;">
        并发推送
      </label>

      <Switcher v-model="pusher.is_concurrent"></Switcher>
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

  whenOptions = [
    { text: '无', value: '' },
    { text: '等于', value: '==' },
    { text: '不等于', value: '!=' },
    { text: '大于', value: '>' },
    { text: '大于等于', value: '>=' },
    { text: '小于', value: '<' },
    { text: '小于等于', value: '<=' },
  ]
}
</script>
