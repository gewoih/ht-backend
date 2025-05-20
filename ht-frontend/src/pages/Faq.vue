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
          <p v-html="faq.a"></p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

const faqs = [
  {
    q: 'Что такое TrackMe и кому он подходит?',
    a: 'TrackMe — это веб-сервис для простого и наглядного отслеживания ваших ежедневных привычек. Подходит всем, кто хочет улучшить свою жизнь через осознанные изменения и понимать, какие привычки влияют на самочувствие.',
  },
  {
    q: 'Как начать пользоваться TrackMe?',
    a: '1. Зарегистрируйтесь через электронную почту.<br/>2. Выберите до 10 привычек или оформите подписку для неограниченного списка.<br/>3. Ежедневно отмечайте выполнение привычек и оценивайте своё самочувствие.',
  },
  {
    q: 'Как TrackMe анализирует влияние привычек?',
    a: 'Сервис применяет алгоритмы машинного обучения для поиска корреляций между вашими привычками и оценками здоровья, энергии и настроения. Вы получаете графики, зоны риска и персональные инсайты.',
  },
  {
    q: 'В чём разница между бесплатным и платным аккаунтом?',
    a: '<strong>Бесплатный:</strong> до 10 привычек и базовая аналитика.<br/><strong>Платный:</strong> неограниченное число привычек, расширенные метрики и приоритетная поддержка.',
  },
  {
    q: 'Где найти мобильное приложение TrackMe?',
    a: 'На данный момент доступна только адаптированная веб-версия для мобильных устройств. Работы над нативными приложениями уже ведутся.',
  },
  {
    q: 'Как TrackMe обеспечивает конфиденциальность данных?',
    a: 'Все данные шифруются и хранятся на защищённых серверах. Мы не передаём информацию третьим лицам, а доступ к аккаунту контролируете только вы.',
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
  padding: 3rem 1rem;
}
.faq-page h1 {
  text-align: center;
  margin-bottom: 2.5rem;
  color: #2c3e50;
  font-size: 2rem;
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
