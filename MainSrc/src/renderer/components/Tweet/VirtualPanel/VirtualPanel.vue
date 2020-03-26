<template>
  <div class="virtual-panal" v-if="tweets!=undefined">
    <div class="virtual-scroller">
    </div>
  </div>
</template>

<script>
import TweetSelector from '../TweetSelector.vue'
export default {
  name: "virtualpanel",
  components:{
    TweetSelector
  },
  props: {
    tweets:undefined,
    minHeight:{
      type:Number,
      default:84,
    },
  },
	data:function(){
		return{
      //VirtualPanel: 가상화 스크롤의 부모
      //VirtualScrollItem: 가상화 스크롤 pool로 사용 될 item 해당 파일에서 Tweet의 size를 구한다

      //min-height를 정해놓고(한줄30, 일반 크게 84, 일반 n)
      //tweets.length가 변경 될 때마다 스크롤 최대치를 +=30 혹은 +=84
      //실제 렌더링 차례가 되면 사이즈를 구해서 84+크기차이+=
      //렌더링 될 때마다 사이즈 구해서 갱신 해야 함. 왜냐 창의 resize도 발생하기 때문
      //창 resize될 때 보이는 애들 갱신 해줘야 함, 이건 flag로 관리 하자. 리사이즈가 발생하지 않았는데 다시 그릴 필요가 없다.
      //list로 해서 스크롤 내려가면 오브젝트 풀 맨아래서 빼고 맨위로 올려주고 해당 index에 해당 하는 트윗 할당 하기
      //오브젝트 풀의 위치는 transform으로 갱신
      //스크롤이 84정도씩 갈 때마다 만들어주기
      //오브젝트 풀의 풀 개수는 (높이/minheight)+6 정도로 해주자
      //스크롤 변경 되는 이벤트는 받아야 한다. 그래야 바꿔주지...
      //이동 되는 건 기존 구현 한 거처럼 scrollToPosition같은 걸 만들어주자
      //ScrollToIndex같은 건 하지 말고 우선 Position만 만들자
      //사이즈는 Map으로 해서 hash, 속도 최대한 높이고 렌더링 될 때마다 id_str을 키로 해서 size갱신 해주자
      //size갱신의 경우 스크롤 이동에 따른 index증가 시 오브젝트 넣고 size뽑아내서 저장
      //tweet.retweeted, tweet.favorited같은 게 있을 경우 size+=n 해주자 
      //TweetSelector를 래핑 하는 걸 하나 더 만들어야되나?
      //트윗 셀렉터는 기능 그대로 두고 래필해서 사이즈 구하는 애 하나 만들자
      //한줄보기, 일반보기 변경 될 때 pool재할당 해주는 로직도 필요하겠다
		}
  },
  computed:{

  },
  methods:{
  
  },

};
</script>
<style lang="scss" scoped>
.virtual-panel{
  overflow-y: auto;
  height: 100%;
  width: 100%;
  .virtual-scroller{
    overflow: visible;
    height: 100%;
  }
}
</style>