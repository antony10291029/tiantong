<template>
  <AsyncLoader
    :handler="getStates"
  >
    <div class="tabs" style="margin-bottom: 0">
      <ul>
        <li class="is-active"><a>数据点</a></li>
        <li><a>Music</a></li>
        <li><a>Videos</a></li>
        <li><a>Documents</a></li>
      </ul>
    </div>

    <router-view
      :plcId="plcId"
      style="padding: 1.25rem"
    />
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '../../providers/axios'

@Component({
  name: 'Plc'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

}
</script>
