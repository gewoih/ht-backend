<template>
  <div class="about-page">
    <div class="about-hero">
      <div class="about-text">
        <h1>О проекте Habit Tracker</h1>
        <p class="about-mission">
          Наша миссия — помочь каждому человеку стать лучше, осознаннее и счастливее, формируя
          полезные привычки и отслеживая их влияние на жизнь.
        </p>
        <p>
          Мы верим, что большие перемены начинаются с маленьких шагов. Habit Tracker создан для
          того, чтобы вы могли легко отслеживать свои привычки, анализировать их влияние на
          самочувствие и получать персональные рекомендации.
        </p>
        <p>
          Проект разработан небольшой командой энтузиастов, которые сами увлекаются темой
          саморазвития, психологии и технологий.
        </p>
      </div>
      <div class="about-illustration">
        <img src="../assets/about-illustration.svg" alt="О проекте иллюстрация" />
      </div>
    </div>
    <div class="about-values">
      <h2>Наши ценности</h2>
      <ul>
        <li><span class="icon">🌱</span> Осознанность и забота о себе</li>
        <li><span class="icon">🤝</span> Поддержка и вдохновение</li>
        <li><span class="icon">🔒</span> Приватность и безопасность данных</li>
        <li><span class="icon">💡</span> Современные технологии и простота</li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts"></script>

<style scoped>
.about-page {
  max-width: 900px;
  margin: 0 auto;
  padding: 3rem 1rem 5rem 1rem;
}
.about-hero {
  display: flex;
  gap: 3rem;
  align-items: center;
  flex-wrap: wrap;
  margin-bottom: 3rem;
}
.about-text {
  flex: 2;
}
.about-illustration {
  flex: 1;
  min-width: 220px;
  display: flex;
  justify-content: center;
}
.about-illustration img {
  max-width: 260px;
  width: 100%;
  height: auto;
}
.about-page h1 {
  color: #2c3e50;
  margin-bottom: 1.5rem;
}
.about-mission {
  font-size: 1.2rem;
  color: #4caf50;
  margin-bottom: 1.2rem;
  font-weight: 600;
}
.about-values {
  background: #f8f9fa;
  border-radius: 12px;
  padding: 2rem 1.5rem;
  box-shadow: 0 2px 8px rgba(44, 62, 80, 0.04);
}
.about-values h2 {
  color: #2c3e50;
  margin-bottom: 1.2rem;
  text-align: center;
}
.about-values ul {
  list-style: none;
  padding: 0;
  margin: 0;
  display: flex;
  flex-wrap: wrap;
  gap: 1.5rem;
  justify-content: center;
}
.about-values li {
  font-size: 1.1rem;
  color: #2c3e50;
  display: flex;
  align-items: center;
  gap: 0.7rem;
  background: #fff;
  border-radius: 8px;
  padding: 0.7rem 1.2rem;
  box-shadow: 0 1px 4px rgba(44, 62, 80, 0.03);
}
.icon {
  font-size: 1.3rem;
}
@media (max-width: 900px) {
  .about-hero {
    flex-direction: column;
    gap: 2rem;
    text-align: center;
  }
  .about-text {
    flex: unset;
  }
  .about-illustration {
    justify-content: center;
  }
}
</style>
