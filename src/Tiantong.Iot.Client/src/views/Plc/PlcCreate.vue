<template>
  <section>
    <div class="tabs is-boxed" style="margin-bottom: 0">
      <ul>
        <li class="is-active">
          <a>添加 PLC</a>
        </li>
      </ul>
    </div>
    <PlcForm :plc="plc">
      <template #footer>
        <AsyncButton
          :handler="handleSubmit"
          class="button is-info is-small"
        >
          提交
        </AsyncButton>
      </template>
    </PlcForm>
  </section>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import PlcForm from '@/components/form/PlcForm.vue'
import { Plc } from '@/entities/Plc'
import axios from '@/providers/axios'

@Component({
  name: 'PlcCreate',
  components: {
    PlcForm
  }
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  plc = new Plc()

  async handleSubmit () {
    let response = await axios.post('/plcs/create', this.plc)
    let id = response.data.id
    this.$router.push(`${this.baseURL}/${id}/states`)
    this.$emit('refresh')
  }
}
</script>
