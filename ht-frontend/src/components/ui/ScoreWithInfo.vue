<script setup lang="ts">
import OverlayPanel from 'primevue/overlaypanel'
import Button from 'primevue/button'
import { ref } from 'vue'

interface Props {
  label: string
  infoText: string
  modelValue: number
  onUpdate: (value: number) => void
  scoreKey: string
}

const props = defineProps<Props>()
const panelRef = ref<InstanceType<typeof OverlayPanel> | null>(null)

const showInfo = (event: Event) => {
  panelRef.value?.toggle(event)
}

const getScoreColor = (value: number) => {
  if (value <= 2) return 'low-score'
  if (value <= 4) return 'medium-score'
  return 'high-score'
}
</script>

<template>
  <div class="score-item">
    <div class="score-header">
      <span class="score-label">{{ label }}</span>
      <i class="pi pi-info-circle info-icon" @click="showInfo($event)" />
    </div>

    <div class="score-buttons">
      <button
        v-for="n in 5"
        :key="`score-${scoreKey}-${n}`"
        class="score-button"
        :class="[modelValue === n ? [getScoreColor(n), 'selected'] : '']"
        @click="onUpdate(n)"
      >
        {{ n }}
      </button>
    </div>

    <OverlayPanel ref="panelRef" class="score-info-panel">
      <div class="score-info" v-html="infoText"></div>
    </OverlayPanel>
  </div>
</template>

<style scoped>
.score-item {
  margin-bottom: 1rem;
  padding: 0.5rem;
  border-radius: 8px;
  border: 1px solid #eee;
  background-color: white;
}

.score-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.75rem;
}

.score-label {
  font-weight: 600;
  color: #333;
}

.info-icon {
  cursor: pointer;
  color: #999;
  font-size: 1rem;
  transition: color 0.2s;
}

.info-icon:hover {
  color: #1976d2;
}

.score-buttons {
  display: flex;
  justify-content: space-between;
  gap: 0.25rem;
}

.score-button {
  flex: 1;
  height: 2.25rem;
  border: 1px solid #ddd;
  background-color: white;
  border-radius: 4px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.15s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #555;
}

.score-button:hover {
  background-color: #f5f5f5;
  border-color: #ccc;
}

.score-button.selected {
  color: white;
  border-color: transparent;
}

.low-score {
  background-color: #ef4444;
}

.medium-score {
  background-color: #eab308;
}

.high-score {
  background-color: #22c55e;
}

.score-info {
  max-width: 300px;
  font-size: 0.9rem;
  line-height: 1.5;
}
</style>
