<template>
  <AsyncLoader
    class="is-flex"
    :handler="getProjects"
  >
    <aside class="menu-container">
      <div
        class="menu is-unselectable"
        style="min-width: 260px; max-width: 260px"
      >
        <ul class="menu-list">
          <li v-for="project in projects" :key="project.id">
            <router-link :to="`/projects/${project.id}/`">
              <span class="icon">
                <i class="iconfont icon-lifter"></i>
              </span>
              <span>{{project.name}}</span>
            </router-link>
          </li>
          <li>
            <router-link to="/projects/create">
              <span class="icon">
                <i class="iconfont icon-add"></i>
              </span>
              <span>添加项目</span>
            </router-link>
          </li>
        </ul>
      </div>
    </aside>

    <router-view
      :key="$route.params.projectId"
      baseURL="projects"
      @refresh="getProjects"
    ></router-view>
  </AsyncLoader>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import echarts from 'echarts'
import domain from '@/providers/contexts/domain'

@Component({
  name: 'Projects',
})
export default class extends Vue {
  projects: any[] = []

  async getProjects () {
    const response = await domain.post('/projects/search', {})

    this.projects = response.data
  }
}
</script>
