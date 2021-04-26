<template>
  <div>
    <div class="box">
      <div class="is-flex">
        <SearchField @search="handleSearch"/>

        <div class="is-flex-auto"></div>

        <CreateTask
          :taskType="taskType"
          @created="getOrders"
        />
      </div>

      <div style="height: 1.25rem"></div>

      <table class="table is-hoverable is-vcentered is-fullwidth">
        <thead>
          <th>Id</th>
          <th v-for="(value, key) in taskType.data" :key="key">
            {{key}}
          </th>
          <th>状态</th>
          <th>创建日期</th>
          <th>结束日期</th>
          <th></th>
        </thead>
        <tbody v-if="orders.keys.length">
          <DataMapIterator
            tag="tr"
            :dataMap="orders"
            v-slot="{ entity: order }"
          >
            <td>
              {{order.id}}
            </td>
            <td v-for="(value, key) in taskType.data" :key="key">
              {{order.data[key]}}
            </td>
            <td>
              <span class="tag is-small">
                {{order.status}}
              </span>
            </td>
            <TimeWrapper :value="order.createdAt" tag="td" />
            <TimeWrapper :value="order.closedAt" tag="td" />
            <td>
              <router-link
                :to="{
                  name: 'TasOrderDetail',
                  params: { orderId: order.id }
                }"
              >
                详情
              </router-link>
            </td>
          </DataMapIterator>
        </tbody>
        <tbody v-else>
          <td class="is-centered" colspan="8">
            无
          </td>
        </tbody>
      </table>

      <div style="height: 1.25rem"></div>

      <Pagination v-bind="orders" @change="getOrders" />
    </div>

    <router-view
      v-if="orderId"
      :taskType="taskType"
      :taskOrder="taskOrder"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, computed } from "vue";
import { useRoute } from "vue-router";
import { Pagination } from "@midos/core";
import { useMidosCenterHttp } from "../../services/midos-center-http";
import { useQuery } from "../../hooks/use-query";
import CreateTask from "./Create.vue";

export default defineComponent({
  name: "TasTaskOrders",

  components: {
    CreateTask
  },

  props: {
    taskType: {
      type: Object,
      required: true,
    }
  },

  setup(props) {
    const route = useRoute();
    const http = useMidosCenterHttp();
    const orders = ref(new Pagination());
    const params = useQuery();
    const orderId = computed(() => +route.params.orderId);
    const taskOrder = computed(() => orders.value.entities[orderId.value]);

    async function getOrders(page = 1) {
      params.value.page = page;
      orders.value = await http.paginate("/midos/tasks/orders/search", {
        ...params.value,
        typeId: props.taskType.id,
      });
    }

    async function handleSearch(query: string) {
      params.value.query = query;
      getOrders();
    }

    return {
      orders,
      getOrders,
      orderId,
      taskOrder,
      handleSearch
    };
  }
});
</script>
