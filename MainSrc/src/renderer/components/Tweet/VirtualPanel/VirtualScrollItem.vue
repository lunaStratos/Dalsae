<template>
  <div v-if="tweet!=undefined" class="virtual-item" :style="{'transform': 'translateY('+transform+'px)', 'height':tweet.size+'px'}">
    <span>{{tweet.text}}</span>
    <span>{{tweet.size}}</span>
  </div>
</template>

<script>
import TweetSelector from '../TweetSelector.vue'
export default {
	name: "virtualitem",
  props: {
    // tweet: undefined,
    option: undefined,
    index:undefined,
    isSelected:{
      type:Boolean,
      default:false,
    },
    isResized:{
      type:Boolean,
      default:false,
    },
    
  },
  watch:{
    tweet: {
      handler:function(newVal, oldVal){//스트리밍으로 인해 트윗이 추가 되면 index를 올려줘야함
        if(this.tweet){
          if(this.tweet.size==undefined){
            this.tweet.size = Math.round(Math.random() * (200 - 84) + 84);
            this.$parent.ChangeSize(this.tweet.size);
          }
        }
      },
      deep:true,
    }
  },
	data:function(){
		return{
      tweet:undefined,
			//이 파일이 동적 사이즈 계산 해주는 애
			transform: 0,
		}
  },
  computed:{

  },
  mounted: function() {//EventBus등록용 함수들

  },
  methods:{
  
  },
  components:{
    TweetSelector,
  },
};
</script>
<style lang="scss" scoped>
.virtual-item{
  position: absolute;
  overflow: hidden;
  height: 84px;
  width: 100%;
  background-color: #e2fbff;
  border-bottom: 1px solid black;
  // display: flex;
  // flex-direction: column;
}
</style>