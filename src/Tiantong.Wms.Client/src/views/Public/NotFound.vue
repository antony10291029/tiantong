<template>
	<div class="is-flex is-centered is-vcentered" style="height: 100vh">
		<div
      class="content"
      style="min-width: 400px; max-width: 600px;"
    >
			<h2 style="margin-bottom: 1.25rem">
        页面不存在
      </h2>
			<h6 style="margin-bottom: 1.25rem">
        很抱歉，您所访问的页面不存在，或由于某种原因暂时无法访问
      </h6>
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
