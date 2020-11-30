<template>
    <div>
        <h1>選擇要匯出的資料表</h1>
        <form class="row g-3">
            <div class="mb-3">
                <label for="filter" class="visually-hidden">過濾資料表</label>
                <input type="text"  class="form-control" id="filter" placeholder="過濾資料表" v-model="filter">
            </div>
        </form>
        <div class="h">

        </div>
        <table class="table table-bordered  table-hover">
            <thead>
                <tr>
                    <th class="text-center"><input type="checkbox" name="all" v-model="form.checkedAll" @click="checkedAll"></th>
                    <th class="text-center">資料庫名稱</th>
                    <th class="text-center">資料表名稱</th>
                    <th class="text-center">資料表描述</th>
                    <th class="text-center">資料表類型</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="table in filterTables" :key="table.id">
                    <td class="text-center"><input type="checkbox" v-model="table.check"> </td>
                    <td>{{table.tableCatalog}}</td>
                    <td>{{table.tableSchema+'.'+table.tableName}}</td>
                    <td>{{table.description}}</td>
                    <td>{{table.tableType}}</td>
                </tr>
            </tbody>
        </table>
        <div class="h-10"></div>
        <ChangTabsButton :canNextPage='canNextPage' @previous="previous" @next="next" ></ChangTabsButton>
    </div>
</template>

<script>
    import ChangTabsButton from '../Share/ChangTabsButton'
    import axios from 'axios'
    export default {
        props: {
            tables: Array,
            stepData: Object,
        },
        name: 'setp02',
        data: function () {
            return {
                filter:"",
                form:{
                    checkedAll: false,
                    tables: this.tables
                },
            }
        },
        components:{
            'ChangTabsButton':ChangTabsButton,
        },
        computed: {
            canNextPage(){
                return {checked:this.form.tables.filter(x=>x.check==true).length>0,message:"尚未選擇Table"}
            },
            filterTables(){
                var filter = this.filter.toUpperCase()
                var tables = this.form.tables
                
                return tables.filter(x=>{
                    console.log(filter)
                    return x.tableName.toUpperCase().indexOf(filter) > -1
                })
            },
        },
        mounted:function(){
            this.getTables()
        },
        methods: {
            checkedAll(){
                this.form.tables.forEach(x=>{
                    x.check = !this.form.checkedAll
                })
            },
            getTables() {
                let stepData = this.stepData
                let form = this.form
                //let emit = this.$emit
                console.log(stepData)
                
                this.$emit("loading")
                axios.post('/api/Table',
                    stepData
                )
                .then(function (response) {
                    form.tables = response.data
                })
                .catch(function (error) {
                    console.log(error)
                    alert(error)
                }).finally(()=>{
                    console.log(this)
                    this.$emit("loaded")
                });
            },
            previous:function(){
                this.$emit('previous', this.form);
            },
            selectTable(){
                
            },
            next:function(){
                this.$emit('next',  this.form);
            }
        },
}
</script>

<style scoped>
    .h-10 {
        height: 10% !important;
    }
    .h-20 {
        height: 20% !important;
    }
    .h-30 {
        height: 30% !important;
    }
</style>
