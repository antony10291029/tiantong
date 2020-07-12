<template>
  <section style="overflow: auto">
    <div class="box" style="margin: 1.25rem; padding: 0">
      <PlcForm :plc="plc">
        <template #header>
          <p class="title is-4">
            添加 PLC
          </p>

          <hr>
        </template>
        <template #footer>
          <hr>

          <div
            class="is-flex"
            style="padding: 0.75rem 0"
          >
            <div style="width: 100px"></div>
            <AsyncButton
              :handler="handleSubmit"
              class="button is-info is-small"
            >
              提交
            </AsyncButton>
          </div>
        </template>
      </PlcForm>
    </div>
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
