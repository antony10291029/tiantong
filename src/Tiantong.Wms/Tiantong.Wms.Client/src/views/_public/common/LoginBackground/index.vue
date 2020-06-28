<template>
  <div
    id="login-particles"
    ref="login-particles"
    class="login-background has-background-info column"
  >
  </div>
</template>

<script>
import Vue from 'vue'
import config from './config.json'
import './particle'

export default {
  name: 'LoginBackground',

  dom: null,

  mounted () {
    this.dom = window.particlesJS('login-particles', config)
  },
  beforeDestroy () {
    this.dom.pJS.fn.vendors.destroypJS()
  }
}
</script>

<style>
.login-particles {
  position: absolute;
  width: 100%;
  height: 100%;
  background-color: #b61924;
  background-repeat: no-repeat;
  background-size: cover;
  background-position: 50% 50%;
}

.login-background {
  height: 100vh;
  -webkit-box-shadow: inset 24px 4px 64px -24px rgba(71,71,71,1);
  -moz-box-shadow: inset 24px 4px 64px -24px rgba(71,71,71,1);
  box-shadow: inset 24px 4px 64px -24px rgba(71,71,71,1);
  padding: 0px;
}

@media (max-width: 769px) {
  .login-background{
    display: none
  }
}
</style>
