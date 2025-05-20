<template>
  <div class="contact-page">
    <div class="contact-card">
      <h1 class="contact-title">Связаться с нами</h1>
      <p class="contact-description">
        Если у вас есть вопросы, предложения или нужна помощь — мы всегда на связи!
      </p>
      <ul class="contact-list">
        <li>
          <i class="pi pi-envelope contact-icon"></i>
          <span>Email:</span>
          <a href="mailto:support@track-me.ru">support@track-me.ru</a>
        </li>
        <li>
          <i class="pi pi-telegram contact-icon"></i>
          <span>Telegram:</span>
          <a href="#">@track-me-support</a>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
// No logic needed for static contact info
</script>

<style scoped>
.contact-page {
  min-height: 60vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: none;
  padding: 3rem 1rem 5rem 1rem;
}

.contact-card {
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 4px 24px rgba(44, 62, 80, 0.08);
  padding: 2.5rem 2rem 2rem 2rem;
  max-width: 600px;
  width: 100%;
  text-align: center;
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.contact-title {
  font-size: 2rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.contact-description {
  color: #4caf50;
  font-size: 1.1rem;
  margin-bottom: 1.5rem;
}

.contact-list {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.contact-list li {
  display: flex;
  align-items: center;
  gap: 0.7rem;
  font-size: 1.08rem;
  color: #2c3e50;
  justify-content: flex-start;
}

.contact-icon {
  font-size: 1.5rem;
  color: #4caf50;
  min-width: 1.5rem;
}

.contact-list a {
  color: #1976d2;
  text-decoration: none;
  font-weight: 500;
  margin-left: 0.3rem;
  transition: color 0.2s;
}

.contact-list a:hover {
  color: #4caf50;
  text-decoration: underline;
}

@media (max-width: 600px) {
  .contact-card {
    padding: 1.5rem 0.7rem 1.5rem 0.7rem;
    max-width: 100%;
  }
  .contact-title {
    font-size: 1.3rem;
  }
  .contact-description {
    font-size: 1rem;
  }
}
</style>
