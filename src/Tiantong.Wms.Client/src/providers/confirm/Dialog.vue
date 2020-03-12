<template>
  <div v-active="isShow" class="modal">
    <div class="modal-background" @click="handleClose"/>
    <div
      class="modal-card"
      v-style:width="width"
      style="min-width: 320px"
    >
      <header class="modal-card-head">
        <p class="modal-card-title">
          {{title}}
        </p>
      </header>
      <section class="modal-card-body">
        {{content}}
      </section>
      <footer class="modal-card-foot">
        <button
          class="button is-success"
          :class="isLoading && 'is-loading'"
          @click="handleConfirm"
        >确认</button>
        <button
          class="button"
          @click="handleClose"
        >取消</button>
      </footer>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Confirmer',
  data () {
    return {
      width: '',
      isShow: false,
      isLoading: false,

      title: '',
      content: '',
      handler: false,
      beforeClose: false
    }
  },
  methods: {
    // handleEscape () {
    //   console.log(1000)
    //   this.handleClose() 
    // },
    async handleClose () {
      try {
        this.beforeClose && await this.beforeClose()
      } finally {
        this.isShow = false
      }
    },
    async handleConfirm () {
      if (this.handler) {
        try {
          this.isLoading = true
          const result = await this.handler()
          if (result !== false) {
            this.isShow = false
          }
        } finally {
          this.isLoading = false
        }
      } else {
        this.close()
        this.$emit('confirm')
      }
    },
    open ({ title, content, handler, beforeClose, width = 'auto' }) {
      this.title = title
      this.width = width
      this.content = content
      this.handler = handler
      this.beforeClose = beforeClose
      this.isShow = true
    },
    close () {
      this.handleClose()
    },
  }
}
</script>
