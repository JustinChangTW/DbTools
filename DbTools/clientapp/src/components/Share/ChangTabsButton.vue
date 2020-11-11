<template>
    <div>
        <div class="btn btn-dark previous" @click="previous()">上一頁</div>
        <div class="btn btn-success next"  @click="next()">下一頁</div>
    </div>
</template>

<script>
export default {
    name: 'ChangTabsButton',
    props:{
        canNextPage:Object,
    },
    methods:{
        connectionTest:function(){
            this.form.check = true;
        },
        previous:function(){
            this.$emit('previous');
        },
        next:function(){
            if(!this.canNextPage.checked){
                alert(this.canNextPage.message)
            }else{
                this.$emit('next');
            }
        }
    },
}
</script>

<style scoped>
.previous ,.next{
    display: inline-block;
    position: fixed;
    width:50%;
    text-align: center;
    line-height: 30px;
    color:white;
}

.previous{
    bottom: 5px;
    left: 0px;
    border: 1px solid;
}
.next{
    bottom: 5px;
    right: 0px;
    border: 1px solid;
}
</style>
