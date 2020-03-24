<template>
  <div class="virtual-item">

  </div>
</template>

<script>
import TweetList from "./Tweetlist.vue";

export default {
	name: "virtualitem",
  props: {
  },
	data:function(){
		return{
			//이 파일이 동적 사이즈 계산 해주는 애
			//
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