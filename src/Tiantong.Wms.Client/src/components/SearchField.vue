<template>
  <div class="field has-addons">
    <div class="control" :style="{ width }">
      <input
        v-model.lazy="value"
        type="text" class="input"
        :placeholder="placeholder"
        @keypress.enter="handleEnter"
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

  @Prop({ default: '160px' })
  width!: string

  value: string = ''

  handleEnter (event: any) {
    this.value = event.target.value
    this.handleSearch()
  }

  handleSearch () {
    this.$emit('search', this.value)
  }
}
</script>
