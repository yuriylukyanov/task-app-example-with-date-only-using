<script lang="ts" setup>
import type CreateTaskDTO from "../../../types/CreateTaskDTO";
import type Task from "../../../types/Task";

import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext'
import Calendar from 'primevue/calendar';

import moment from "moment";

const model = defineModel<Boolean>()

const {
    task,
    taskId
} = defineProps<{
    taskId?: string,
    task: CreateTaskDTO
}>();


const emit = defineEmits<{
  onSaved: [isSuccessfull: Boolean, task: Task]
}>()

const handleModalFormSaveButtonClick = async () => {
  task.finishDate = moment(task.finishDate).format('YYYY-MM-DD')
  task.startDate = moment(task.startDate).format('YYYY-MM-DD')

  const method = taskId? "put" : "post"; 

  const data = 
    taskId? { 
      id: taskId,
      ...task
    }
    : task

  const response = await fetch(`${import.meta.env.VITE_BACKEND_API_URL}/task`, {
    method: method,
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(data)
  })

  const isSuccessfull = response.ok

  if(isSuccessfull) {
    const responseTask = await response.json() as Task
    emit('onSaved', isSuccessfull, responseTask);
    model.value = false;
    return;
  }
  emit('onSaved', isSuccessfull, null);
}
</script>

<template>
    <Dialog 
        v-model:visible="model" 
        modal 
        :header="!taskId? 'Create Task' : 'Edit Task'" 
        :style="{ width: '25rem' }"
    >
        <div class="flex align-items-center gap-3 mb-3">
            <label for="name" class="font-semibold w-6rem">Name</label>
            <InputText id="name" v-model="task.name" class="flex-auto w-8rem" autocomplete="off" />
        </div>
        <div class="flex align-items-center gap-3 mb-3">
            <label for="startDate" class="font-semibold w-6rem">Start at</label>
            <Calendar id="startDate" v-model="task.startDate" dateFormat="yy-mm-dd" class="flex-auto w-8rem" />
        </div>
        <div class="flex align-items-center gap-3 mb-3">
            <label for="finishDate" class="font-semibold w-6rem">End at</label>
            <Calendar id="finishDate" v-model="task.finishDate" dateFormat="yy-mm-dd" class="flex-auto w-8rem" />
        </div>
        <div class="flex justify-content-end gap-2">
            <Button type="button" label="Cancel" severity="secondary" @click="model.value = false"></Button>
            <Button type="button" label="Save" @click="handleModalFormSaveButtonClick"></Button>
        </div>
    </Dialog>
</template>