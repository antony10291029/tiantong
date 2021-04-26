<template>
  <AsyncLoader :handler="getPushers" #default="{ isPending }">
    <template v-if="!isPending">
      <HttpPusherForm
        v-for="(pusher, index) in pushers" :key="pusher.id"
        :pusher="pusher"
      >
        <template #footer>
          <div class="is-flex" style="padding: 0.75rem 0">
            <div style="width: 100px"></div>

            <div class="field">
              <div class="control">
                <AsyncButton
                  :disabled="!isPusherChanged(index)"
                  class="button is-info is-small"
                  style="margin-right: 0.5rem"
                  :handler="() => handleSave(stateId, pusher)"
                >保存</AsyncButton>

                <AsyncButton
                  class="button is-danger is-light is-small"
                  :handler="() => handleDelete(stateId, pusher.id)"
                >删除</AsyncButton>
              </div>
            </div>
          </div>

          <div class="has-border-bottom" style="margin: 1.25rem -1.25rem"></div>
        </template>
      </HttpPusherForm>
    </template>

    <HttpPusherCreate
      v-if="isCreateShow"
      :stateId="stateId"
      @created="handleCreated"
      @close="isCreateShow = false"
    />

    <div style="margin: -1.25rem">
      <a
        @click="isCreateShow = true"
        class="button is-white has-text-link"
        style="border: none; border-radius: 0; width: 100%;"
      >
        添加推送
      </a>
    </div>

  </AsyncLoader>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useConfirm } from "@midos/vue-ui";
import cloneDeep from "lodash/cloneDeep";
import isEqual from "lodash/isEqual";
import { HttpPusher } from "../../entities";
import { useIotHttp } from "../../services/iot-http-client";
import HttpPusherForm from "../../components/HttpPusherForm.vue";
import HttpPusherCreate from "./Create.vue";

export default defineComponent({
  name: "PlcStateHttpPosters",

  components: {
    HttpPusherForm,
    HttpPusherCreate
  },

  props: {
    stateId: {
      type: Number,
      required: true
    },
  },

  setup(props) {
    const http = useIotHttp();
    const confirm = useConfirm();
    const pushers = ref<HttpPusher[]>([]);
    const sourceData = ref<HttpPusher[]>([]);
    const isCreateShow = ref(false);

    function isPusherChanged(index: number) {
      return !isEqual(sourceData.value[index], pushers.value[index]);
    }

    async function getPushers() {
      const result = await http.post("/plcs/states/http-pushers/all", {
        state_id: props.stateId
      });

      pushers.value = result;
      sourceData.value = cloneDeep(result);
    }

    async function handleSave(state_id: number, pusher: HttpPusher) {
      await http.post("/plcs/states/http-pushers/update", {
        state_id, pusher
      });

      getPushers();
    }

    function handleDelete(state_id: number, pusher_id: number) {
      confirm.open({
        title: "提示",
        content: "删除后将无法恢复",
        handler: async () => {
          await http.post("/plcs/states/http-pushers/delete", {
            state_id, pusher_id
          });

          getPushers();
        }
      });
    }

    function handleCreated () {
      isCreateShow.value = false;
      getPushers();
    }

    return {
      pushers,
      sourceData,
      isCreateShow,
      isPusherChanged,
      getPushers,
      handleSave,
      handleDelete,
      handleCreated
    };
  }
});
</script>
