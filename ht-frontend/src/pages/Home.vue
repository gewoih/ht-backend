<template>
  <div class="home">
    <!-- Hero Section -->
    <section class="hero">
      <div class="container">
        <div class="hero-content">
          <h1 class="hero-title">Откройте как привычки меняют вашу жизнь</h1>
          <p class="hero-subtitle">
            Умный трекер привычек с аналитикой на основе ИИ для улучшения вашего самочувствия,
            энергии и продуктивности каждый день
          </p>
          <div class="hero-buttons">
            <Button
              label="Начать бесплатно"
              icon="pi pi-arrow-right"
              iconPos="right"
              class="p-button-lg p-button-primary"
              @click="$router.push('/register')"
            />
            <Button
              label="Узнать больше"
              icon="pi pi-info-circle"
              iconPos="right"
              class="p-button-lg p-button-outlined p-button-secondary"
              @click="scrollToSection('about')"
            />
          </div>
        </div>
        <div class="hero-image">
          <img src="../assets/images/analytics-chart.jpg" alt="Аналитика трекера привычек" />
        </div>
      </div>
    </section>

    <!-- Stats Section -->
    <section class="stats">
      <div class="container">
        <div class="stats-grid">
          <div class="stat-card">
            <div class="stat-value">50+</div>
            <div class="stat-label">Доступных привычек</div>
          </div>
          <div class="stat-card">
            <div class="stat-value">6</div>
            <div class="stat-label">Показателей самочувствия</div>
          </div>
          <div class="stat-card">
            <div class="stat-value">2</div>
            <div class="stat-label">Вида аналитики</div>
          </div>
        </div>
      </div>
    </section>

    <!-- About Section -->
    <section id="about" class="about">
      <div class="container">
        <div class="section-header">
          <h2>Сила малых привычек</h2>
          <p>Научно подтвержденные принципы формирования устойчивых привычек</p>
        </div>
        <div class="habit-facts">
          <div class="fact-card">
            <div class="fact-icon">
              <i class="pi pi-percentage"></i>
            </div>
            <h3>Правило 1% улучшений</h3>
            <p>
              Если вы улучшаете себя всего на 1% каждый день, через год вы станете в 37 раз лучше.
              Маленькие изменения в привычках дают удивительные результаты.
            </p>
          </div>
          <div class="fact-card">
            <div class="fact-icon">
              <i class="pi pi-sync"></i>
            </div>
            <h3>Петля привычки</h3>
            <p>
              Каждая привычка состоит из стимула, действия, награды и желания. Понимание этой
              структуры — ключ к изменению любого поведения.
            </p>
          </div>
          <div class="fact-card">
            <div class="fact-icon">
              <i class="pi pi-calendar"></i>
            </div>
            <h3>Системы, не цели</h3>
            <p>
              Фокус на системе (ежедневная практика) вместо цели дает намного лучшие долгосрочные
              результаты. Создайте систему отслеживания привычек.
            </p>
          </div>
          <div class="fact-card">
            <div class="fact-icon">
              <i class="pi pi-chart-line"></i>
            </div>
            <h3>Идентичность, а не результаты</h3>
            <p>
              Настоящие изменения начинаются с вашей идентичности. Станьте человеком, который "ведет
              здоровый образ жизни", а не "хочет похудеть".
            </p>
          </div>
        </div>
      </div>
    </section>

    <!-- How It Works -->
    <section class="how-it-works">
      <div class="container">
        <div class="section-header">
          <h2>Как работает наш трекер</h2>
          <p>Простой процесс для глубокого понимания своих привычек</p>
        </div>
        <div class="steps">
          <div class="step-card">
            <div class="step-number">1</div>
            <h3>Выберите привычки</h3>
            <p>
              Отметьте из более чем 50 привычек те, которые хотите отслеживать — от физической
              активности до медитации и режима сна.
            </p>
          </div>
          <div class="step-card">
            <div class="step-number">2</div>
            <h3>Заполняйте дневник</h3>
            <p>
              Ежедневно отмечайте выполненные привычки и оцените 6 показателей самочувствия:
              энергию, настроение, качество сна, спокойствие и общую удовлетворенность.
            </p>
          </div>
          <div class="step-card">
            <div class="step-number">3</div>
            <h3>Получайте инсайты</h3>
            <p>
              Наши алгоритмы на основе ИИ анализируют данные и показывают, как конкретные привычки
              влияют на ваше физическое и эмоциональное состояние.
            </p>
          </div>
        </div>
      </div>
    </section>

    <!-- Analytics Section -->
    <section class="analytics">
      <div class="container">
        <div class="section-header">
          <h2>Умная аналитика на основе ИИ</h2>
          <p>Персонализированный анализ для лучшего понимания себя</p>
        </div>

        <div class="analytics-types">
          <div class="analytics-card">
            <div class="analytics-icon">
              <i class="pi pi-sitemap"></i>
            </div>
            <h3>Влияние привычек на самочувствие</h3>
            <p>
              Нейронные сети анализируют корреляции между вашими привычками и оценками самочувствия,
              выявляя, какие действия больше всего влияют на ваше состояние.
            </p>
            <div class="analytics-images">
              <div class="analytics-image">
                <h4>Полезные привычки</h4>
                <img
                  src="../assets/images/positive-habits.jpg"
                  alt="Полезные привычки"
                  class="impact-image"
                />
              </div>
              <div class="analytics-image">
                <h4>Вредные привычки</h4>
                <img
                  src="../assets/images/negative-habits.jpg"
                  alt="Вредные привычки"
                  class="impact-image"
                />
              </div>
            </div>
          </div>

          <div class="analytics-card">
            <div class="analytics-icon">
              <i class="pi pi-chart-line"></i>
            </div>
            <h3>Динамика показателей во времени</h3>
            <p>
              Отслеживайте изменения всех шести показателей самочувствия с течением времени,
              находите закономерности и тренды для достижения оптимального состояния.
            </p>
            <div class="analytics-image">
              <img
                src="../assets/images/analytics-chart.jpg"
                alt="Динамика показателей"
                class="impact-image"
              />
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Daily Tracking -->
    <section class="daily-tracking">
      <div class="container">
        <div class="section-header">
          <h2>Ежедневное отслеживание</h2>
          <p>6 показателей для полной картины вашего состояния</p>
        </div>

        <div class="metrics-grid">
          <div class="metric-card">
            <div class="metric-icon">
              <i class="pi pi-heart"></i>
            </div>
            <h3>Самочувствие</h3>
            <p>Общее физическое состояние организма и ощущение здоровья</p>
          </div>
          <div class="metric-card">
            <div class="metric-icon">
              <i class="pi pi-bolt"></i>
            </div>
            <h3>Энергия</h3>
            <p>Уровень жизненных сил и способность выполнять задачи</p>
          </div>
          <div class="metric-card">
            <div class="metric-icon">
              <i class="pi pi-thumbs-up"></i>
            </div>
            <h3>Настроение</h3>
            <p>Эмоциональное состояние и отношение к происходящему</p>
          </div>
          <div class="metric-card">
            <div class="metric-icon">
              <i class="pi pi-moon"></i>
            </div>
            <h3>Качество сна</h3>
            <p>Насколько хорошо и полноценно вы отдохнули за ночь</p>
          </div>
          <div class="metric-card">
            <div class="metric-icon">
              <i class="pi pi-eye"></i>
            </div>
            <h3>Спокойствие</h3>
            <p>Уровень стресса и внутреннего равновесия в течение дня</p>
          </div>
          <div class="metric-card">
            <div class="metric-icon">
              <i class="pi pi-star"></i>
            </div>
            <h3>Удовлетворенность</h3>
            <p>Общая оценка дня и чувство удовлетворения прожитым днем</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features">
      <div class="container">
        <div class="section-header">
          <h2>Ключевые особенности</h2>
          <p>Все необходимое для трансформации ваших привычек</p>
        </div>
        <div class="features-grid">
          <div class="feature-card">
            <div class="feature-icon">
              <i class="pi pi-check-square"></i>
            </div>
            <h3>Более 50 привычек</h3>
            <p>Обширная база привычек для отслеживания во всех сферах жизни</p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">
              <i class="pi pi-microchip-ai"></i>
            </div>
            <h3>ИИ-аналитика</h3>
            <p>Алгоритмы машинного обучения для выявления скрытых зависимостей</p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">
              <i class="pi pi-calendar"></i>
            </div>
            <h3>Ежедневные записи</h3>
            <p>Удобный журнал для фиксации привычек и показателей самочувствия</p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">
              <i class="pi pi-chart-bar"></i>
            </div>
            <h3>Наглядные графики</h3>
            <p>Визуализация прогресса и трендов для лучшего понимания</p>
          </div>
          <div class="feature-card">
            <div class="feature-icon">
              <i class="pi pi-lock"></i>
            </div>
            <h3>Защита данных</h3>
            <p>Ваши личные данные надежно защищены и принадлежат только вам</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Pricing Section -->
    <section class="pricing" id="pricing">
      <div class="container">
        <div class="section-header">
          <h2>Доступные тарифы</h2>
          <p>Выберите подходящий вам план</p>
        </div>
        <div class="pricing-cards">
          <div class="pricing-card">
            <div class="pricing-header">
              <h3>Базовый</h3>
              <div class="pricing-price">
                <span class="price">Бесплатно</span>
                <span class="period">навсегда</span>
              </div>
            </div>
            <div class="pricing-features">
              <ul>
                <li><i class="pi pi-check"></i> Выбор до 10 привычек</li>
                <li><i class="pi pi-check"></i> Ежедневный дневник</li>
                <li><i class="pi pi-check"></i> Оценка 6 показателей самочувствия</li>
                <li><i class="pi pi-check"></i> Базовая аналитика</li>
                <li><i class="pi pi-check"></i> Графики динамики показателей</li>
                <li><i class="pi pi-check"></i> Напоминания</li>
                <li><i class="pi pi-times"></i> Доступ ко всем 50+ привычкам</li>
              </ul>
            </div>
          </div>
          <div class="pricing-card highlighted">
            <div class="popular-badge">Популярный</div>
            <div class="pricing-header">
              <h3>Премиум</h3>
              <div class="pricing-price">
                <span class="price">349 ₽</span>
                <span class="period">/ месяц</span>
              </div>
            </div>
            <div class="pricing-features">
              <ul>
                <li><i class="pi pi-check"></i> Доступ ко всем 50+ привычкам</li>
                <li><i class="pi pi-check"></i> Ежедневный дневник</li>
                <li><i class="pi pi-check"></i> Оценка 6 показателей самочувствия</li>
                <li><i class="pi pi-check"></i> Расширенная ИИ-аналитика</li>
                <li><i class="pi pi-check"></i> Графики динамики показателей</li>
                <li><i class="pi pi-check"></i> Приоритетная поддержка</li>
                <li><i class="pi pi-check"></i> Ранний доступ к новым функциям</li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Final CTA -->
    <section class="cta">
      <div class="container">
        <div class="cta-content">
          <h2>Начните свой путь к лучшим привычкам сегодня</h2>
          <p>
            Всего несколько минут в день для отслеживания привычек помогут вам лучше понять себя и
            добиться значительных улучшений в самочувствии, энергии и продуктивности
          </p>
          <Button
            label="Создать бесплатный аккаунт"
            iconPos="right"
            class="p-button-lg"
            @click="$router.push('/register')"
          />
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import Button from 'primevue/button'
import { useRouter } from 'vue-router'

