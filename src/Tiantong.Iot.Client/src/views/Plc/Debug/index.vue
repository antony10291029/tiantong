<template>
  <section style="padding: 1.25rem">
    <div class="columns" style="height: 100%;">
      <div class="column is-4">
        <div class="field">
          <label class="label">
            操作
          </label>
        </div>
        <div class="field has-addons">
          <div class="control">
            <a
              v-class:is-focused="params.operation == 'get'"
              @click="params.operation = 'get'"
              class="button is-small"
            >
              数据读取
            </a>
          </div>
          <div class="control">
            <a
              v-class:is-focused="params.operation == 'set'"
              @click="params.operation = 'set'"
              class="button is-small"
            >
              数据写入
            </a>
          </div>
        </div>

        <div class="field">
          <label class="label">
            数据类型
          </label>
          <PlcStateTypes
            v-model="params.type"
            :length="params.length"
          />
        </div>

        <div
          class="field"
          v-if="params.type == 'string'"
        >
          <label class="label">字符串长度</label>
          <div class="control">
            <input
              v-model.number="params.length"
              type="text" class="input"
            >
          </div>
        </div>

        <div class="field">
          <label class="label">地址</label>
          <div class="control">
            <input
              v-model="params.address"
              type="text" class="input"
            >
          </div>
        </div>

        <div
          v-if="params.operation === 'set'"
          class="field"
        >
          <label class="label">写入数据</label>
          <div class="control">
            <input
              type="text" class="input"
              v-model.lazy="params.value"
            >
          </div>
        </div>

        <div class="field">
          <div class="control">
            <AsyncButton
              class="button is-small is-info is-light"
              :handler="handleOperation"
            >
              开始执行
            </AsyncButton>
          </div>
        </div>
      </div>

      <div class="column" style="height: 100%">
        <textarea
          readonly
          :value="logs.join('\n')"
          class="textarea is-family-monospace"
          style="height: 100%; min-width: 400px;"
        ></textarea>
      </div>
    </div>
  </section>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'
import DateTime from '@/utils/DateTime'
import PlcStateTypes from '@/components/PlcStateTypes.vue'

@Component({
  name: 'PlcDebug',
  components: {
    PlcStateTypes
  }
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  operation = 'get'

  type = 'uint16'

  logs = [] as string[]

  params = {
    operation: 'get',
    type: 'uint16',
    address: 'D100',
    length: 10,
    value: '',
  }

  types = [
    { value: 'uint16', text: 'uint16' },
    { value: 'int32', text: 'int32' },
    { value: 'string', text: 'ASCII 字符串' },
  ]

  async handleOperation () {
    let url = `plc-workers/debug/${this.params.type}/${this.params.operation}`

    let response = await axios.post(url, {
      plc_id: this.plcId,
      address: this.params.address,
      length: this.params.length,
      value: this.params.value
    })

    let time = DateTime.now.split('T')[1].split('.')[0]

    this.logs.unshift(`[${time}] ` + response.data.message)
    if (this.logs.length > 50) {
      this.logs.splice(50, this.logs.length - 50)
    }
  }
}
</script>
