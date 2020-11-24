<template>

<div class="container-fluid">
    <div class="row">
        <ul class="nav nav-tabs nav-fill">
            <li class="nav-item" v-for="(step,index) in steps" :key="index">
                <a class="nav-link" :class="{active:tabActive==index}" href="#" @click="changTag(index)">{{step}}</a>
            </li>
        </ul>
        <component
        :is="currectStep" 
        :stepData="stepData"
        :tables="stepData.tables||[]"
        @previous="previous($event)" 
        @next="next($event)" 
        @getTables="getTables()"
        @loading="loading()"
        @loaded="loaded()"></component>
    </div>
    <div class="load" :class="{hidden:!isload}">
        <svg width="3em" height="3em" viewBox="0 0 16 16" class="loading bi bi-arrow-repeat" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
            <path d="M11.534 7h3.932a.25.25 0 0 1 .192.41l-1.966 2.36a.25.25 0 0 1-.384 0l-1.966-2.36a.25.25 0 0 1 .192-.41zm-11 2h3.932a.25.25 0 0 0 .192-.41L2.692 6.23a.25.25 0 0 0-.384 0L.342 8.59A.25.25 0 0 0 .534 9z"/>
            <path fill-rule="evenodd" d="M8 3c-1.552 0-2.94.707-3.857 1.818a.5.5 0 1 1-.771-.636A6.002 6.002 0 0 1 13.917 7H12.9A5.002 5.002 0 0 0 8 3zM3.1 9a5.002 5.002 0 0 0 8.757 2.182.5.5 0 1 1 .771.636A6.002 6.002 0 0 1 2.083 9H3.1z"/>
        </svg>
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
            isload:false,
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
        },
        loading(){
            this.isload=true
        },
        loaded(){
            this.isload=false
        }
    }
}

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .load{
        position: fixed;
        background-color: #aaaa;
        width: 100%;
        height: 100%;
        top: 0;
        left: 0;
    }
    .hidden{
        display: none;
    }
    .loading{
        position: relative;
        top: 50%;
        left: 50%;
        animation-name:loading;
        animation-iteration-count: infinite;
        animation-duration:1s;
        transform: translate(1.5em,1.5em);
    }
    @keyframes loading{
    from{
        transform: rotate(180deg); 
    }
    to{
        transform: rotate(360deg);  
    }
}
</style>
