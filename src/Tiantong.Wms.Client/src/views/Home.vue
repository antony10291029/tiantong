<template>
  <nav class="navbar has-border-bottom" style="height: 64px">
    <div class="navbar-brand">
      <span
        class="navbar-item"
        style="margin-left: 0.5rem"
      >
        <a class="icon">
          <i class="iconfont icon-menu is-size-4"></i>
        </a>
      </span>
      <span class="navbar-item is-size-4 is-unselectable has-text-link is-hoverable">
        天瞳WMS
      </span>

      <a role="button" class="navbar-burger burger">
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
      </a>
    </div>

    <div id="navbarBasicExample" class="navbar-menu">
      <div class="navbar-start">
        <a class="navbar-item">
          Home
        </a>

        <a class="navbar-item">
          Documentation
        </a>

        <div class="navbar-item has-dropdown is-hoverable">
          <a class="navbar-link">
            More
          </a>

          <div class="navbar-dropdown">
            <a class="navbar-item">
              About
            </a>
            <a class="navbar-item">
              Jobs
            </a>
            <a class="navbar-item">
              Contact
            </a>
            <hr class="navbar-divider">
            <a class="navbar-item">
              Report an issue
            </a>
          </div>
        </div>
      </div>

      <div class="navbar-end">
        <div class="navbar-item">
          <div class="buttons">
            <a class="button is-info">
              <strong>Sign up</strong>
            </a>
            <a class="button is-light">
              Log in
            </a>
          </div>
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
export default {

}
</script>
