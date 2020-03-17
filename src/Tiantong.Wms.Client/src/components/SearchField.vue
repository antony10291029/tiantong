<template>
  <div class="field has-addons" style="width: 200px">
    <div class="control">
      <input
        v-model.lazy="value"
        type="text" class="input"
        :placeholder="placeholder"
        @keypress.enter="handleSearch"
      >
    </div>
    <div class="control">
      <a
        v-loading="isPending"
        @click="handleSearch"
        class="button is-info"
      >
        <span class="icon">
          <i class="iconfont icon-search"></i>
        </span>
      </a>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'SearchFeidl',
  model: {
    prop: 'value',
    event: 'input'
  }
})
export default class extends Vue {
  @Prop({ required: true })
  isPending!: boolean

  @Prop({ default: '' })
  placeholder!: string

  value: string = ''

  handleSearch (event: any) {
    this.$emit('search', this.value)
  }
}
</script>
