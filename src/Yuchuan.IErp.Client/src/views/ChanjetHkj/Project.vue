<template>
  <AsyncLoader
    class="is-flex-auto"
    :handler="getDetails"
  >
    <ProjectChart
      :details="details"
      :categories="categories"
    />

  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import chanjetContext from '@/providers/contexts/chanjet'
import ProjectChart from './ProjectChart.vue'

@Component({
  name: 'ProjectDetail',
  components: {
    ProjectChart
  }
})
export default class extends Vue {
  @Prop({ required: true })
  orgCode!: string

  @Prop({ required: true })
  bookCode!: string

  @Prop({ required: true })
  projectCode!: string

  @Prop({ required: true })
  categories!: any[]

  details = [] as any[]

  get totalPostedDr () {
    return this.details.reduce((s, d) => s += d.basePostedDr, 0)
  }

  get totalPostedCrPrice () {
    return this.details.reduce((s, d) => s += d.basePostedCr, 0)
  }

  async getDetails () {
    const details = await chanjetContext.getProjectDetails(this.orgCode, this.bookCode, this.projectCode)
    this.details = details.filter((detail: any) => !!detail.acctgTransCode)
  }
}
</script>
