<template>
  <div>
    <div class="tabs" style="margin-bottom: 0">
      <ul>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/dashboard`"
        >
          <a>运行状态</a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/config`"
        >
          <a>设备信息</a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/states`"
        >
          <a>数据点</a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/state-logs`"
        >
          <a>读写记录</a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/state-errors`"
        >
          <a>读写异常</a>
        </router-link>
      </ul>
    </div>

    <router-view
      :plcId="plcId"
      :baseURL="`${baseURL}/${plcId}`"
    />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'Plc'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  @Prop({ required: true })
  baseURL!: string
}
</script>
