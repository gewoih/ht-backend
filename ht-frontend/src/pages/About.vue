<template>
  <div class="about-page">
    <section class="about-hero-modern">
      <div class="about-hero-content">
        <h1>
          <i class="pi pi-star-fill hero-icon" /> TrackMe — твой помощник в формировании привычек
        </h1>
        <p class="about-lead">
          TrackMe — это современный трекер привычек, который помогает легко внедрять полезные
          привычки, отслеживать прогресс и видеть реальное влияние на твою жизнь.
        </p>
        <ul class="about-features">
          <li><i class="pi pi-check-circle feature-icon" /> Удобный журнал привычек и аналитика</li>
          <li><i class="pi pi-chart-line feature-icon" /> Визуализация прогресса и мотивация</li>
          <li><i class="pi pi-user-plus feature-icon" /> Сообщество единомышленников</li>
          <li><i class="pi pi-lock feature-icon" /> Приватность и безопасность данных</li>
        </ul>
        <div class="about-cta">
          <router-link to="/register" class="p-button p-button-lg p-button-success cta-btn">
            Начать бесплатно
          </router-link>
        </div>
      </div>
      <div class="about-hero-illustration">
        <img src="../assets/about-illustration.svg" alt="TrackMe иллюстрация" />
      </div>
    </section>

    <section class="about-project-info">
      <h2>Что такое TrackMe?</h2>
      <p>
        TrackMe — это не просто приложение, а твой личный инструмент для осознанного саморазвития.
        Мы верим, что большие перемены начинаются с маленьких шагов. С помощью TrackMe ты сможешь:
      </p>
      <ul class="project-benefits">
        <li>Создавать и отслеживать любые привычки: от спорта до чтения</li>
        <li>Анализировать влияние привычек на самочувствие и настроение</li>
        <li>Получать персональные рекомендации и напоминания</li>
        <li>Делиться успехами с друзьями и получать поддержку</li>
      </ul>
    </section>

    <section class="about-values-modern">
      <h2>Наши ценности</h2>
      <div class="values-list">
        <div class="value-item">
          <i class="pi pi-heart value-icon" />
          <span>Забота о себе и осознанность</span>
        </div>
        <div class="value-item">
          <i class="pi pi-users value-icon" />
          <span>Поддержка и вдохновение</span>
        </div>
        <div class="value-item">
          <i class="pi pi-lock value-icon" />
          <span>Приватность и безопасность</span>
        </div>
        <div class="value-item">
          <i class="pi pi-bolt value-icon" />
          <span>Современные технологии и простота</span>
        </div>
      </div>
    </section>

    <section class="about-team">
      <h2>О команде</h2>
      <p>
        TrackMe создан небольшой командой энтузиастов, которые сами увлекаются темой саморазвития,
        психологии и технологий. Мы постоянно улучшаем продукт и открыты к вашим идеям!
      </p>
      <div class="about-feedback">
        <span>Есть вопросы или предложения?</span>
        <router-link to="/contact" class="p-button p-button-text feedback-link">
          Связаться с нами
        </router-link>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import 'primeicons/primeicons.css'
</script>

