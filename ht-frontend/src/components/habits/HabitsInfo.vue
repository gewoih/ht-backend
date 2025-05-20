<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import Toast from 'primevue/toast'
import HabitDetails from './HabitDetails.vue'
import { useUserStore } from '../../stores/userStore'
import { useHabitStore } from '../../stores/habitStore'
import { Habit } from '../../types/habit'

const props = defineProps({
  visible: {
    type: Boolean,
    default: false,
  },
})

const emit = defineEmits(['update:visible', 'habitsUpdated'])

const userStore = useUserStore()
const habitStore = useHabitStore()
const toastRef = ref()
const error = ref('')

const selectedHabits = ref<Set<string>>(new Set())
const saving = ref(false)
const saveSuccess = ref(false)
const saveError = ref('')

const detailsPopup = ref<{ habit: Habit } | null>(null)

const loadDataOnce = ref(false)

const allHabitIds = computed(() => habitStore.allHabits.map((h) => h.id))
const allSelected = computed(
  () =>
    allHabitIds.value.length > 0 && allHabitIds.value.every((id) => selectedHabits.value.has(id))
)

onMounted(async () => {
  if (!userStore.user) {
    await userStore.loadUserProfile()
  }
})

watch(
  () => props.visible,
  async (isVisible) => {
    if (isVisible && !loadDataOnce.value) {
      await loadHabitsData()
      loadDataOnce.value = true
    }
  }
)

async function loadHabitsData() {
  error.value = ''

  try {
    await habitStore.loadAllHabitsData()
    selectedHabits.value = new Set(habitStore.userHabitIds)
  } catch (e: any) {
    error.value = e.message || 'Неизвестная ошибка'
  }
}

function isDisabled(habitId: string): boolean {
  // No more limits, always return false
  return false
}

function toggleHabit(id: string) {
  if (selectedHabits.value.has(id)) {
    selectedHabits.value.delete(id)
  } else {
    selectedHabits.value.add(id)
  }
}

function toggleSelectAll() {
  if (allSelected.value) {
    selectedHabits.value.clear()
  } else {
    selectedHabits.value = new Set(allHabitIds.value)
  }
}

async function saveHabits() {
  saving.value = true
  saveSuccess.value = false
  saveError.value = ''
  try {
    const habitIds = Array.from(selectedHabits.value)
    const success = await habitStore.saveUserHabitSelection(habitIds)

    if (success) {
      saveSuccess.value = true

      // Create selected habits array to pass to parent
      const selectedHabitsData = habitStore.allHabits.filter((habit) =>
        selectedHabits.value.has(habit.id)
      )

      // Notify parent that habits were updated with complete habit data
      emit('habitsUpdated', selectedHabitsData)

      // Close dialog immediately after successful save
      closeDialog()
    } else {
      saveError.value = habitStore.errorMessage || 'Ошибка при сохранении'
    }
  } catch (e: any) {
    saveError.value = e.message || 'Неизвестная ошибка'
  } finally {
    saving.value = false
  }
}

function closeDialog() {
  emit('update:visible', false)
}

const showDetails = (habit: Habit) => {
  detailsPopup.value = { habit }
}
</script>

<template>
  <Dialog
    v-model:visible="props.visible"
    :modal="true"
    :closable="true"
    header="Выбрать привычки"
    :style="{ width: '90%', maxWidth: '700px' }"
    @update:visible="closeDialog"
  >
    <div class="habits-info-container">
      <Toast ref="toastRef" position="bottom-right" />
      <div class="actions-bar">
        <Button
          :label="allSelected ? 'Снять выделение' : 'Выбрать все'"
          icon="pi pi-check-square"
          class="select-all-btn"
          @click="toggleSelectAll"
          text
        />
      </div>
      <div class="save-feedback">
        <span v-if="saveSuccess" class="save-success">Сохранено!</span>
        <span v-if="saveError" class="save-error">Ошибка: {{ saveError }}</span>
      </div>

      <div v-if="habitStore.isLoading" class="loading">Загрузка...</div>
      <div v-else-if="error || habitStore.hasError" class="error">
        Ошибка: {{ error || habitStore.errorMessage }}
      </div>
      <div v-else class="habits-content">
        <div class="habits-list">
          <div
            v-for="(habits, category) in habitStore.groupedHabits"
            :key="category"
            class="habit-category-group"
          >
            <div class="habit-category-label">{{ category }}</div>
            <ul class="habit-list">
              <li
                v-for="habit in habits"
                :key="habit.id"
                class="habit-list-item"
                @click="toggleHabit(habit.id)"
                :class="{ selected: selectedHabits.has(habit.id) }"
              >
                <div class="habit-item-content">
                  <input
                    type="checkbox"
                    :checked="selectedHabits.has(habit.id)"
                    class="habit-checkbox"
                    readonly
                  />
                  <span class="habit-list-label">{{ habit.name }}</span>
                </div>
                <i class="pi pi-info-circle habit-info-icon" @click.stop="showDetails(habit)"></i>
              </li>
            </ul>
          </div>
        </div>
      </div>

      <div class="dialog-footer">
        <Button label="Отмена" icon="pi pi-times" text @click="closeDialog" />
        <Button
          label="Сохранить"
          icon="pi pi-save"
          severity="primary"
          @click="saveHabits"
          :loading="saving"
          :disabled="saving"
        />
      </div>
    </div>

    <Dialog
      v-if="detailsPopup"
      :visible="!!detailsPopup"
      :modal="true"
      header="Информация о привычке"
      :closable="true"
      @update:visible="detailsPopup = null"
      :style="{ width: '90%', maxWidth: '500px' }"
    >
      <HabitDetails :habitId="detailsPopup.habit.id" :habitName="detailsPopup.habit.name" />
    </Dialog>
  </Dialog>
