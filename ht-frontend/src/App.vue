<script setup lang="ts">
import AppHeader from './components/layout/AppHeader.vue'
import AppFooter from './components/layout/AppFooter.vue'
</script>

<template>
  <AppHeader />
  <div class="content-wrapper">
    <main class="app-main">
      <router-view />
    </main>
  </div>
  <AppFooter />
</template>

<style>
body,
html,
#app {
  height: 100%;
  min-height: 100vh;
  overflow-y: auto;
  width: 100%;
  display: flex;
  flex-direction: column;
}

body {
  margin: 0;
  padding: 0;
  background-color: #f5f5f5;
}

.content-wrapper {
  width: 100%;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
  display: flex;
  justify-content: center;
}

.app-main {
  width: 100%;
  padding: 0 0 2rem 0;
}

/* Стили для ползунка оценки дня */
input[type='range'] {
  -webkit-appearance: none;
  width: 100%;
  height: 8px;
  border-radius: 4px;
  background: #e2e8f0;
  outline: none;
}

input[type='range']::-webkit-slider-thumb {
  -webkit-appearance: none;
  appearance: none;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  background: var(--primary-color);
  cursor: pointer;
  transition: background 0.3s;
}

input[type='range']::-webkit-slider-thumb:hover {
  background: var(--primary-hover);
}

/* Стили для чекбоксов */
input[type='checkbox'] {
  width: 18px;
  height: 18px;
  border: 2px solid #cbd5e0;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s;
}

input[type='checkbox']:checked {
  background-color: var(--primary-color);
  border-color: var(--primary-color);
}

/* Анимации */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