const router = useRouter()

// Scroll to section function
function scrollToSection(sectionId: string) {
  const element = document.getElementById(sectionId)
  if (element) {
    element.scrollIntoView({ behavior: 'smooth' })
  }
}

onMounted(() => {
  // Check if URL has query param for scrolling to pricing
  const queryParams = new URLSearchParams(window.location.search)
  if (queryParams.get('pricing')) {
    scrollToSection('pricing')
  }
})
</script>

<style scoped>
/* Main container */
.home {
  font-family: var(--font-family);
  color: var(--color-text);
  overflow-x: hidden;
  width: 100vw; /* Use viewport width instead of % */
  max-width: 100%; /* Ensure it doesn't overflow */
}

/* Container for sections */
.container {
  width: 100%;
  max-width: 100%; /* Changed from 1200px to 100% to take full width */
  margin: 0 auto;
  padding: 0 var(--spacing-md);
  box-sizing: border-box; /* Ensure padding is included in width */
}

/* Section header */
.section-header {
  text-align: center;
  margin-bottom: var(--spacing-xl);
  max-width: 800px;
  margin-left: auto;
  margin-right: auto;
}

.section-header h2 {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: var(--spacing-xs);
  color: var(--text-color);
}

.section-header p {
  font-size: 1.1rem;
  color: var(--text-color-secondary);
}

