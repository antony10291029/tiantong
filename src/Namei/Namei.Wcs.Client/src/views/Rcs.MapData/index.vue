<template>
  <AsyncLoader :handler="getMapData">
    <div class="box" style="margin: 1.25rem">
      <SearchField
        @search="handleSearch"
        :value="params.areaCode"
      />

      <table class="table is-fullwidth is-bordered is-nowrap">
        <thead>
          <th style="width: 1px">#</th>
          <th>区域</th>
          <th>地图编码</th>
          <th>库位</th>
          <th>托盘号</th>
          <th>优先级</th>
          <th style="width: 1px"></th>
        </thead>
        <tbody>
          <DataMap
            tag="tr"
            :dataMap="mapData"
            v-slot="{ value, index }"
          >
            <td>{{index + 1}}</td>
            <td>{{value.areaCode}}</td>
            <td>{{value.mapDataCode}}</td>
            <td>{{value.dataName}}</td>
            <td>{{value.podCode}}</td>
            <td>{{value.wcsAreaSeq}}</td>
            <MapDataDetail
              :value="value"
              @refresh="getMapData"
            />
          </DataMap>
        </tbody>
      </table>
    </div>
  </AsyncLoader>
</template>

<script lang="ts">
import { DataMap } from "@midos/seed-work";
import { defineComponent, ref } from "vue";
import { useWcsHttp } from "../../services/wcs-http";
import SearchField from "../../components/SearchField.vue";
import MapDataDetail from "./MapDataDetail.vue";

export default defineComponent({
  components: {
    SearchField,
    MapDataDetail
  },

  setup() {
    const http = useWcsHttp();
    const params = ref({ areaCode: "201" });
    const mapData = ref(new DataMap());

    async function getMapData() {
      mapData.value = await http.post("/rcs/mapData/search", params.value);
    }

    function handleSearch(text: string) {
      params.value.areaCode = text;

      return getMapData();
    }

    return {
      mapData,
      params,
      getMapData,
      handleSearch,
    };
  }
});
</script>
