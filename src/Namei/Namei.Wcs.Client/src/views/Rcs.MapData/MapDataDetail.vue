<template>
  <td>
    <a @click="isShow = true">
      编辑
    </a>
    <teleport to="body">
      <div
        v-if="isShow"
        class="modal is-active"
      >
        <div class="modal-background"></div>
        <div class="modal-card" style="width: 480px">
          <header class="modal-card-head">
            <p class="modal-card-title">
              地图数据
            </p>
            <a @click="isShow = false" class="delete"></a>
          </header>
          <section class="modal-card-body">
            <div class="field">
              <label class="label">地图编号</label>
              <div class="control">
                <Input :value="data.mapDataCode" readonly />
              </div>
            </div>

            <div class="field">
              <label class="label">库位</label>
              <div class="control">
                <Input :value="data.dataName" readonly />
              </div>
            </div>

            <div class="field">
              <label class="label">区域</label>
              <div class="control">
                <Input :value="data.areaCode" readonly />
              </div>
            </div>

            <div class="field">
              <label class="label">排序</label>
              <div class="control">
                <Input v-model:value="data.wcsAreaSeq" />
              </div>
            </div>
          </section>
          <footer class="modal-card-foot">
            <AsyncButton
              class="button is-success"
              :handler="handleSubmit"
              :disabled="!isChanged"
            >
              更新
            </AsyncButton>
          </footer>
        </div>
      </div>
    </teleport>
  </td>
</template>

<script lang="ts">
import { defineComponent, Ref, ref, toRefs } from "vue";
import { useWcsHttp } from "../../services/wcs-http";
import { useCopy } from "../../hooks/use-copy";

export default defineComponent({
  props: {
    value: {
      type: Object,
      required: true
    }
  },

  setup(props, { emit }) {
    const http = useWcsHttp();
    const isShow = ref(false);
    const { isChanged, data } = useCopy<any>(toRefs(props).value as Ref<any>);

    async function handleSubmit() {
      await http.post("/rcs/mapData/updateRange", [data.value]);

      isShow.value = false;
      emit("refresh");
    }

    return {
      data,
      isShow,
      isChanged,
      handleSubmit
    };
  }
});
</script>
