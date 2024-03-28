<script lang="ts" setup>

import { ref, onMounted } from 'vue'
import moment from "moment";
import { useToast } from "primevue/usetoast";
import { useConfirm } from "primevue/useconfirm";

import ConfirmDialog from 'primevue/confirmdialog';
import TaskModalForm from './components/TaskModalForm.vue'

import type Task from '../../types/Task';
import type CreateTaskDTO from '../../types/CreateTaskDTO';


const toast = useToast();

const showModalFormForCreating = () => {
  modalFormVisible.value = true;
  modalTask.value = createDefaultTask();
  modalTaskId.value = "";
}

const showModalFormForEditing = (editingTask: Task) => {
  modalFormVisible.value = true;
  modalTask.value = { ...editingTask }
  modalTaskId.value = editingTask.id
}

const createDefaultTask: () => CreateTaskDTO = () => ({
  name: "",
  startDate: "",
  finishDate: ""
}) 

const handleModalFormSaveButtonClick = (isSuccessfull: Boolean, task: Task) => {
  if(isSuccessfull) {
    const { name, startDate, finishDate } = task!;
    toast.add({ 
      severity: 'success', 
      summary: name, 
      detail: `${startDate} - ${finishDate}`, 
      life: 3000
    })
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
    <TaskModalForm
      v-model="modalFormVisible"
      :taskId="modalTaskId"
      :task="modalTask"
      @onSaved="handleModalFormSaveButtonClick"
    />
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
