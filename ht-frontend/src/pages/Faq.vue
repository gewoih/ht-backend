<template>
  <div class="faq-page">
    <h1>Часто задаваемые вопросы</h1>
    <div class="faq-list">
      <div class="faq-item" v-for="(faq, idx) in faqs" :key="idx">
        <div class="faq-question" @click="toggle(idx)">
          <span>{{ faq.q }}</span>
          <span class="arrow" :class="{ open: openIndex === idx }">&#9660;</span>
        </div>
        <div class="faq-answer" v-if="openIndex === idx">
          {{ faq.a }}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const faqs = [
  {
    q: 'Что такое Habit Tracker?',
    a: 'Это инструмент для отслеживания ваших ежедневных привычек, анализа их влияния на ваше самочувствие и получения персональных рекомендаций.',
  },
  {
    q: 'Как начать пользоваться сервисом?',
    a: 'Зарегистрируйтесь, добавьте привычки, которые хотите отслеживать, и ежедневно отмечайте их выполнение. В конце дня оценивайте своё настроение и самочувствие.',
  },
  {
    q: 'Какие привычки можно отслеживать?',
    a: 'Любые! Вы можете добавить свои собственные привычки или выбрать из предложенных вариантов.',
  },
  {
    q: 'Как работает анализ привычек?',
    a: 'Система использует алгоритмы и нейросети для поиска закономерностей между вашими привычками и состоянием, чтобы давать персональные советы.',
  },
  {
    q: 'Могу ли я просматривать отчёты и статистику?',
    a: 'Да, вы получите наглядные отчёты и графики по своим привычкам и их влиянию на вашу жизнь.',
  },
  {
    q: 'Безопасны ли мои данные?',
    a: 'Ваши данные надёжно защищены и не передаются третьим лицам.',
  },
]

const openIndex = ref<number | null>(null)
const toggle = (idx: number) => {
  openIndex.value = openIndex.value === idx ? null : idx
}
</script>

<style scoped>
.faq-page {
  max-width: 700px;
  margin: 0 auto;
  padding: 3rem 1rem 5rem 1rem;
}
.faq-page h1 {
  text-align: center;
  margin-bottom: 2.5rem;
  color: #2c3e50;
}
.faq-list {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}
.faq-item {
  background: #f8f9fa;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(44, 62, 80, 0.04);
  padding: 1.2rem 1.5rem;
  transition: box-shadow 0.2s;
}
.faq-item:hover {
  box-shadow: 0 4px 16px rgba(44, 62, 80, 0.08);
}
.faq-question {
  font-weight: 600;
  color: #4caf50;
  cursor: pointer;
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 1.1rem;
}
.arrow {
  transition: transform 0.2s;
}
.arrow.open {
  transform: rotate(180deg);
}
.faq-answer {
  margin-top: 1rem;
  color: #2c3e50;
  font-size: 1rem;
  line-height: 1.6;
}
</style>
