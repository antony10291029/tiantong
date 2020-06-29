<template>
  <div
    class="is-flex is-vcentered"
    style="padding: 0.7rem 0"
  >
    <label class="label mb-0" style="width: 80px">
      姓名
    </label>

    <div
      class="is-flex-auto control"
      v-class:is-loading="isPending"
    >
      <input
        v-model="input"
        type="text" class="input is-flex-auto"
        @focus="isFocused = true"
        @blur="isFocused = false"
        v-style:cursor="isFocused ? 'auto' : 'pointer'"
      >
    </div>

    <div style="width: 3rem"></div>

    <div style="width: 3rem">
      <a
        v-if="isChanged && !isPending"
        @click="handleSave"
      >修改</a>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import account from '@/providers/contexts/account'

@Component({
  name: 'Profile'
})
export default class extends Vue {
  @Prop({ required: true })
  name!: string

  @Prop({ required: true })
  refresh: any

  isFocused = false

  input = ''

  isPending = false

  get isChanged () {
    return this.name !== this.input.trim()
  }

  async handleSave () {
    try {
      this.isPending = true
      await account.updateProfile(this.input)
      await this.refresh()
    } finally {
      this.isPending = false
    }
  }

  @Watch('name', { immediate: true })
  handleNameChange () {
    this.input = this.name
  }
}
</script>
