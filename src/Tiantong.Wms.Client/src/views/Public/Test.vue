<template>
  <div class="container">
    <div style="height: 3rem"></div>
    <table class="table is-fullwidth is-bordered">
      <thead>
        <th>1</th>
        <th>2</th>
        <th>3</th>
        <th>4</th>
      </thead>

      <TBody>
        <Tr>
          <td rowspan="2">1</td>
          <td>2</td>
          <td>3</td>
          <td>4</td>
        </Tr>
        <Tr>
          <td>2</td>
          <td>3</td>
          <td>4</td>
        </Tr>
      </TBody>
    </table>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

class Test extends Vue {
  value = 100

  params!: string

  get test() {
    return 2000
  }
}

@Component({
  name: 'test'
})
export default class extends Test {
  value = 1000

  handle (value: never) {
    console.log(value)
  }

  created () {
    this.handle(1000 as never)
  }

  mounted () {
    var tbody: any = this.$refs.tbody
  }
}
</script>
