<template>
  <table class="table is-bordered is-fullwidth">
    <thead>
      <tr>
        <th style="width: 1px">#</th>
        <th>规格码</th>
        <th>规格名</th>
        <th>单位</th>
        <th style="width: 1px">
          <a
            class="icon"
            @click="$emit('add')"
          >
            <i class="iconfont icon-plus"></i>
          </a>
        </th>
      </tr>
    </thead>
    <tbody>
      <slot></slot>
    </tbody>
  </table>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'GoodItems'
})
export default class extends Vue {
  id: number = 0
}
</script>
