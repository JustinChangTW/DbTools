<template>
    <div>
        <h1>選擇要匯出的資料表</h1>
        
        <table class="table table-bordered  table-hover">
            <thead>
                <tr>
                    <th class="text-center"><input type="checkbox" name="all" v-model="form.checkedAll" @click="checkedAll"></th>
                    <th class="text-center">表格名稱</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="table in form.tables" :key="table.id">
                    <td class="text-center"><input type="checkbox" v-model="table.checked"> </td>
                    <td>{{table.tableName}}</td>
                </tr>
            </tbody>
        </table>
        <ChangTabsButton :canNextPage='canNextPage' @previous="previous" @next="next" ></ChangTabsButton>
    </div>
</template>

<script>
import ChangTabsButton from '../Share/ChangTabsButton'
export default {
        name: 'setp02',
        data:function(){
            return{
                form:{
                    checkedAll:false,
                    tables:[]
                },
            }
        },
        components:{
            'ChangTabsButton':ChangTabsButton,
        },
        computed: {
            canNextPage(){
                return {checked:this.form.tables.filter(x=>x.checked==true).length>0,message:"尚未選擇Table"}
            },
        },
        mounted:function(){
            this.form.tables.push({id:1,tableName:"abc",checked:false})
            this.form.tables.push({id:2,tableName:"def",checked:false})
            this.form.tables.push({id:3,tableName:"ght",checked:false})
        },
        methods:{
            checkedAll(){
                this.form.tables.forEach(x=>{
                    x.checked = !this.form.checkedAll
                })
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
