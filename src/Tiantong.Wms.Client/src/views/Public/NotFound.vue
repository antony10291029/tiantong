<template>
	<div class="is-flex is-centered is-vcentered" style="height: 100vh">
		<div
      class="content"
      style="min-width: 400px; max-width: 600px; margin: 1rem"
    >
			<h2>页面不存在</h2>
			<h6>很抱歉，您所访问的页面不存在，或由于某种原因暂时无法访问</h6>
			<a
        @click="$router.go(-1)"
        class="button is-info is-outlined"
      >
        返回
      </a>
		</div>
	</div>
</template>

<script>
export default {
  name: 'Unauthorization'
}
</script>
