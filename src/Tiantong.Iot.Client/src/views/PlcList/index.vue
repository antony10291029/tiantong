<template>
  <AsyncLoader
    class="is-flex"
    :handler="getPlcs"
  >
    <aside
      class="menu has-border-right is-unselectable"
      style="min-width: 220px; max-width: 220px; height: 100%; overflow-y: auto"
    >
      <ul class="menu-list">
        <li>
          <router-link
            :to="`/plcs`"
            active-class="none"
            exact-active-class="is-active"
          >
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <i class="iconfont icon-manage"></i>
            </span>
            <span>服务管理</span>
          </router-link>
        </li>
        <li v-for="plc in plcs" :key="plc.id">
          <router-link :to="`/plcs/${plc.id}`">
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <span class="iconfont icon-device"></span>
            </span>
            <span>{{plc.name}}</span>
          </router-link>
        </li>
        <li>
          <router-link :to="`/plcs/create`">
            <span
              class="icon"
              style="margin-right: 0.25rem"
            >
              <i class="iconfont icon-add"></i>
            </span>
            <span>添加设备</span>
          </router-link>
        </li>
      </ul>
    </aside>
    <router-view
      :key="$route.params.plcId"
      baseURL="/plcs"
      class="is-flex-auto"
      style="overflow: auto"
      @refresh="getPlcs"
    />
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import axios from '@/providers/axios'

@Component({
  name: 'PlcList',
  components: {

  }
})
export default class extends Vue {
  plcs: object[] = []

  async getPlcs () {
    let response = await axios.post('/plcs/all')
    this.plcs = response.data
  }
}
</script>
