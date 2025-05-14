<template>
  <footer class="app-footer">
    <div class="footer-content">
      <div class="footer-brand">
        <img src="../../assets/logo.svg" alt="Habit Tracker Logo" class="footer-logo" />
        <span class="footer-title">Habit Tracker</span>
      </div>
      <nav class="footer-nav">
        <a href="/" class="footer-link">Главная</a>
        <a href="/about" class="footer-link">О проекте</a>
        <a href="/faq" class="footer-link">FAQ</a>
        <a href="/contact" class="footer-link">Связаться</a>
      </nav>
      <div class="footer-social">
        <a href="#" class="footer-social-link" title="Telegram"><i class="pi pi-telegram"></i></a>
        <a href="#" class="footer-social-link" title="VK"><i class="pi pi-vk"></i></a>
        <a href="#" class="footer-social-link" title="GitHub"><i class="pi pi-github"></i></a>
      </div>
    </div>
    <div class="footer-bottom">
      <span>© {{ new Date().getFullYear() }} Habit Tracker. Все права защищены.</span>
    </div>
  </footer>
</template>

<script setup lang="ts"></script>

<style scoped>
.app-footer {
  background: #f8f9fa;
  border-top: 1px solid #e0e0e0;
  padding-top: 2rem;
  padding-bottom: 1rem;
  margin-top: 3rem;
  font-size: 1rem;
  color: #2c3e50;
}
.footer-content {
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: space-between;
  gap: 2rem;
  padding: 0 1rem;
}
.footer-brand {
  display: flex;
  align-items: center;
  gap: 0.75rem;
}
.footer-logo {
  width: 36px;
  height: 36px;
}
.footer-title {
  font-weight: 700;
  font-size: 1.2rem;
  color: #4caf50;
}
.footer-nav {
  display: flex;
  gap: 1.5rem;
}
.footer-link {
  color: #2c3e50;
  text-decoration: none;
  transition: color 0.2s;
}
.footer-link:hover {
  color: #4caf50;
}
.footer-social {
  display: flex;
  gap: 1rem;
}
.footer-social-link {
  color: #2c3e50;
  font-size: 1.3rem;
  transition: color 0.2s;
}
.footer-social-link:hover {
  color: #4caf50;
}
.footer-bottom {
  text-align: center;
  margin-top: 1.5rem;
  color: #888;
  font-size: 0.95rem;
}
@media (max-width: 768px) {
  .footer-content {
    flex-direction: column;
    gap: 1.5rem;
    text-align: center;
  }
  .footer-nav {
    flex-direction: column;
    gap: 0.75rem;
  }
  .footer-social {
    justify-content: center;
  }
}
</style>