/* Hero Section */
.hero {
  padding: 6rem 0;
  position: relative;
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e8ec 100%);
  overflow: hidden;
  width: 100%;
}

.hero .container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 2rem;
}

.hero-content {
  flex: 1;
  max-width: 600px;
}

.hero-title {
  font-size: 3.5rem;
  font-weight: 800;
  line-height: 1.2;
  margin-bottom: var(--spacing-lg);
  background: linear-gradient(90deg, var(--primary-color) 0%, #64b5f6 100%);
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
}

.hero-subtitle {
  font-size: 1.25rem;
  line-height: 1.6;
  margin-bottom: var(--spacing-xl);
  color: var(--text-color-secondary);
}

.hero-buttons {
  display: flex;
  gap: var(--spacing-md);
}

.hero-image {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
}

.hero-image img {
  max-width: 100%;
  /* Animation removed */
}

/* Stats Section */
.stats {
  padding: 3rem 0;
  background-color: white;
  width: 100%;
}

.stats-grid {
  display: flex;
  justify-content: space-around;
  flex-wrap: wrap;
  gap: var(--spacing-lg);
}

.stat-card {
  text-align: center;
  padding: var(--spacing-lg);
}

.stat-value {
  font-size: 3rem;
  font-weight: 700;
  color: var(--primary-color);
  margin-bottom: var(--spacing-xs);
}

.stat-label {
  font-size: 1.1rem;
  color: var(--text-color-secondary);
}

/* About Section */
.about {
  padding: 6rem 0;
  background-color: #f8f9fa;
  width: 100%;
}

.habit-facts {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: var(--spacing-lg);
}

.fact-card {
  background: white;
  border-radius: var(--border-radius-md);
  padding: var(--spacing-lg);
  box-shadow: var(--box-shadow-sm);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  height: 100%;
}

.fact-card:hover {
  transform: translateY(-5px);
  box-shadow: var(--box-shadow-md);
}

.fact-icon {
  width: 60px;
  height: 60px;
  background: var(--primary-color-lighter, rgba(25, 118, 210, 0.1));
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: var(--spacing-md);
}

.fact-icon i {
  font-size: 1.5rem;
  color: var(--primary-color);
}

.fact-card h3 {
  font-size: 1.25rem;
  margin-bottom: var(--spacing-sm);
  color: var(--text-color);
}

.fact-card p {
  color: var(--text-color-secondary);
  line-height: 1.6;
}

/* How It Works */
.how-it-works {
  padding: 6rem 0;
  background-color: white;
  width: 100%;
}

.steps {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: var(--spacing-lg);
}

.step-card {
  background: #f8f9fa;
  border-radius: var(--border-radius-md);
  padding: var(--spacing-xl);
  box-shadow: var(--box-shadow-sm);
  position: relative;
  height: 100%;
}

.step-number {
  width: 40px;
  height: 40px;
  background: var(--primary-color);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  margin-bottom: var(--spacing-md);
}

.step-card h3 {
  font-size: 1.25rem;
  margin-bottom: var(--spacing-md);
  color: var(--text-color);
}

.step-card p {
  color: var(--text-color-secondary);
  line-height: 1.6;
}

/* Analytics Section */
.analytics {
  padding: 6rem 0;
  background-color: #f8f9fa;
  width: 100%;
}

.analytics-types {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(450px, 1fr));
  gap: var(--spacing-xl);
}

