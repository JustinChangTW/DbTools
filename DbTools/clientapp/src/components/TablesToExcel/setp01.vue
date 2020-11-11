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
        <div class="btn btn-dark previous" disabled @click="previous()">上一頁</div>
        <div class="btn btn-success next" @click="next()">下一頁</div>
        
    </div>
</template>

<script>
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
        methods:{
            connectionTest:function(){
                this.form.check = true;
            },
            previous:function(){
                this.$emit('previous', this.form);
            },
            next:function(){
                if(!this.form.check){
                    alert("尚未連線測試成功")
                }else{
                    this.$emit('next',  this.form);
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