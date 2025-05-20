<template>
  <footer class="app-footer">
    <div class="footer-content">
      <nav class="footer-nav">
        <Button label="О проекте" text class="footer-link" @click="$router.push('/about')" />
        <Button label="FAQ" text class="footer-link" @click="$router.push('/faq')" />
        <Button label="Связаться" text class="footer-link" @click="$router.push('/contact')" />
      </nav>
    </div>
    <div class="footer-bottom">
      <span>© {{ new Date().getFullYear() }} TrackMe. Все права защищены.</span>
    </div>
  </footer>
</template>

<script setup lang="ts">
import Button from 'primevue/button'
import { useRouter } from 'vue-router'
const $router = useRouter()
</script>

<style scoped>
.app-footer {
  background: #fff;
  border-top: 1px solid #e0e0e0;
  padding-top: 2rem;
  padding-bottom: 1rem;
  font-size: 1rem;
  color: #2c3e50;
  box-shadow: 0 -2px 4px rgba(0, 0, 0, 0.05);
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

.logo-container {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.logo {
  height: 36px;
  width: auto;
}

.app-title {
  font-size: 1.2rem;
  font-weight: 700;
  color: #4caf50;
  margin: 0;
}

.footer-nav {
  display: flex;
  gap: 1rem;
  flex: 1;
  justify-content: center;
  align-items: center;
}

.footer-link.p-button {
  font-weight: 500;
  color: #666;
  padding: 0.5rem 1rem;
  transition: all 0.3s ease;
  background: none;
  border: none;
}

.footer-link.p-button:hover {
  color: #4caf50;
  background: rgba(76, 175, 80, 0.1);
}

.footer-social {
  display: flex;
  gap: 1rem;
}

.footer-social-link {
  color: #2c3e50;
  font-size: 1.3rem;
  transition: all 0.2s;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
}

.footer-social-link:hover {
  color: #4caf50;
  background-color: rgba(76, 175, 80, 0.1);
}

.footer-bottom {
  text-align: center;
  margin-top: 1.5rem;
  color: #888;
  font-size: 0.95rem;
  padding: 0 1rem;
}

@media (max-width: 768px) {
  .app-footer {
    padding-top: 1.5rem;
    margin-top: 2rem;
  }
  .footer-content {
    flex-direction: column;
    gap: 1.5rem;
    text-align: center;
    padding: 0 1.5rem;
  }
}

@media (max-width: 480px) {
  .app-footer {
    padding-top: 1.25rem;
    padding-bottom: 1rem;
  }
  .footer-content {
    gap: 1.25rem;
  }
  .footer-nav {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    width: 100%;
    gap: 0.5rem 1rem;
    justify-content: center;
  }
  .footer-link.p-button {
    padding: 0.75rem 0;
    font-size: 0.95rem;
  }
  .footer-social {
    width: 100%;
    justify-content: center;
    gap: 1.5rem;
  }
  .footer-social-link {
    font-size: 1.5rem;
  }
  .footer-bottom {
    font-size: 0.85rem;
    margin-top: 1.25rem;
  }
}
</style>