.analytics-card {
  background: white;
  border-radius: var(--border-radius-md);
  padding: var(--spacing-lg);
  box-shadow: var(--box-shadow-sm);
  height: 100%;
}

.analytics-icon {
  width: 60px;
  height: 60px;
  background: var(--primary-color);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: var(--spacing-md);
}

.analytics-icon i {
  font-size: 1.5rem;
  color: white;
}

.analytics-card h3 {
  font-size: 1.25rem;
  margin-bottom: var(--spacing-md);
  color: var(--text-color);
}

.analytics-card p {
  color: var(--text-color-secondary);
  line-height: 1.6;
  margin-bottom: var(--spacing-lg);
}

.analytics-images {
  display: flex;
  gap: var(--spacing-md);
  margin-top: var(--spacing-md);
  flex-wrap: wrap;
}

.analytics-image {
  flex: 1;
  min-width: 250px;
}

.analytics-image h4 {
  font-size: 1.1rem;
  margin-bottom: var(--spacing-sm);
  color: var(--text-color);
  text-align: center;
}

.impact-image {
  width: 100%;
  border-radius: var(--border-radius-md);
  box-shadow: var(--box-shadow-sm);
}

/* Daily Tracking */
.daily-tracking {
  padding: 6rem 0;
  background-color: white;
  width: 100%;
}

