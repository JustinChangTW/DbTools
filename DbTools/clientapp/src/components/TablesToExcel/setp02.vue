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
        <div class="btn btn-dark previous" disabled @click="previous()">上一頁</div>
        <div class="btn btn-success next" @click="next()">下一頁</div>
    </div>
</template>

<script>
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
            next:function(){
                this.$emit('next',  this.form);
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
