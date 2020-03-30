<template>
  <div ref="panel" class="virtual-panel" @scroll="OnScroll">
    <div ref="scroller" class="virtual-scroller" :style="{'height':height+'px'}">
      <VirtualScrollItem ref="scrollItem" v-for="index in 15" :key="index" :minHeight="84" />
    </div>
  </div>
</template>

<script>
import TweetSelector from '../TweetSelector.vue'
import VirtualScrollItem from './VirtualScrollItem.vue'
import AwaitLock from 'await-lock';
export default {
  name: "virtualpanel",
  components:{
    TweetSelector,
    VirtualScrollItem,
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
      lock: new AwaitLock(),
      height:0,
      moveY:0,
      prevScrollTop:0,
      listData:[],
      listPool:[],
      isResized:false,
      startIndex:0,//시작 index
      endIndex:14,//렌더링 30개 할 거니까 endINdex는 30시작
// #region 주석
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
      //리사이즈 정보는 트윗에 저장하고 렌더링 할 때 리사이즈 됐다고 하면 사이즈를 재계산
      //오브젝트 풀 변동은 맨아래있는애 사이즈만큼 이동하면? 
// #endregion
		}
  },
  created:function(){
    for(var i=0;i<2000;i++){
      var obj=new Object();
      obj.text=i.toString();
      obj.index=i;
      obj.size=undefined;
      this.listData.push(obj);
    }
    this.height=this.listData.length*this.minHeight;
   
  },
  watch:{
    // listData:{
    //   handler: function(newVal, oldVal){//스트리밍으로 인해 트윗이 추가 되면 index를 올려줘야함
    //     console.log(oldVal)
    //     console.log(newVal)
    //   },
    //   deep:true,
    // }
  },
  mounted:function(){
    this.$refs.scrollItem.forEach((item)=>{
      this.listPool.push(item);
    })
    window.addEventListener('resize', (e)=>{//윈도우 리사이즈 체크
      this.width=this.$refs.panel.clientWidth;
      this.height=this.$refs.panel.clientHeight;
    })
    this.SetData();
    this.$nextTick(()=>{
      //여기서 최초 transform을 할당 해준다
      //그 이후는 size가 변함에 따라 사이즈 계산해주는 함수 쪽에서 transform을 결정한다
      var y=0;
      for(var i=0,j=this.startIndex;j<=this.endIndex;i++,j++){
        this.listPool[i].transform=y;
        y+=this.listData[j].size;
      }
    });
  },
  computed:{
    scrollSize(){
      return this.$refs.panel.clientHeight;
    }
  },
  methods:{
    OnScroll(e){//pop: 마지막 빼기 / shift: 첫번째 빼기
      this.Scroll();
    },
    async Scroll(){
      await this.lock.acquireAsync();
      var moved=this.$refs.panel.scrollTop - this.prevScrollTop;//스크롤 위, 아래 구하기 위한 식
      this.prevScrollTop=this.$refs.panel.scrollTop;
      this.moveY+=moved;//이동 좌표
      if(moved < 0){
        this.ScrollUp();
      }
      else{
        this.ScrollDown();
      }
      this.lock.release();
    },
    IndexUp(){
      if(this.endIndex + 1 < this.listData.length && (this.endIndex - this.startIndex) < 15){//pool개수만큼 차이나야 함
        this.endIndex++;
      }
      else{//더 이상 스왑하면 안 되는 상태
        return false;
      }
      if(this.startIndex + 1 < this.listData.length){
        this.startIndex++;
      }
      else{//더 이상 스왑하면 안 되는 상태
        return false;
      }
      return true;
    },
    IndexDown(){
      if((this.endIndex - this.startIndex) < 15){//pool개수만큼 차이나야 함!!!!
        this.endIndex=14;
      }
      else{//더 이상 스왑하면 안 되는 상태
        return false;
      }
      if(this.startIndex - 1 > -1 ){
        this.startIndex--;
      }
      else{//더 이상 스왑하면 안 되는 상태
        return false;
      }
      return true;
    },
    ScrollDown(){
      var size = this.listData[this.startIndex].size;
      while(this.moveY>size){
        if(!this.IndexUp()){//스왑 하면 안 되는 상태 체크
          this.moveY=0;//movey 초기화, 그래야 스왑 안 할 때와 꼬이지 않는다
          break;
        }
        var swap = this.listPool.shift();//맨앞 걸 뺌
        var next = this.listPool[0];
        var last = this.listPool[this.listPool.length - 1];
        swap.transform=last.transform + last.GetSize();
        this.listPool.push(swap);//맨뒤에 추가
        swap.tweet=this.listData[this.endIndex];
        this.moveY-=size;
        size = next.GetSize();//맨 위에서 2번째 애 사이즈만큼 이동 시 index가 올라야 함
      }
      console.log(this.moveY)
    },
    ScrollUp(){//맨끝 아이템이 0번으로 감
      var size = this.listData[this.endIndex].size;
      while(-this.moveY > size){
        if(!this.IndexDown()){ //스왑 하면 안 되는 상태 체크
          this.moveY=0;
          break;
        }
        var swap = this.listPool.pop();//맨 뒤에 걸 뺌
        var next = this.listPool[this.listPool.length - 1];//맨뒤에서 두번째 애
        var first = this.listPool[0];
        swap.tweet=this.listData[this.startIndex];
        swap.transform=first.transform - swap.tweet.size;
        this.listPool.splice(0,0,swap);//맨앞에 추가
        this.moveY+=size;
        size = next.tweet.size;//맨 위에서 2번째 애 사이즈만큼 이동 시 index가 올라야 함
        // console.log('swap')
      }
    },
    async ChangeSize(size){
      //여기서 스크롤이 다량으로 이동 했을 경우 transform을 재계산 해야 된다
      //스크롤을 왕창 이동 했을 경우 min-height로 일단 다 더한 상태기 때문에...
      /*
ㅇ
ㅇ//transform계산이 이 위치에 있는 경우. 이런 건 어떻게 해야 되냐.... 
ㅇ


ㅁ스크린영역이 이렇지만
ㅁ
ㅁ
ㅁ
      */
      await this.lock.acquireAsync();
      this.height += size - this.minHeight;
      this.Render();
      this.lock.release();
    },
    Render(){
      if(this.listPool[0].transform<this.prevScrollTop){
        console.log('render~!');
        var y=this.prevScrollTop;
        for(var i=0,j=this.startIndex;j<=this.endIndex;i++,j++){
          this.listPool[i].transform=y;
          y+=this.listData[j].size;
        }
      }
    },
    SetData(){
      for(var i=0,j=this.startIndex;j<=this.endIndex;i++,j++){
        this.listPool[i].tweet=this.listData[j];
      }
    },
  },

};
</script>
<style lang="scss" scoped>
.virtual-panel{
  overflow-y: auto;
  height: 100%;
  width: 100%;
  margin-bottom: 44px;
  position: relative;
  .virtual-scroller{
    overflow: visible;
    // height: 100%;
  }
}
</style>