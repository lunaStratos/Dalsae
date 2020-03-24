<template>
  <div class="virtual-item">

  </div>
</template>

<script>
import TweetList from "./Tweetlist.vue";

export default {
	name: "dynamicpool",
  props: {
  },
	data:function(){
		return{
			//이 파일에 사이즈 계산 할 애들 넣고 사이즈 계산 후 리턴 
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
.virtual-item{
  overflow: hidden;
  height: 100%;
  display: flex;
  flex-direction: column;
}
</style>