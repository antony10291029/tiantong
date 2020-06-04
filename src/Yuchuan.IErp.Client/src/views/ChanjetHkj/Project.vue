<template>
  <AsyncLoader :handler="getDetails">
    {{projectCode}}
    <table class="table is-bordered is-centered is-fullwidth is-nowrap">
      <thead>
        <th style="width: 1px">#</th>
        <th style="width: 1px">日期</th>
        <th>科码</th>
        <th>科目</th>
        <th>借方</th>
        <th>贷方</th>
        <th>摘要</th>
      </thead>
      <tbody>
        <tr v-for="(detail, key) in details" :key="detail.id">
          <td>{{key + 1}}</td>
          <td>
            {{detail.period}}
          </td>
          <td style="text-align: left">
            {{detail.glAccountCode}}
          </td>
          <td>
            {{detail.glAccountLongName}}
          </td>
          <td>
            {{detail.basePostedDr}}
          </td>
          <td>
            {{detail.basePostedCrPrice}}
          </td>
          <td style="text-align: left">
            {{detail.comments}}
          </td>
        </tr>
      </tbody>
    </table>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import chanjetContext from '@/providers/contexts/chanjet'

@Component({
  name: 'ProjectDetail'  
})
export default class extends Vue {
  @Prop({ required: true })
  orgCode!: string

  @Prop({ required: true })
  bookCode!: string

  @Prop({ required: true })
  projectCode!: string

  details = [] as any[]


  async getDetails (code: string) {
    const details = await chanjetContext.getProjectDetails(this.orgCode, this.bookCode, code)
    this.details = details.filter((detail: any) => !!detail.acctgTransCode)
  }

}
</script>
