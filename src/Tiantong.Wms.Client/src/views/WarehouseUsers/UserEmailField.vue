<template>
  <div class="field">
    <label class="label">邮箱</label>
    <div class="control field has-addons">
      <p class="control is-expanded">
        <input
          :value="inputValue"
          @input="handleInput"
          class="input" type="text"
        >
      </p>
      <p class="control">
        <a class="button is-static">
          @als-yuchuan.com
        </a>
      </p>
    </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component({
  name: 'UserEmailField',
  model: {
    prop: 'value',
    event: 'input'
  }
})
export default class extends Vue {
  @Prop({ required: true })
  value!: string

  get inputValue () {
    return this.value.split('@')[0]
  }

  handleInput (event: any) {
    this.$emit('input', event.target.value + '@als-yuchuan.com')
  }
}
</script>
