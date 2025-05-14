<template>
  <div class="contact-page">
    <h1>Связаться с нами</h1>
    <div class="contact-content">
      <form class="contact-form" @submit.prevent="submitForm">
        <label>
          Ваше имя
          <input v-model="name" type="text" required placeholder="Имя" />
        </label>
        <label>
          Email для ответа
          <input v-model="email" type="email" required placeholder="you@email.com" />
        </label>
        <label>
          Сообщение
          <textarea
            v-model="message"
            required
            placeholder="Ваш вопрос или предложение..."
            rows="5"
          ></textarea>
        </label>
        <button type="submit">Отправить</button>
        <div v-if="sent" class="contact-success">Спасибо! Ваше сообщение отправлено.</div>
      </form>
      <div class="contact-info">
        <h2>Поддержка</h2>
        <p>Если у вас есть вопросы, предложения или нужна помощь — напишите нам!</p>
        <ul>
          <li>
            <b>Email:</b> <a href="mailto:support@habittracker.ru">support@habittracker.ru</a>
          </li>
          <li><b>Telegram:</b> <a href="#">@habittracker_support</a></li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
const name = ref('')
const email = ref('')
const message = ref('')
const sent = ref(false)
const submitForm = () => {
  sent.value = true
  setTimeout(() => {
    sent.value = false
    name.value = ''
    email.value = ''
    message.value = ''
  }, 3000)
}
</script>

<style scoped>
.contact-page {
  max-width: 700px;
  margin: 0 auto;
  padding: 3rem 1rem 5rem 1rem;
}
.contact-page h1 {
  text-align: center;
  margin-bottom: 2.5rem;
  color: #2c3e50;
}
.contact-content {
  display: flex;
  gap: 3rem;
  flex-wrap: wrap;
  align-items: flex-start;
  justify-content: center;
}
.contact-form {
  background: #f8f9fa;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(44, 62, 80, 0.04);
  padding: 2rem 1.5rem;
  min-width: 300px;
  flex: 1 1 320px;
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}
.contact-form label {
  font-weight: 500;
  color: #2c3e50;
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}
.contact-form input,
.contact-form textarea {
  border: 1px solid #cfd8dc;
  border-radius: 6px;
  padding: 0.6rem 0.8rem;
  font-size: 1rem;
  margin-top: 0.2rem;
  background: #fff;
  color: #2c3e50;
  transition: border 0.2s;
}
.contact-form input:focus,
.contact-form textarea:focus {
  border: 1.5px solid #4caf50;
  outline: none;
}
.contact-form button {
  background: #4caf50;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 0.8rem 1.5rem;
  font-size: 1.1rem;
  font-weight: 600;
  cursor: pointer;
  margin-top: 0.5rem;
  transition: background 0.2s;
}
.contact-form button:hover {
  background: #388e3c;
}
.contact-success {
  color: #4caf50;
  margin-top: 1rem;
  font-weight: 600;
}
.contact-info {
  min-width: 220px;
  flex: 1 1 220px;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(44, 62, 80, 0.04);
  padding: 2rem 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
.contact-info h2 {
  color: #2c3e50;
  margin-bottom: 0.7rem;
}
.contact-info ul {
  list-style: none;
  padding: 0;
  margin: 0;
  color: #2c3e50;
  font-size: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
.contact-info a {
  color: #4caf50;
  text-decoration: none;
}
.contact-info a:hover {
  text-decoration: underline;
}
@media (max-width: 900px) {
  .contact-content {
    flex-direction: column;
    gap: 2rem;
    align-items: stretch;
  }
}
</style>