.metrics-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr); /* Force 3 columns */
  gap: var(--spacing-lg);
}

.metric-card {
  text-align: center;
  padding: var(--spacing-lg);
  background: #f8f9fa;
  border-radius: var(--border-radius-md);
  transition: transform 0.3s ease;
  height: 100%;
}

.metric-card:hover {
  transform: translateY(-5px);
}

.metric-icon {
  width: 60px;
  height: 60px;
  background: var(--primary-color);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto var(--spacing-md);
}

.metric-icon i {
  font-size: 1.5rem;
  color: white;
}

.metric-card h3 {
  font-size: 1.25rem;
  margin-bottom: var(--spacing-sm);
  color: var(--text-color);
}

.metric-card p {
  color: var(--text-color-secondary);
}

/* Features Section */
.features {
  padding: 6rem 0;
  background-color: #f8f9fa;
  width: 100%;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr); /* Set to 3 columns */
  gap: var(--spacing-lg);
}

.feature-card {
  padding: var(--spacing-lg);
  border-radius: var(--border-radius-md);
  background: white;
  transition: transform 0.3s ease;
  box-shadow: var(--box-shadow-sm);
  height: 100%;
}

.feature-card:hover {
  transform: translateY(-5px);
}

.feature-icon {
  width: 50px;
  height: 50px;
  background: var(--primary-color);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: var(--spacing-md);
}

.feature-icon i {
  color: white;
  font-size: 1.5rem;
}

.feature-card h3 {
  font-size: 1.25rem;
  margin-bottom: var(--spacing-sm);
  color: var(--text-color);
}

.feature-card p {
  color: var(--text-color-secondary);
  line-height: 1.6;
}

/* Pricing Section */
.pricing {
  padding: 6rem 0;
  background-color: white;
  width: 100%;
}

.pricing-cards {
  display: flex;
  justify-content: center;
  gap: var(--spacing-xl);
  flex-wrap: wrap;
}

