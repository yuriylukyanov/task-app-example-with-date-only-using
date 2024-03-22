<script lang="ts" setup>

import { ref, onMounted } from 'vue'
import moment from "moment";
import { useToast } from "primevue/usetoast";
import { useConfirm } from "primevue/useconfirm";

import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext'
import Calendar from 'primevue/calendar';
import ConfirmDialog from 'primevue/confirmdialog';

import type Task from '../types/Task';
import type CreateTaskDTO from '../types/CreateTaskDTO';

const toast = useToast();

const showModalFormForCreating = () => {
  modalFormVisible.value = true;
  isModalFormForCreating.value = true;
  modalTask.value = createDefaultTask();
  modalTaskId.value = "";
}

const showModalFormForEditing = (editingTask: Task) => {
  modalFormVisible.value = true;
  isModalFormForCreating.value = false;
  modalTask.value = { ...editingTask }
  modalTaskId.value = editingTask.id
}

const createDefaultTask: () => CreateTaskDTO = () => ({
  name: "",
  startDate: "",
  finishDate: ""
}) 

const handleMoodalFormSaveButtonClick = async () => {
  modalTask.value.finishDate = moment(modalTask.value.finishDate).format('YYYY-MM-DD')
  modalTask.value.startDate = moment(modalTask.value.startDate).format('YYYY-MM-DD')

  const method = modalTaskId.value? "put" : "post"; 

  const data = 
    modalTaskId.value? { 
      id: modalTaskId.value,
      ...modalTask.value
    }
    : modalTask.value

  const response = await fetch(`${import.meta.env.VITE_BACKEND_API_URL}/task`, {
    method: method,
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(data)
  })
  if(response.ok) {
    const responseTask = await response.json() as Task
    const { name, startDate, finishDate } = responseTask;
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
  fetch(`${import.meta.env.VITE_BACKEND_API_URL}/task`)
  .then(async response => {
    tasks.value = await response.json();
  })
}

const confirm = useConfirm();

const handleDeleteTaskClick = (task: Task) => {
  confirm.require({
        group: 'deleteTaskConfirmation',
        header: `Are you sure want to delete "${task.name}"?`,
        message: 'Please confirm to delete.',
        accept: () => handleAcceptDelete(task)
    });
}

const handleAcceptDelete = async (task: Task) => {
  const response = await fetch(`${import.meta.env.VITE_BACKEND_API_URL}/task/${task.id}`, {
    method: 'delete',
    headers: {
      "Content-Type": "application/json"
    }
  })
  if(response.ok) {
    const { name, startDate, finishDate } = task;
    toast.add({ 
      severity: 'success', 
      summary: `Task "${name}" deleted"`, 
      life: 3000
    })
  }
  loadTasks();
}

const tasks = ref(Array<Task>());
const modalFormVisible = ref(false);
const isModalFormForCreating = ref(true)
const modalTask = ref<CreateTaskDTO>(createDefaultTask())
const modalTaskId = ref<string>();

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
        <Column>
          <template #body="row">
            <Button icon="pi pi-pencil" class="mr-2" severity="secondary" @click="() => showModalFormForEditing(row.data)" />
            <Button icon="pi pi-trash" class="mr-2" severity="secondary" @click="() => handleDeleteTaskClick(row.data)"/>
        </template>
        </Column>
    </DataTable>
    <Dialog 
      v-model:visible="modalFormVisible" 
      modal 
      :header="isModalFormForCreating? 'Create Task' : 'Edit Task'" 
      :style="{ width: '25rem' }"
    >
      <div class="flex align-items-center gap-3 mb-3">
          <label for="name" class="font-semibold w-6rem">Name</label>
          <InputText id="name" v-model="modalTask.name" class="flex-auto w-8rem" autocomplete="off" />
      </div>
      <div class="flex align-items-center gap-3 mb-3">
          <label for="startDate" class="font-semibold w-6rem">Start at</label>
          <Calendar id="startDate" v-model="modalTask.startDate" dateFormat="yy-mm-dd" class="flex-auto w-8rem" />
      </div>
      <div class="flex align-items-center gap-3 mb-3">
          <label for="finishDate" class="font-semibold w-6rem">End at</label>
          <Calendar id="finishDate" v-model="modalTask.finishDate" dateFormat="yy-mm-dd" class="flex-auto w-8rem" />
      </div>
      <div class="flex justify-content-end gap-2">
          <Button type="button" label="Cancel" severity="secondary" @click="modalFormVisible = false"></Button>
          <Button type="button" label="Save" @click="handleMoodalFormSaveButtonClick"></Button>
      </div>
  </Dialog>
  <ConfirmDialog group="deleteTaskConfirmation">
    <template #container="{ message, acceptCallback, rejectCallback }">
        <div class="flex flex-column align-items-center p-5 surface-overlay border-round">
            <span class="font-bold text-2xl block mb-2 mt-4">{{ message.header }}</span>
            <p class="mb-0">{{ message.message }}</p>
            <div class="flex align-items-center gap-2 mt-4">
                <Button label="Yes" @click="acceptCallback"></Button>
                <Button label="No" outlined @click="rejectCallback"></Button>
            </div>
        </div>
    </template>
  </ConfirmDialog>
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
