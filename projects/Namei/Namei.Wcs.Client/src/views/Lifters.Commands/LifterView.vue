<template>
  <div>
    <div class="box">
      <p class="title is-size-5">
        指令面板
      </p>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          目标楼层
        </div>

        <div class="field has-addons">
          <div
            v-for="floor in floors" :key="floor"
            class="control"
          >
            <a
              class="button"
              @click="setDestination(floor)"
              v-class:is-info="floor === destination"
            >
              {{floor}} 楼
            </a>
          </div>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          放货完成
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(floor, 'imported')"
            class="button is-info"
            v-for="floor in floors" :key="floor"
          >
            {{floor}}F - 放货完成
          </a>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          扫码完成
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(floor, 'scanned')"
            class="button is-info"
            v-for="floor in floors" :key="floor"
          >
            {{floor}}F - 扫码完成
          </a>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          请求取货
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(floor, 'exported')"
            class="button is-info"
            v-for="floor in floors" :key="floor"
          >
            {{floor}}F - 请求取货
          </a>
        </div>
      </div>

      <hr>

      <div
        class="is-flex is-vcentered"
        style="padding: 1.25rem 0"
      >
        <div
          class="label"
          style="width: 100px"
        >
          取货完成
        </div>

        <div class="buttons">
          <a
            @click="publishMessage(floor, 'taken')"
            class="button is-info"
            v-for="floor in floors" :key="floor"
          >
            {{floor}}F - 取货完成
          </a>
        </div>
      </div>

      <hr>

    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import { useWcsHttp } from "../../services/wcs-http";

export default defineComponent({
  name: "Lifter",

  props: {
    lifterId: {
      type: String,
      required: true
    }
  },

  setup(props) {
    const http = useWcsHttp();
    const destination = ref("1");

    async function getDestination() {
      const result = await http.post("/test/lifters/destination");

      destination.value = result.destination;
    }

    async function setDestination(value: string) {
      await http.post("/test/lifters/set-destination", {
        destination: value
      });
      await getDestination();
    }

    async function publishMessage(floor: string, message: string) {
      await http.post("/test/lifters/publish-message", {
        lifter_id: props.lifterId,
        floor,
        message,
      });
    }

    onMounted(getDestination);

    return {
      destination,
      getDestination,
      setDestination,
      publishMessage,
      floors: ["1", "2", "3", "4"],
    };
  }
});
</script>
