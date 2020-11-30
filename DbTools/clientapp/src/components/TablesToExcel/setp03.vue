<template>
    <div>
        <h1>產生Excel</h1>

        <button class="btn btn-primary mb-3 m-3" @click.prevent="exportFile">產生Excel</button>

        <button class="btn btn-primary mb-3 m-3" @click.prevent="exportInsert">產生Insert語法</button>

        <button class="btn btn-primary mb-3 m-3" @click.prevent="exportEntityClass">產生EntityClass語法</button>

        <ChangTabsButton :canNextPage='canNextPage' @previous="previous" @next="next"></ChangTabsButton>
    </div>
</template>

<script>
    import ChangTabsButton from '../Share/ChangTabsButton'
    import axios from 'axios'
    var  moment  =  require('moment');
    export default {
        props: {
            stepData: Object,
        },
        name: 'setp03',
        data:function(){
            return{
                form:{
                },
            }
        },
        components:{
            'ChangTabsButton':ChangTabsButton,
        },
        computed: {
            canNextPage(){
                return {checked:false,message:"已經是最後一頁了"}
            },
        },
        methods:{
            exportFile() {
                let stepData = this.stepData
                //let form = this.form
                console.log(stepData)
                this.$emit("loading")
                axios.post('/api/ExportFile', stepData, { responseType: 'blob' })
                .then((response) => {
                    //form.tables = response.data
                    let blob = response.data
                    let reader = new FileReader()
                    reader.readAsDataURL(blob)
                    reader.onload = (e) => {
                        downlogFile(stepData, e.target, 'xlsx')
                    }

                })
                .catch(function (error) {
                    console.log(error)
                    alert(error)
                }).finally(()=>{
                    console.log(this)
                    this.$emit("loaded")
                });
            },
            exportInsert() {
                let stepData = this.stepData
                //let form = this.form
                console.log(stepData)
                this.$emit("loading")
                axios.post('/api/ExportInsert', stepData, { responseType: 'blob' })
                .then((response)=> {
                    //form.tables = response.data
                    let blob = response.data
                    let reader = new FileReader()
                    reader.readAsDataURL(blob)
                    reader.onload = (e) => {
                        downlogFile(stepData, e.target, 'txt')
                    }

                })
                .catch(function (error) {
                    console.log(error)
                    alert(error)
                }).finally(() => {
                    console.log(this)
                    this.$emit("loaded")
                });
            },
            exportEntityClass() {
                let stepData = this.stepData
                //let form = this.form
                console.log(stepData)
                this.$emit("loading")
                axios.post('/api/ExportEntityClass', stepData, { responseType: 'blob' })
                .then((response) => {
                    //form.tables = response.data
                    let blob = response.data
                    let reader = new FileReader()
                    reader.readAsDataURL(blob)
                    reader.onload = (e) => {
                        downlogFile(stepData, e.target, 'txt')
                    }
                })
                .catch((error)=> {
                    console.log(error)
                    alert(error)
                }).finally(() => {
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

    var downlogFile = function (stepData, target, fileType) {
        console.log(stepData, fileType)
        let a = document.createElement('a')
        a.download = stepData.dbName + '-' + moment().format('YYYYMMDDHHmmss') + '.' + fileType //filename
        a.href = target.result
        document.body.appendChild(a)
        a.click()
        document.body.removeChild(a)
    }
</script>

<style scoped>

</style>
