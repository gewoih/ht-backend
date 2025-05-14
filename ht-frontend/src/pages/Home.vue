<template>
  <div class="home">
    <!-- Hero Section -->
    <section class="hero">
      <div class="hero-content">
        <h1>Откройте, как привычки формируют вашу жизнь</h1>
        <p class="hero-subtitle">
          Начните отслеживать свои ежедневные привычки и узнайте, как они влияют на ваше здоровье,
          настроение и продуктивность. Наша умная система поможет вам лучше понять себя — день за
          днем.
        </p>
        <button class="cta-button">Начать сейчас</button>
      </div>
      <div class="hero-image">
        <img src="../assets/hero-image.svg" alt="Иллюстрация отслеживания привычек" />
      </div>
    </section>

    <!-- Interesting Facts Section -->
    <section class="facts">
      <h2>Интересные факты о привычках</h2>
      <div class="facts-grid">
        <div class="fact-card">
          <div class="fact-icon">🧠</div>
          <h3>21 день</h3>
          <p>
            Исследования показывают, что в среднем требуется 21 день для формирования новой привычки
          </p>
        </div>
        <div class="fact-card">
          <div class="fact-icon">⏰</div>
          <h3>40%</h3>
          <p>40% наших ежедневных действий — это привычки, выполняемые на автопилоте</p>
        </div>
        <div class="fact-card">
          <div class="fact-icon">📊</div>
          <h3>66 дней</h3>
          <p>Для полного закрепления привычки может потребоваться до 66 дней</p>
        </div>
      </div>
    </section>

    <!-- How It Works Section -->
    <section class="how-it-works">
      <h2>Как это работает</h2>
      <div class="steps">
        <div class="step">
          <div class="step-number">1</div>
          <h3>Отслеживайте свой день</h3>
          <p>
            Каждый вечер отмечайте, что вы сделали в течение дня — например, приняли холодный душ,
            провели время в соцсетях или съели молочные продукты. Это займет всего пару минут.
          </p>
        </div>
        <div class="step">
          <div class="step-number">2</div>
          <h3>Оцените свое состояние</h3>
          <p>
            Оцените, как вы себя чувствовали: ваше самочувствие, концентрация и общее настроение.
            Это поможет выявить закономерности между привычками и вашим состоянием.
          </p>
        </div>
        <div class="step">
          <div class="step-number">3</div>
          <h3>Получайте инсайты</h3>
          <p>
            Наши умные алгоритмы анализируют ваши данные, чтобы найти закономерности. Вы получите
            персональные рекомендации о том, какие привычки помогают вам чувствовать себя лучше.
          </p>
        </div>
      </div>
    </section>

    <!-- Benefits Section -->
    <section class="benefits">
      <h2>Преимущества отслеживания привычек</h2>
      <div class="benefits-content">
        <div class="benefit-text">
          <p>Отслеживание привычек помогает:</p>
          <ul>
            <li>Повысить осознанность своих действий</li>
            <li>Выявить негативные паттерны поведения</li>
            <li>Улучшить самодисциплину</li>
            <li>Достигать поставленных целей</li>
            <li>Повысить качество жизни</li>
          </ul>
        </div>
        <div class="benefit-image">
          <img src="../assets/benefits.svg" alt="Преимущества отслеживания привычек" />
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features">
      <h2>Почему выбирают нас</h2>
      <div class="feature-grid">
        <div class="feature">
          <div class="feature-icon">✅</div>
          <h3>Простое ежедневное отслеживание</h3>
          <p>Интуитивно понятный интерфейс для быстрого заполнения</p>
        </div>
        <div class="feature">
          <div class="feature-icon">📊</div>
          <h3>Отслеживайте любые привычки</h3>
          <p>Настраивайте и отслеживайте любые привычки, которые важны для вас</p>
        </div>
        <div class="feature">
          <div class="feature-icon">📱</div>
          <h3>Быстрая система оценки</h3>
          <p>Оценивайте свой день всего несколькими нажатиями</p>
        </div>
        <div class="feature">
          <div class="feature-icon">📈</div>
          <h3>Умный анализ</h3>
          <p>Визуальные отчеты и интеллектуальный анализ привычек</p>
        </div>
        <div class="feature">
          <div class="feature-icon">💡</div>
          <h3>Персональные советы</h3>
          <p>Получайте индивидуальные рекомендации для улучшения образа жизни</p>
        </div>
        <div class="feature">
          <div class="feature-icon">🎯</div>
          <h3>Отслеживание целей</h3>
          <p>Ставьте и отслеживайте свои личные цели</p>
        </div>
      </div>
    </section>

    <!-- User Reviews Section -->
    <section class="reviews">
      <h2>Отзывы пользователей</h2>
      <div class="reviews-list">
        <div class="review-card">
          <img
            class="review-avatar"
            src="https://randomuser.me/api/portraits/women/44.jpg"
            alt="Анна"
          />
          <div class="review-content">
            <div class="review-name">Анна, 28 лет</div>
            <div class="review-text">
              С помощью Habit Tracker я стала ложиться спать раньше и чувствую себя бодрее! Очень
              удобно видеть прогресс и получать советы.
            </div>
          </div>
        </div>
        <div class="review-card">
          <img
            class="review-avatar"
            src="https://randomuser.me/api/portraits/men/32.jpg"
            alt="Игорь"
          />
          <div class="review-content">
            <div class="review-name">Игорь, 35 лет</div>
            <div class="review-text">
              Теперь я не забываю про важные привычки и стал более организованным. Особенно нравится
              анализ и визуализация!
            </div>
          </div>
        </div>
        <div class="review-card">
          <img
            class="review-avatar"
            src="https://randomuser.me/api/portraits/men/65.jpg"
            alt="Владимир"
          />
          <div class="review-content">
            <div class="review-name">Владимир, 41 год</div>
            <div class="review-text">
              Простой и понятный сервис. Помог избавиться от вредных привычек и улучшить
              самочувствие.
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Decorative SVG Section Divider -->
    <div class="section-divider">
      <img src="../assets/section-divider.svg" alt="Декоративный разделитель" />
    </div>

    <!-- CTA Section -->
    <section class="cta">
      <h2>Начните свой путь к лучшим привычкам сегодня</h2>
      <p>
        Хотите ли вы повысить энергию, лучше спать, сохранять концентрацию или просто глубже понять
        себя — этот инструмент станет вашим персональным гидом к более здоровой и осознанной жизни.
      </p>
      <button class="cta-button">Присоединиться</button>
    </section>
  </div>
