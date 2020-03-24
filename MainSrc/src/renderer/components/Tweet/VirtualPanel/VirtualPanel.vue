<template>
  <div class="virtual-panal" v-if="tweets!=undefined">

  </div>
</template>

<script>
import TweetList from "./Tweetlist.vue";

export default {
	name: "virtualpanel",
  props: {
		tweets:undefined,
  },
	data:function(){
		return{
			//보여줄 오브젝트 풀 개수 정해야 함. 이건 뭐... 적당히 height로 유사값 구해서 넣으면 될 거 같다 아니면 고정 하던가
			//오브젝트 풀에 보여줄 트윗 index from ~ index to 를 정해야 함
			//스크롤이 페이지 사이즈 정도 지나면 풀에 트윗 데이터 재할당 하는 식으로?

			//사이즈 측정 할 풀도 있어야 함
			//사이즈 측정은 적당량의 풀을 동적으로 만들고 그 안에 애 집어넣고 거기서 사이즈 구하고 풀에서 제거
			//사이즈 저장은 tweet에다가 watch도 붙여서 할당?
			//패널 자체의 사이즈가 바뀌면 풀에 100개씩 끊어서 넣고 사이즈 계산 해야 되나?
			//2000개 트윗이면 20번 nexttick으로 사이즈 20번 구하기...
		}
  },
  computed:{

  },
  mounted: function() {//EventBus등록용 함수들

  },
  methods:{
  
  },
  components:{
  },
};
</script>
<style lang="scss" scoped>
.virtual-panel{
  overflow: hidden;
  height: 100%;
  display: flex;
  flex-direction: column;
}
</style>