<style scoped>
.about-page {
  max-width: 900px;
  margin: 0 auto;
  padding: 2.5rem 1rem 4rem 1rem;
  display: flex;
  flex-direction: column;
  gap: 2.5rem;
}
.about-hero-modern {
  display: flex;
  gap: 2.5rem;
  align-items: center;
  flex-wrap: wrap;
  background: linear-gradient(90deg, #e0f7fa 0%, #f8fafc 100%);
  border-radius: 18px;
  padding: 2.5rem 2rem 2rem 2rem;
  box-shadow: 0 2px 12px rgba(44, 62, 80, 0.06);
}
.about-hero-content {
  flex: 2;
  min-width: 260px;
}
.about-hero-illustration {
  flex: 1;
  min-width: 220px;
  display: flex;
  justify-content: center;
}
.about-hero-illustration img {
  max-width: 260px;
  width: 100%;
  height: auto;
}
.hero-icon {
  color: #4caf50;
  font-size: 2.1rem;
  margin-right: 0.5rem;
}
.about-lead {
  font-size: 1.18rem;
  color: #1976d2;
  margin: 1.1rem 0 1.5rem 0;
  font-weight: 600;
}
.about-features {
  list-style: none;
  padding: 0;
  margin: 0 0 1.5rem 0;
  display: flex;
  flex-wrap: wrap;
  gap: 1.1rem 2.2rem;
}
.about-features li {
  font-size: 1.05rem;
  color: #333;
  display: flex;
  align-items: center;
  gap: 0.6rem;
}
.feature-icon {
  color: #22c55e;
  font-size: 1.2rem;
}
.about-cta {
  margin-top: 1.5rem;
}
.cta-btn {
  font-size: 1.1rem;
  padding: 0.7rem 2.2rem;
  border-radius: 8px;
  font-weight: 600;
  background: linear-gradient(90deg, #43e97b 0%, #38f9d7 100%);
  color: #fff !important;
  box-shadow: 0 2px 8px rgba(34, 197, 94, 0.13);
  border: none;
  transition: background 0.2s, box-shadow 0.2s, color 0.2s;
}
.cta-btn:hover,
.cta-btn:focus {
  background: linear-gradient(90deg, #38f9d7 0%, #43e97b 100%);
  color: #fff !important;
  box-shadow: 0 4px 16px rgba(34, 197, 94, 0.18);
  text-decoration: none;
  outline: none;
}
.about-project-info {
  background: #fff;
  border-radius: 14px;
  box-shadow: 0 1px 6px rgba(44, 62, 80, 0.04);
  padding: 2rem 1.5rem 1.5rem 1.5rem;
}
.about-project-info h2 {
  color: #1976d2;
  margin-bottom: 1.1rem;
}
.project-benefits {
  list-style: disc inside;
  margin: 1.1rem 0 0 0.5rem;
  color: #333;
  font-size: 1.05rem;
  padding-left: 1.2rem;
}
.about-values-modern {
  background: #f8f9fa;
  border-radius: 14px;
  padding: 2rem 1.5rem 1.5rem 1.5rem;
  box-shadow: 0 1px 6px rgba(44, 62, 80, 0.04);
}
.about-values-modern h2 {
  color: #1976d2;
  margin-bottom: 1.1rem;
  text-align: center;
}
.values-list {
  display: flex;
  flex-wrap: wrap;
  gap: 1.5rem 2.5rem;
  justify-content: center;
  margin-top: 1.2rem;
}
.value-item {
  display: flex;
  align-items: center;
  gap: 0.7rem;
  font-size: 1.08rem;
  color: #2c3e50;
  background: #fff;
  border-radius: 8px;
  padding: 0.7rem 1.2rem;
  box-shadow: 0 1px 4px rgba(44, 62, 80, 0.03);
}
.value-icon {
  font-size: 1.3rem;
  color: #4caf50;
}
.about-team {
  background: #fff;
  border-radius: 14px;
  box-shadow: 0 1px 6px rgba(44, 62, 80, 0.04);
  padding: 2rem 1.5rem 1.5rem 1.5rem;
  text-align: center;
}
.about-team h2 {
  color: #1976d2;
  margin-bottom: 1.1rem;
}
.about-feedback {
  margin-top: 1.2rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.7rem;
}
.feedback-link {
  font-size: 1rem;
  color: #1976d2;
  text-decoration: underline;
  margin-top: 0.2rem;
}
@media (max-width: 900px) {
  .about-hero-modern {
    flex-direction: column;
    gap: 2rem;
    text-align: center;
    padding: 2rem 0.7rem 1.5rem 0.7rem;
  }
  .about-hero-content {
    flex: unset;
  }
  .about-hero-illustration {
    justify-content: center;
  }
  .about-page {
    padding: 1.2rem 0.2rem 2.5rem 0.2rem;
  }
}
</style>
