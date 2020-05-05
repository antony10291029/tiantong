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

}
</script>
