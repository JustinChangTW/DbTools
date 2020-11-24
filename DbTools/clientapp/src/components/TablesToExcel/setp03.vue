<template>
    <div>
        <h1>產生Excel</h1>
        
        <button  class="btn btn-primary mb-3" @click.prevent="exportFile">產生Excel</button>
        
        <ChangTabsButton :canNextPage='canNextPage' @previous="previous" @next="next" ></ChangTabsButton>
    </div>
</template>

<script>
    import ChangTabsButton from '../Share/ChangTabsButton'
    import axios from 'axios'
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
                let form = this.form
                console.log(stepData)
                axios.post('/api/ExportFile', stepData, { responseType: 'blob' })
                .then(function (response) {
                    form.tables = response.data
                    let blob = response.data
                    let reader = new FileReader()
                    reader.readAsDataURL(blob)
                    reader.onload = (e) => {
                        let a = document.createElement('a')
                        var today = new Date();
                        today.toISOString().substring(0, 10);
                        a.download = stepData.dbName + today.toISOString().substring(0, 10)+'.xlsx' //filename
                        a.href = e.target.result
                        document.body.appendChild(a)
                        a.click()
                        document.body.removeChild(a)
                    } 

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