.pricing-card {
  flex: 1;
  min-width: 300px;
  max-width: 400px;
  background: #f8f9fa;
  border-radius: var(--border-radius-md);
  padding: var(--spacing-lg);
  box-shadow: var(--box-shadow-sm);
  position: relative;
  transition: transform 0.3s ease;
}

.pricing-card:hover {
  transform: translateY(-5px);
}

.pricing-card.highlighted {
  background: #f0f7ff;
  box-shadow: var(--box-shadow-md);
  border: 2px solid var(--primary-color);
}

.popular-badge {
  position: absolute;
  top: -12px;
  right: 20px;
  background: var(--primary-color);
  color: white;
  padding: 5px 15px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 500;
}

.pricing-header {
  text-align: center;
  margin-bottom: var(--spacing-lg);
  padding-bottom: var(--spacing-lg);
  border-bottom: 1px solid rgba(0, 0, 0, 0.1);
}

.pricing-header h3 {
  font-size: 1.5rem;
  margin-bottom: var(--spacing-md);
  color: var(--text-color);
}

.pricing-price {
  display: flex;
  align-items: baseline;
  justify-content: center;
  gap: 5px;
}

.price {
  font-size: 2.5rem;
  font-weight: 700;
  color: var(--primary-color);
}

.period {
  color: var(--text-color-secondary);
}

.pricing-features {
  margin-bottom: var(--spacing-lg);
}

.pricing-features ul {
  list-style-type: none;
  padding: 0;
}

.pricing-features li {
  display: flex;
  align-items: center;
  gap: var(--spacing-sm);
  margin-bottom: var(--spacing-sm);
  color: var(--text-color-secondary);
}

.pricing-features i.pi-check {
  color: var(--primary-color);
}

.pricing-features i.pi-times {
  color: #ccc;
}

.pricing-action {
  text-align: center;
}

/* CTA Section */
.cta {
  padding: 6rem 0;
  background: linear-gradient(135deg, var(--primary-color) 0%, #64b5f6 100%);
  color: white;
  width: 100%;
}

.cta-content {
  text-align: center;
  max-width: 800px;
  margin: 0 auto;
}

.cta h2 {
  font-size: 2.5rem;
  margin-bottom: var(--spacing-md);
}

.cta p {
  font-size: 1.1rem;
  margin-bottom: var(--spacing-xl);
  opacity: 0.9;
}

/* Media Queries */
@media (max-width: 991px) {
  .hero {
    padding: 4rem 0;
  }

  .hero-title {
    font-size: 2.8rem;
  }

  .section-header h2 {
    font-size: 2.2rem;
  }

  .analytics-types {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 768px) {
  .hero .container {
    flex-direction: column;
  }

  .hero-content {
    text-align: center;
    max-width: 100%;
  }

  .hero-buttons {
    justify-content: center;
  }

  .hero-title {
    font-size: 2.5rem;
  }

  .pricing-cards {
    flex-direction: column;
    align-items: center;
  }

  .pricing-card {
    width: 100%;
  }

  .stat-card {
    flex: 0 0 calc(50% - var(--spacing-lg));
  }

  .metrics-grid {
    grid-template-columns: repeat(2, 1fr); /* 2 columns on tablet */
  }

  .features-grid {
    grid-template-columns: repeat(2, 1fr); /* 2 columns on tablet */
  }

  .analytics-images {
    flex-direction: column;
  }
}

@media (max-width: 480px) {
  .hero {
    padding: 3rem 0;
  }

  .hero-title {
    font-size: 2rem;
  }

  .hero-subtitle {
    font-size: 1.1rem;
  }

  .hero-buttons {
    flex-direction: column;
    width: 100%;
  }

  .section-header h2 {
    font-size: 1.8rem;
  }

  .stat-card {
    flex: 0 0 100%;
  }

  .cta h2 {
    font-size: 2rem;
  }

  .metrics-grid {
    grid-template-columns: 1fr; /* 1 column on mobile */
  }

  .features-grid {
    grid-template-columns: 1fr; /* 1 column on mobile */
  }
}
</style>
