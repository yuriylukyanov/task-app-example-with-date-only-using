<script lang="ts" setup>

import { ref, onMounted } from 'vue'
import moment from "moment";
import { useToast } from "primevue/usetoast";
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext'
import Calendar from 'primevue/calendar';

import type Task from '../types/Task';
import type CreateTaskDTO from '../types/CreateTaskDTO';

const toast = useToast();

const showModalFormForCreating = () => {
  modalFormVisible.value = true;
  isModalFormForCreating.value = true;
}

const showModalFormForEditing = () => {
  modalFormVisible.value = true;
  isModalFormForCreating.value = false;
}

const createDefaultTask: () => CreateTaskDTO = () => ({
  name: "",
  startDate: "",
  finishDate: ""
}) 

const handleMoodalFormSaveButtonClick = async () => {
  newTask.value.finishDate = moment(newTask.value.finishDate).format('YYYY-MM-DD')
  newTask.value.startDate = moment(newTask.value.startDate).format('YYYY-MM-DD')
  const response = await fetch(`${import.meta.env.VITE_BACKEND_API_URL}/task/create`, {
    method: "post",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(newTask.value)
  })
  if(response.ok) {
    const createdTask = await response.json() as Task
    const { name, startDate, finishDate } = createdTask;
    toast.add({ 
      severity: 'success', 
      summary: name, 
      detail: `${startDate} - ${finishDate}`, 
      life: 3000
    })
    modalFormVisible.value = false;
  }
  loadTasks();
}

const loadTasks = () => {
  fetch(`${import.meta.env.VITE_BACKEND_API_URL}/task/get`)
  .then(async response => {
    tasks.value = await response.json();
  })
}

const tasks = ref(Array<Task>());
const modalFormVisible = ref(false);
const isModalFormForCreating = ref(true)
const newTask = ref(createDefaultTask())

onMounted(() => {
  loadTasks();
})
</script>

<template>
  <div class="greetings">
    <Toast />
    <Button @click="showModalFormForCreating" label="Add"></Button>
    <DataTable :value="tasks" tableStyle="min-width: 50rem">
        <Column field="name" header="Name"></Column>
        <Column field="startDate" header="Start at"></Column>
        <Column field="finishDate" header="End at"></Column>
        <Column field="createDateTime" header="Created at"></Column>
    </DataTable>
    <Dialog 
      v-model:visible="modalFormVisible" 
      modal 
      :header="isModalFormForCreating? 'Create Task' : 'Edit Task'" 
      :style="{ width: '25rem' }"
    >
      <div class="flex align-items-center gap-3 mb-3">
          <label for="name" class="font-semibold w-6rem">Name</label>
          <InputText id="name" v-model="newTask.name" class="flex-auto  w-8rem" autocomplete="off" />
      </div>
      <div class="flex align-items-center gap-3 mb-3">
          <label for="startDate" class="font-semibold w-6rem">Start at</label>
          <Calendar id="startDate" v-model="newTask.startDate" dateFormat="yy-mm-dd" class="flex-auto w-8rem" />
      </div>
      <div class="flex align-items-center gap-3 mb-3">
          <label for="finishDate" class="font-semibold w-6rem">End at</label>
          <Calendar id="finishDate" v-model="newTask.finishDate" dateFormat="yy-mm-dd" class="flex-auto w-8rem" />
      </div>
      <div class="flex justify-content-end gap-2">
          <Button type="button" label="Cancel" severity="secondary" @click="modalFormVisible = false"></Button>
          <Button type="button" label="Save" @click="handleMoodalFormSaveButtonClick"></Button>
      </div>
  </Dialog>
  </div>
</template>

<style scoped>
h1 {
  font-weight: 500;
  font-size: 2.6rem;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.green {
  margin-top: 20px;
}
button {
  margin-top: 20px;

}
</style>