</template>

<script setup lang="ts">
// Component logic can be added here if needed
</script>

<style scoped>
.home {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
}

.hero {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 4rem;
  margin-bottom: 6rem;
}

.hero-content {
  flex: 1;
}

.hero h1 {
  font-size: 3.5rem;
  line-height: 1.2;
  margin-bottom: 1.5rem;
  color: #2c3e50;
}

.hero-subtitle {
  font-size: 1.25rem;
  line-height: 1.6;
  color: #666;
  margin-bottom: 2rem;
}

.hero-image {
  flex: 1;
  display: flex;
  justify-content: center;
}

.hero-image img {
  max-width: 100%;
  height: auto;
}

.facts {
  margin-bottom: 6rem;
}

.facts h2 {
  text-align: center;
  margin-bottom: 3rem;
  color: #2c3e50;
}

.facts-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
}

.fact-card {
  text-align: center;
  padding: 2rem;
  background: #f8f9fa;
  border-radius: 12px;
  transition: transform 0.3s ease;
}

.fact-card:hover {
  transform: translateY(-5px);
}

.fact-icon {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

.fact-card h3 {
  font-size: 2rem;
  color: #4caf50;
  margin-bottom: 1rem;
}

.benefits {
  margin-bottom: 6rem;
  padding: 4rem 0;
  background: #f8f9fa;
  border-radius: 12px;
}

.benefits h2 {
  text-align: center;
  margin-bottom: 3rem;
  color: #2c3e50;
}

.benefits-content {
  display: flex;
  align-items: center;
  gap: 4rem;
  max-width: 1000px;
  margin: 0 auto;
}

.benefit-text {
  flex: 1;
}

.benefit-text ul {
  list-style-type: none;
  padding: 0;
}

.benefit-text li {
  margin-bottom: 1rem;
  padding-left: 2rem;
  position: relative;
}

.benefit-text li:before {
  content: '✓';
  color: #4caf50;
  position: absolute;
  left: 0;
}

.benefit-image {
  flex: 1;
  display: flex;
  justify-content: center;
}

.benefit-image img {
  max-width: 100%;
  height: auto;
}

.how-it-works {
  margin-bottom: 6rem;
}

.how-it-works h2 {
  text-align: center;
  margin-bottom: 3rem;
  color: #2c3e50;
}

.steps {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
}

.step {
  text-align: center;
  padding: 2rem;
  background: #f8f9fa;
  border-radius: 12px;
  transition: transform 0.3s ease;
}

.step:hover {
  transform: translateY(-5px);
}

.step-number {
  width: 40px;
  height: 40px;
  background: #4caf50;
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 1rem;
  font-weight: bold;
}

.features {
  margin-bottom: 6rem;
}

.features h2 {
  text-align: center;
  margin-bottom: 3rem;
  color: #2c3e50;
}

.feature-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
}

