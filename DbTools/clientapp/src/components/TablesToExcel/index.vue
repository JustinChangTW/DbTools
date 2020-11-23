<template>

<div class="container-fluid">
    <div class="row">
        <ul class="nav nav-tabs nav-fill">
            <li class="nav-item" v-for="(step,index) in steps" :key="index">
                <a class="nav-link" :class="{active:tabActive==index}" href="#" @click="changTag(index)">{{step}}</a>
            </li>
        </ul>
        <keep-alive>
            <component
            :is="currectStep" 
            :stepData="stepData"
            :tables="stepData.tables||[]"
            @previous="previous($event)" 
            @next="next($event)" 
            @getTables="getTables()"></component>
        </keep-alive>
    </div>
</div>

</template>

<script>
import Step01 from './setp01'
import Step02 from './setp02'
import Step03 from './setp03'
//import axios from 'axios';
export default {
    name: 'TablesToExcel',
    props: {
        msg: String
    },
    components: {
    'Step01': Step01,
    'Step02': Step02,
    'Step03': Step03,
    },
    data:function(){
        return {
            steps:['Step01','Step02','Step03'],
            tabActive:0,
            currectStep:"Step01",
            stepData: {
                tables: [],},
        }
    },
    methods:{
        changTag:function(index){
            this.tabActive=index>=0?index>=this.steps.length?this.steps.length-1:index:0;
            this.currectStep=this.steps[this.tabActive]
        },
        previous:function(data){
            console.log(data)
            this.changTag(--this.tabActive)
            this.stepData = Object.assign(this.stepData,data)
        },
        next:function(data){
            console.log(data)
            this.changTag(++this.tabActive)
            this.stepData = Object.assign(this.stepData,data)
        }
    }
}

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
