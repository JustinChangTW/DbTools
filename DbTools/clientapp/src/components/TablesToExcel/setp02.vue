<template>
    <div>
        <h1>選擇要匯出的資料表</h1>
        
        <table class="table table-bordered  table-hover">
            <thead>
                <tr>
                    <th class="text-center"><input type="checkbox" name="all" v-model="form.checkedAll" @click="checkedAll"></th>
                    <th class="text-center">資料庫名稱</th>
                    <th class="text-center">表格名稱</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="table in form.tables" :key="table.id">
                    <td class="text-center"><input type="checkbox" v-model="table.check"> </td>
                    <td>{{table.dbName}}</td>
                    <td>{{table.tableName}}</td>
                </tr>
            </tbody>
        </table>
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
                console.log(stepData)
                axios.post('/api/Table',
                    stepData
                )
                .then(function (response) {
                    //form.check = response.data
                    form.tables = response.data
                })
                .catch(function (error) {
                    console.log(error)
                    alert(error)
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

</style>
