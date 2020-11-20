<template>
    <div>
        <h1>資料庫連線設定</h1>
        <form class="row g-3">
            <div class="mb-3">
                <label for="dbType" class="visually-hidden">資料庫類型</label>
                <select class="form-select" aria-label="Default select example"  v-model="form.dbType">
                    <option disabled value=""> 請選擇資料庫類型</option>
                    <option value="MSSQL">MSSQL</option>
                    <option value="MYSQL">MYSQL</option>
                </select>
            </div>
            <div class="mb-3">
                <label for="dbServer" class="visually-hidden">資料庫伺服器</label>
                <input type="text"  class="form-control" id="dbServer" placeholder="資料庫伺服器" v-model="form.dbServer">
            </div>
            <div class="mb-3">
                <label for="dbServer" class="visually-hidden">資料庫名稱</label>
                <input type="text"  class="form-control" id="dbName" placeholder="資料庫名稱" v-model="form.dbName">
            </div>
            <div class="mb-3">
                <label for="user" class="visually-hidden">連線帳號</label>
                <input type="text"  class="form-control" id="user" placeholder="連線帳號" v-model="form.user">
            </div>
            <div class="mb-3">
                <label for="password" class="visually-hidden">密碼</label>
                <input type="password" class="form-control" id="password" placeholder="密碼" v-model="form.password">
            </div>
            <div class="mb-3">
                <button  class="btn btn-primary mb-3" @click.prevent="connectionTest">測試連線</button>
            </div>
        </form>
        <ChangTabsButton :canNextPage='canNextPage' @previous="previous" @next="next" ></ChangTabsButton>
    </div>
</template>

<script>
import ChangTabsButton from '../Share/ChangTabsButton'
import axios from 'axios';
export default {
        name: 'setp01',
        data:function(){
            return{
                form:{
                    dbType:'',
                    dbServer:'',
                    dbName:'',
                    user:'',
                    password:'',
                    check:false
                }
            }
        },
        computed: {
            canNextPage(){
                return {checked:this.form.check,message:"尚未連線"}
            }
        },
        components:{
            'ChangTabsButton':ChangTabsButton,
        },
        methods:{
            connectionTest:function(){
                var form = this.form
                form.check = false
                axios.post('/api/DbConnectionTest', 
                    this.form
                )
                .then(function (response) {
                    form.check = response.data
                    if(!form.check )  {
                        alert("連線失敗，請確認參數，謝謝")
                    }
                })
                .catch(function (error) {
                    console.log(error)
                    alert(error)
                });
            },
            previous(){
                this.$emit('previous',this.form)
            },
            next(){
                this.$emit('next',this.form)
            }
        },
}
</script>

<style scoped>

</style>