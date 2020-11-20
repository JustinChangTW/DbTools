<template>
    <div>
        <h1>產生Excel</h1>
        
        <button  class="btn btn-primary mb-3" @click.prevent="connectionTest">產生Excel</button>
        
        <ChangTabsButton :canNextPage='canNextPage' @previous="previous" @next="next" ></ChangTabsButton>
    </div>
</template>

<script>
import ChangTabsButton from '../Share/ChangTabsButton'
export default {
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
