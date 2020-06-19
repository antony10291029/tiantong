<template>
  <div style="padding: 1.25rem; padding-top: 1.25rem">
    <div
      class="tabs is-toggle is-small"
      style="margin-bottom: 0.75rem"
    >
      <ul>
        <router-link
          tag="li"
          :to="`${baseURL}/logs/state-logs`"
        >
          <a>数据读写</a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/logs/state-errors`"
        >
          <a>读写异常</a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/logs/http-pusher-logs`"
        >
          <a>HTTP推送</a>
        </router-link>
        <router-link
          tag="li"
          :to="`${baseURL}/logs/http-pusher-errors`"
        >
          <a>推送异常</a>
        </router-link>
      </ul>
    </div>

    <router-view :plcId="plcId" :baseURL="baseURL"/>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'PlcLogs'
})
export default class extends Vue {
  @Prop({ required: true })
  plcId!: number

  @Prop({ required: true })
  baseURL!: string
}
</script>
