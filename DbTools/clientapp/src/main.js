import Vue from 'vue'
import App from './App.vue'
//載入Bootstrap
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap'
Vue.config.productionTip = false

new Vue({
    render: h => h(App),
}).$mount('#app')