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
      <label class="label">字段名</label>
      <div class="control">
        <input
          v-model="pusher.value_key"
          type="text" class="input"
        >
      </div>
    </div>

    <div class="field">
      <label class="label">推送条件</label>
    </div>

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

    <div class="field" style="width: 480px">
      <label class="label">附加数据</label>
      <div class="control">
        <Textarea v-model="pusher.data"></Textarea>
      </div>
    </div>

    <div class="class">
      <label class="label">
        <Checkbox v-model="pusher.is_value_to_string">转为字符串</Checkbox>
      </label>
      <div class="control">

      </div>
    </div>

    <div class="class">
      <label class="label">
        <Checkbox v-model="pusher.is_concurrent">支持并发</Checkbox>
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
