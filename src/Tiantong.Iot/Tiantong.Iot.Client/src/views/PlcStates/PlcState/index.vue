<template>
  <div>
    <div
      class="tabs is-bordered"
      style="margin-bottom: 0"
    >
      <ul>
        <router-link
          tag="li"
          to="detail"
          active-class="is-active"
        >
          <a>基本配置</a>
        </router-link>
        <router-link
          tag="li"
          to="http-posters"
          active-class="is-active"
        >
          <a>HTTP 推送</a>
        </router-link>
      </ul>
    </div>

    <router-view
      :plcId="$attrs.plcId"
      :stateId="stateId"
      :baseURL="`${baseURL}/states/${stateId}`"
      v-on="$listeners"
    />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'PlcState'
})
export default class extends Vue {
  @Prop({ required: true })
  stateId: any

  @Prop({ required: true })
  baseURL!: string
}
</script>
