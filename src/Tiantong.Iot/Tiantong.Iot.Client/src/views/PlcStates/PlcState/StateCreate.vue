<template>
  <div>
    <div
      class="tabs is-bordered"
      style="margin-bottom: 0; border-bottom: none"
    >
      <ul>
        <li class="is-active">
          <a>创建数据点</a>
        </li>
      </ul>
    </div>

    <PlcStateForm :state="state" />

    <div style="height: 0.75rem"></div>

    <AsyncButton
      class="button is-info is-small"
      style="margin-right: 0.5rem"
      :handler="handleSubmit"
    >
      提交
    </AsyncButton>

    <a
      @click="$router.go(-1)"
      class="button is-info is-light is-small"
    >
      返回
    </a>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import PlcStateForm from '@/components/form/PlcStateForm.vue'
import { PlcState } from '@/entities'
import axios from '@/providers/axios'

@Component({
  name: 'PlcStateCreate',
  components: {
    PlcStateForm
  }
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  state = new PlcState()

  async handleSubmit () {
    let response = await axios.post('/plcs/states/create', this.state)
    let id = response.data.id
    this.$router.push(`${id}/detail`)
    this.$emit('refresh')
  }

  created () {
    this.state.plc_id = this.plcId
  }
}
</script>
