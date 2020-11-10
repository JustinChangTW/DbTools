import Vue from 'vue'
import VueRouter from 'vue-router'
import HelloWorld from '../components/HelloWorld.vue'
import TablesToExcel from '../components/TablesToExcel'
Vue.use(VueRouter)
export default new VueRouter({
    routes: [{
            path: '*',
            name: 'HelloWorld',
            component: HelloWorld
        },
        {
            path: '/HelloWorld',
            name: 'HelloWorld',
            component: HelloWorld
        },
        {
            path: '/TablesToExcel',
            name: 'TablesToExcel',
            component: TablesToExcel,
            props: true
        },
    ]
})