</template>

<style scoped>
.habits-info-container {
  max-width: 100%;
  margin: 0;
  padding: 0.5rem;
  position: relative;
}

.habits-header {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 1rem;
  gap: 12px;
}

.habits-header h2 {
  text-align: center;
  font-size: 1.5rem;
  font-weight: 700;
  color: #1976d2;
  margin: 0;
}

.subscription-info {
  margin-bottom: 1.5rem;
  display: flex;
  justify-content: center;
}

.limit-indicator {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  justify-content: center;
  gap: 0.5rem;
  background-color: #f0f7ff;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  max-width: 500px;
  width: 100%;
}

.selected-count {
  font-weight: 600;
  color: #1976d2;
  background-color: rgba(25, 118, 210, 0.1);
  padding: 0.25rem 0.75rem;
  border-radius: 12px;
}

.selected-count.limit-reached {
  color: #ef4444;
  background-color: rgba(239, 68, 68, 0.1);
}

.limit-warning {
  font-size: 0.9rem;
  color: #ef4444;
  display: flex;
  align-items: center;
}

.upgrade-btn {
  font-weight: 600;
  color: #1976d2;
  text-decoration: underline;
  padding: 0.25rem;
}

.save-feedback {
  display: flex;
  justify-content: center;
  height: 1.5rem;
  margin-bottom: 1rem;
}

.save-success,
.save-error {
  padding: 0.25rem 1rem;
  border-radius: 2rem;
  font-size: 0.9rem;
  animation: fadeIn 0.3s;
}

.save-success {
  background-color: rgba(34, 197, 94, 0.1);
  color: #22c55e;
}

.save-error {
  background-color: rgba(239, 68, 68, 0.1);
  color: #ef4444;
}

.habits-content {
  position: relative;
  max-height: 50vh;
  overflow-y: auto;
}

.habit-category-group {
  margin-bottom: 2rem;
}

.habit-category-label {
  font-weight: 700;
  font-size: 1.15rem;
  margin-bottom: 0.7rem;
  color: #1976d2;
  letter-spacing: 0.02em;
}

.habit-list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.habit-list-item {
  padding: 0.7rem 1rem;
  margin-bottom: 0.3rem;
  border-radius: 8px;
  background: #f8f9fa;
  transition: background 0.2s, box-shadow 0.2s;
  font-size: 1rem;
  font-weight: 500;
  color: #222;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.03);
  display: flex;
  align-items: center;
  justify-content: space-between;
  cursor: pointer;
}

.habit-list-item.disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.habit-list-item:not(.disabled):hover {
  background: #e3eefd;
  color: #1976d2;
  box-shadow: 0 2px 8px rgba(25, 118, 210, 0.08);
}

.habit-item-content {
  display: flex;
  align-items: center;
  flex: 1;
}

.habit-checkbox {
  cursor: pointer;
}

.habit-checkbox:disabled {
  cursor: not-allowed;
}

.habit-list-label {
  margin-left: 0.7rem;
  flex-grow: 1;
}

.habit-info-icon {
  color: #1976d2;
  font-size: 1.1rem;
  cursor: pointer;
  margin-left: 0.5rem;
  opacity: 0.7;
  transition: opacity 0.2s, transform 0.2s;
}

.habit-info-icon:hover {
  opacity: 1;
  transform: scale(1.1);
}

.upgrade-dialog-content {
  padding: 1rem 0;
  text-align: center;
}

.benefits-list {
  text-align: left;
  margin: 1.5rem 0;
  padding-left: 1rem;
  list-style: none;
}

.benefits-list li {
  margin-bottom: 0.75rem;
  display: flex;
  align-items: center;
}

.benefits-list li i {
  color: #22c55e;
  margin-right: 0.5rem;
}

.pricing {
  margin-top: 1.5rem;
}

.price {
  font-size: 1.5rem;
  font-weight: 700;
  color: #1976d2;
}

.billing-note {
  font-size: 0.8rem;
  color: #6c757d;
  margin-top: 0.3rem;
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
  margin-top: 1.5rem;
  padding-top: 1rem;
  border-top: 1px solid #eee;
}

@media (max-width: 768px) {
  .habits-header {
    flex-wrap: wrap;
  }

  .limit-indicator {
    flex-direction: column;
    align-items: center;
  }
}

.actions-bar {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 1rem;
}
.select-all-btn {
  font-weight: 600;
  color: var(--primary-color);
}
.habit-list-item.selected {
  background: #e3eefd;
  color: #1976d2;
  box-shadow: 0 2px 8px rgba(25, 118, 210, 0.08);
}
</style>
