<template>
  <div class="container" style="max-width: 800px">
    <div style="height: 1rem"></div>
    <div class="field is-horizontal">
      <div class="field-label">
        <label class="label"></label>
      </div>

      <div class="field-body">
        <div class="field">
          <p class="control">
            <!-- <span class="title is-4">创建仓库</span> -->
          <nav class="breadcrumb is-medium has-arrow-separator" aria-label="breadcrumbs">
            <ul>
              <li>
                <router-link to="/warehouses" href="#">全部仓库</router-link>
              </li>
              <li class="is-active">
                <a href="#" aria-current="page">添加仓库</a>
              </li>
            </ul>
          </nav>
          </p>
        </div>
      </div>
    </div>

    <div style="height: 0.5rem"></div>

    <div class="field is-horizontal">
      <div class="field-label is-normal">
        <label class="label">编号</label>
      </div>
      <div class="field-body">
        <div class="field">
          <p class="control">
            <input
              class="input" type="text"
              v-model.lazy="params.number"
            >
          </p>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-label is-normal">
        <label class="label">名称</label>
      </div>
      <div class="field-body">
        <div class="field">
          <p class="control">
            <input
              class="input" type="text"
              v-model.lazy="params.name"
            >
          </p>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-label is-normal">
        <label class="label">地址</label>
      </div>
      <div class="field-body">
        <div class="field">
          <div class="control">
            <textarea
              class="textarea"
              v-model.lazy="params.address"
            ></textarea>
          </div>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-label is-normal">
        <label class="label">备注</label>
      </div>
      <div class="field-body">
        <div class="field">
          <div class="control">
            <textarea
              class="textarea"
              v-model.lazy="params.comment"
            ></textarea>
          </div>
        </div>
      </div>
    </div>

    <div class="field is-horizontal">
      <div class="field-label"></div>
      <div class="field-body">
        <div class="field">
          <div class="control">
            <button
              @click="handleSubmit"
              v-loading="isPending"
              class="button is-info"
            >
              提交
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from '@/providers/axios'

export default {
  name: 'WarehouseCreate',
  data: () => ({
    params: {
      number: '',
      name: '',
      addrss: '',
      comment: '',
    },
    isPending: false
  }),
  methods: {
    async handleSubmit () {
      try {
        this.isPending = true
        var response = await axios.post('/warehouses/create', this.params)
        this.$notify.success('仓库已创建')
        this.$router.push('/warehouses')
      } catch (error) {
        this.$notify.danger('仓库创建失败，请重试')
        throw error
      } finally {
        this.isPending = false
      }
    }
  }
}
</script>
