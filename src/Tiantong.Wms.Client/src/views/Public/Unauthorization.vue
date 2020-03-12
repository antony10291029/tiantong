<template>
	<div class="is-flex is-centered is-vcentered" style="height: 100vh">
		<div
      class="content"
      style="min-width: 400px; max-width: 600px; margin: 1rem"
    >
			<h2>身份已失效，请重新登陆</h2>
			<h6>您的用户身份已失效，可能是由于太久未登陆，或身份信息发生了变更。请可以尝试重新登陆，若无法解决则联系您的管理人员和技术人员。</h6>
			<router-link
        to="/login"
        class="button is-info is-outlined"
      >
        重新登陆
      </router-link>
		</div>
	</div>
</template>

<script>
export default {
  name: 'Unauthorization'
}
</script>
