<template>
  <div style="text-align: left">
    <input
      readonly
      type="text" class="input"
      v-style:cursor="'pointer'"
      @click="isShow = !isShow"
      :value="supplier === null ? '' :  supplier.name"
    >
    <AsyncLoader
      v-if="isShow && !readonly"
      class="modal is-active"
      :handler="getSuppliers"
    >
      <div
        @click="handleClose"
        class="modal-background"
      ></div>
      <div class="modal-card">
        <header class="modal-card-head">
          <p class="modal-card-title">
            选择供应商
          </p>
        </header>
        <div class="modal-card-body">
          <Table class="is-hoverable">
            <thead>
              <tr>
                <th style="width: 1px">#</th>
                <th>供应商名</th>
                <th>备注</th>
                <th style="width: 1px">
                  <span
                    class="icon"
                    style="color: transparent"
                  >
                    <i class="iconfont icon-success"></i>
                  </span>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="(supplier, index) in suppliers" :key="supplier.id"
                @click="currentSupplier = supplier"
              >
                <td>{{index + 1}}</td>
                <td>{{supplier.name}}</td>
                <td>{{supplier.comment}}</td>
                <td>
                  <span
                    v-if="currentSupplier && currentSupplier.id === supplier.id"
                    class="icon has-text-success"
                  >
                    <i class="iconfont icon-tick"></i>
                  </span>
                </td>
              </tr>
            </tbody>
          </Table>
        </div>
        <footer class="modal-card-foot">
          <a
            class="button is-success"
            @click.stop="handleSelect"
          >选择</a>
          <div class="is-flex-auto"></div>
          <SupplierCreate
            :warehouseId="warehouseId"
            @refresh="getSuppliers"
          />
        </footer>
      </div>
    </AsyncLoader>
  </div>
</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'
import AsyncLoader from '@/components/AsyncLoader.vue'
import { Supplier } from '@/Entities'
import axios from '@/providers/axios'
import Table from '@/components/Table.vue'
import SupplierCreate from './SupplierCreate.vue'

@Component({
  name: 'PurchaseOrderSupplierSelector',
  components: {
    Table,
    AsyncLoader,
    SupplierCreate
  }
})
export default class extends Vue {
  @Prop({ required: true })
  warehouseId!: number

  @Prop({ required: true })
  supplier!: Supplier | null

  @Prop({ default: false })
  readonly!: boolean

  currentSupplier: Supplier | null = null

  isShow: boolean = false

  entities: {
    keys: Array<number>
    data: { [ key: string ]: Supplier }
  } = { keys: [], data: {} }

  get suppliers () {
    return this.entities.keys.map(key => this.entities.data[key])
  }

  async getSuppliers () {
    let response = await axios.post('/suppliers/all', {
      warehouse_id: this.warehouseId
    })

    this.entities = response.data
  }

  handleSelect () {
    this.$emit('select', this.currentSupplier)
    this.handleClose()
  }

  handleClose () {
    this.isShow = false
  }

  created () {
    this.currentSupplier = this.supplier
  }
}
</script>
