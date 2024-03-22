import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import Button from 'primevue/button';
import Toast from 'primevue/toast';
import ToastService from 'primevue/toastservice';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import ConfirmationService from 'primevue/confirmationservice';

import './assets/main.css'
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';
import "primeflex/primeflex.css";
import "primevue/resources/themes/aura-light-green/theme.css";


const app = createApp(App);
app.use(PrimeVue);
app.use(ToastService);
app.use(ConfirmationService);

app.component('Button', Button);
app.component('Toast', Toast);
app.component('DataTable', DataTable);
app.component('Column', Column);

app.mount('#app');