.feature {
  text-align: center;
  padding: 2rem;
  background: #f8f9fa;
  border-radius: 12px;
  transition: transform 0.3s ease;
}

.feature:hover {
  transform: translateY(-5px);
}

.feature-icon {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.cta {
  text-align: center;
  padding: 4rem 2rem;
  background: #f8f9fa;
  border-radius: 12px;
  margin-bottom: 4rem;
}

.cta h2 {
  margin-bottom: 1.5rem;
  color: #2c3e50;
}

.cta p {
  max-width: 600px;
  margin: 0 auto 2rem;
  color: #666;
}

.cta-button {
  background: #4caf50;
  color: white;
  border: none;
  padding: 1rem 2rem;
  font-size: 1.1rem;
  border-radius: 8px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.cta-button:hover {
  background: #45a049;
}

.reviews {
  margin-bottom: 6rem;
}
.reviews h2 {
  text-align: center;
  margin-bottom: 2.5rem;
  color: #2c3e50;
}
.reviews-list {
  display: flex;
  flex-wrap: wrap;
  gap: 2rem;
  justify-content: center;
}
.review-card {
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(44, 62, 80, 0.06);
  padding: 1.5rem 1.2rem;
  display: flex;
  align-items: flex-start;
  gap: 1.2rem;
  max-width: 340px;
  min-width: 260px;
  flex: 1 1 260px;
}
.review-avatar {
  width: 56px;
  height: 56px;
  border-radius: 50%;
  object-fit: cover;
  border: 2px solid #4caf50;
}
.review-content {
  flex: 1;
}
.review-name {
  font-weight: 600;
  color: #4caf50;
  margin-bottom: 0.3rem;
}
.review-text {
  color: #2c3e50;
  font-size: 1rem;
  line-height: 1.5;
}
.section-divider {
  width: 100%;
  display: flex;
  justify-content: center;
  margin: 3rem 0 2rem 0;
}
.section-divider img {
  width: 100%;
  max-width: 700px;
  height: auto;
  opacity: 0.7;
}

@media (max-width: 768px) {
  .hero {
    flex-direction: column;
    text-align: center;
  }

  .hero h1 {
    font-size: 2.5rem;
  }

  .benefits-content {
    flex-direction: column;
  }

  .steps,
  .feature-grid,
  .facts-grid {
    grid-template-columns: 1fr;
  }
}
</style>
