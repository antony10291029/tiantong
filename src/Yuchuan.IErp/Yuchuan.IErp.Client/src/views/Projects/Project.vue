<template>
  <div class="is-flex-auto is-flex is-flex-column">
    <div class="has-border-bottom">
      <div class="tabs" style="margin-bottom: 0">
        <ul>
          <router-link :to="`/${baseURL}/${projectId}/dashboard`" tag="li">
            <a>
              <span>实时状态</span>
            </a>
          </router-link>
          <router-link :to="`/${baseURL}/${projectId}/devices`" tag="li">
            <a>
              <span>设备管理</span>
            </a>
          </router-link>
          <router-link :to="`/${baseURL}/${projectId}/detail`" tag="li">
            <a>
              <span>项目信息</span>
            </a>
          </router-link>
        </ul>
      </div>
    </div>

    <router-view
      :projectId="projectId"
      :baseURL="`/${baseURL}/${projectId}`"
      @refresh="$emit('refresh')"
    ></router-view>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'ProjectDetail',
})
export default class extends Vue {
  @Prop({ required: true })
  baseURL!: string

  @Prop({ required: true })
  projectId!: number
}
</script>
