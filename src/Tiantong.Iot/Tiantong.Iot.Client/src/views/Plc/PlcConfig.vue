<template>
  <div class="box is-paddingless" style="margin: 1.25rem">
    <PlcForm
      :plc="plc"
      :handler="getPlc"
    >
      <template #footer>
        <div class="field" style="width: 480px">
          <div class="control is-flex">
            <AsyncButton
              :handler="handleSave"
              :disabled="!isChanged"
              class="button is-info is-small"
              style="margin-right: 0.5rem"
            >
              保存
            </AsyncButton>

            <AsyncButton
              :handler="handleTest"
              :disabled="isChanged"
              class="button is-success is-small"
              style="margin-right: 0.5rem"
            >
              连接测试
            </AsyncButton>

            <span class="is-flex-auto"></span>

            <AsyncButton
              :handler="handleDelete"
              class="button is-danger is-light is-small"
            >
              删除
            </AsyncButton>
          </div>
        </div>
      </template>
    </PlcForm>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import PlcForm from '@/components/form/PlcForm.vue'
import cloneDeep from 'lodash/cloneDeep'
import isEqual from 'lodash/isEqual'
import axios from '@/providers/axios'
import { Plc } from '@/entities'

@Component({
  name: 'PlcDashboard',
  components: {
    PlcForm
  }
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  plc = new Plc()

  sourceData = new Plc()

  get isChanged () {
    return !isEqual(this.plc, this.sourceData);
  }

  async getPlc () {
    let response = await axios.post('/plcs/find', {
      plc_id: this.plcId
    })

    this.sourceData = response.data
    this.plc = cloneDeep(this.sourceData)
  }

  async handleSave () {
    await axios.post('/plcs/update', this.plc)
    await this.getPlc()
    this.$emit('refresh')
  }

  async handleTest () {
    await axios.post('/plc-workers/test', {
      plc_id: this.plcId
    })
  }

  handleDelete () {
    this.$confirm({
      title: '提示',
      content: '删除后设备将无法恢复',
      handler: async () => {
        await axios.post('/plcs/delete', {
          plc_id: this.plcId
        })
        this.$emit('refresh')
        this.$router.push('/plcs')
      }
    })
  }
}
</script>
