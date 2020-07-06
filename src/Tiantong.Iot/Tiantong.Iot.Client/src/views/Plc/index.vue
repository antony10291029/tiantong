<template>
  <div class="is-flex-auto is-flex is-flex-column">
    <div class="tabs" style="margin-bottom: 0; flex-shrink: 0">
      <ul>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/dashboard`"
        >
          <a>
            <span class="icon">
              <i class="iconfont icon-dashboard"></i>
            </span>
            <span>控制台</span>
          </a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/debug`"
        >
          <a>
            <span
              class="icon"
              style="padding-top: 0.125rem"
            >
              <i class="iconfont icon-debug"></i>
            </span>
            <span>调试</span>
          </a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/config`"
        >
          <a>
            <span
              class="icon"
              style="padding-top: 0.125rem"
            >
              <i class="iconfont icon-settings"></i>
            </span>
            <span>配置</span>
          </a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/states`"
        >
          <a>
            <span
              class="icon"
              style="padding-top: 0.125rem"
            >
              <i class="iconfont icon-table"></i>
            </span>
            <span>数据点</span>
          </a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/${plcId}/logs`"
        >
          <a>
            <span
              class="icon"
              style="padding-top: 0.125rem"
            >
              <i class="iconfont icon-logs"></i>
            </span>
            <span>日志</span>
          </a>
        </router-link>
      </ul>
    </div>

    <router-view
      class="is-flex-auto"
      style="overflow: auto"
      :plcId="plcId"
      :baseURL="`${baseURL}/${plcId}`"
      v-on="$listeners"
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
