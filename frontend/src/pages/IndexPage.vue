<template>
  <q-page class="chat-homepage">
    <div class="hero-background"></div>
    <q-header
      class="nav-header"
      :class="{ 'nav-scrolled': scrollY > 50 }"
      reveal
      height-hint="80"
    >
      <q-toolbar class="nav-toolbar">
        <q-toolbar-title class="nav-title">
          ChaChaChat
        </q-toolbar-title>

        <div class="desktop-menu">
          <q-btn flat no-caps class="nav-link">Sobre</q-btn>
          <q-btn flat no-caps class="nav-link">Contato</q-btn>
          <q-btn
            class="login-btn"
            no-caps
            unelevated
            @click="showLoginDialog = true"
          >
            Entrar
          </q-btn>
        </div>
      </q-toolbar>
    </q-header>


    <section class="hero-section">
      <div class="hero-content">
        <h1 class="hero-title" :style="{ transform: `translateY(${scrollY * 0.2}px)` }">
          Conversas que<br>
          <span class="hero-title-gradient">conectam</span>
        </h1>

        <p class="hero-subtitle" :style="{ transform: `translateY(${scrollY * 0.15}px)` }">
          A nova era dos bate-papos online. Simples, rápido e elegante.<br>
          Conecte-se com pessoas de todo o mundo em tempo real.
        </p>

        <div class="hero-actions" :style="{ transform: `translateY(${scrollY * 0.1}px)` }">
          <q-btn
            class="hero-btn-primary"
            no-caps
            unelevated
            size="lg"
            @click="startChat"
          >
            Começar agora
            <q-icon name="arrow_forward" class="q-ml-sm" />
          </q-btn>

          <q-btn
            class="hero-btn-secondary"
            no-caps
            flat
            size="lg"
            @click="startChat"
          >
            <q-icon name="person_add" class="q-mr-sm" />
            Cadastre-se
          </q-btn>
        </div>
      </div>
    </section>

    <!-- Features Section -->
    <section class="features-section">
      <div class="features-container">
        <div class="features-header">
          <h2 class="features-title">Recursos incríveis</h2>
          <p class="features-subtitle">
            Tudo que você precisa para uma experiência de chat perfeita
          </p>
        </div>

        <div class="features-grid">
          <div
            v-for="(feature, index) in features"
            :key="index"
            class="feature-card"
            :style="{
              animationDelay: `${index * 0.1}s`,
              transform: `translateY(${Math.max(0, (scrollY - 400) * 0.1)}px)`
            }"
          >
            <div class="feature-icon">
              <q-icon :name="feature.icon" />
            </div>
            <h3 class="feature-title">{{ feature.title }}</h3>
            <p class="feature-description">{{ feature.description }}</p>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="cta-section">
      <div class="cta-container">
        <div class="cta-content">
          <h2 class="cta-title">Pronto para começar?</h2>
          <p class="cta-subtitle">
            Converse de forma rápida e sem enrolação. Junte-se a nós!
          </p>
          <q-btn
            class="cta-btn"
            no-caps
            unelevated
            size="xl"
            @click="startChat"
          >
            Entrar na Conversa
          </q-btn>
        </div>
      </div>
    </section>

    <!-- Login Dialog -->
    <q-dialog v-model="showLoginDialog" class="login-dialog">
      <q-card class="login-card">
        <!-- Close Button -->
        <q-btn
          icon="close"
          flat
          round
          dense
          class="login-close-btn"
          @click="showLoginDialog = false"
        />

        <!-- Login Header -->
        <q-card-section class="login-header">
          <div class="login-avatar">
            <q-icon name="person" class="login-avatar-icon" />
          </div>
          <div class="login-title">Bem-vindo de volta</div>
          <div class="login-subtitle">Entre em sua conta para continuar</div>
        </q-card-section>

        <!-- Login Form -->
        <q-card-section class="login-form">
          <div class="login-input-group">
            <q-input
              v-model="email"
              label="Email"
              type="email"
              outlined
              class="login-input"
              :class="{ 'login-input-error': emailError }"
            >
              <template v-slot:prepend>
                <q-icon name="email" class="login-input-icon" />
              </template>
            </q-input>

            <q-input
              v-model="password"
              :label="showPassword ? 'Senha' : 'Senha'"
              :type="showPassword ? 'text' : 'password'"
              outlined
              class="login-input"
              :class="{ 'login-input-error': passwordError }"
            >
              <template v-slot:prepend>
                <q-icon name="lock" class="login-input-icon" />
              </template>
              <template v-slot:append>
                <q-btn
                  :icon="showPassword ? 'visibility_off' : 'visibility'"
                  flat
                  round
                  dense
                  class="login-password-toggle"
                  @click="showPassword = !showPassword"
                />
              </template>
            </q-input>
          </div>

          <div class="login-options">
            <q-checkbox
              v-model="rememberMe"
              label="Lembrar de mim"
              class="login-checkbox"
            />
            <q-btn
              flat
              no-caps
              class="login-forgot-btn"
              @click="handleForgotPassword"
            >
              Esqueceu a senha?
            </q-btn>
          </div>
        </q-card-section>

        <!-- Login Actions -->
        <q-card-section class="login-actions">
          <q-btn
            class="login-submit-btn"
            no-caps
            unelevated
            :loading="loginLoading"
            @click="handleLogin"
          >
            <span v-if="!loginLoading">Entrar</span>
            <q-spinner v-else size="20px" />
          </q-btn>

          <div class="login-divider">
            <span class="login-divider-text">ou</span>
          </div>

          <q-btn
            class="login-google-btn"
            no-caps
            unelevated
            @click="handleGoogleLogin"
          >
            <q-icon name="g_translate" class="q-mr-sm" />
            Continuar com Google
          </q-btn>

          <div class="login-signup">
            <span class="login-signup-text">Não tem uma conta?</span>
            <q-btn
              flat
              no-caps
              class="login-signup-btn"
              @click="handleSignup"
            >
              Criar conta
            </q-btn>
          </div>
        </q-card-section>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import { useQuasar } from 'quasar'
import { useRouter } from 'vue-router';

const router = useRouter();
const $q = useQuasar()

// Reactive data
const scrollY = ref(0)
const showLoginDialog = ref(false)
const email = ref('')
const password = ref('')
const showPassword = ref(false)
const rememberMe = ref(false)
const loginLoading = ref(false)
const emailError = ref(false)
const passwordError = ref(false)

// Features data
const features = ref([
  {
    icon: 'chat',
    title: 'Mensagens Instantâneas',
    description: 'Converse em tempo real com velocidade e precisão incomparáveis'
  },
  {
    icon: 'bolt',
    title: 'Ultra Rápido',
    description: 'Tecnologia de ponta para uma experiência fluida e responsiva'
  },
  {
    icon: 'security',
    title: '100% Seguro',
    description: 'Criptografia end-to-end e privacidade garantida'
  }
])

// Methods
const handleScroll = () => {
  scrollY.value = window.scrollY
}

async function startChat()  {
  await router.push("/chat");
}

const handleLogin = async () => {
  emailError.value = false
  passwordError.value = false

  if (!email.value) {
    emailError.value = true
    $q.notify({
      message: 'Por favor, digite seu email',
      type: 'negative',
      position: 'top',
      timeout: 2000
    })
    return
  }

  if (!password.value) {
    passwordError.value = true
    $q.notify({
      message: 'Por favor, digite sua senha',
      type: 'negative',
      position: 'top',
      timeout: 2000
    })
    return
  }

  loginLoading.value = true

  try {
    await new Promise(resolve => setTimeout(resolve, 1500))

    showLoginDialog.value = false
    email.value = ''
    password.value = ''
    rememberMe.value = false

  } catch (error) {
    $q.notify({
      message: 'Erro ao fazer login. Tente novamente.',
      type: 'negative',
      position: 'top',
      timeout: 2000
    })
  } finally {
    loginLoading.value = false
  }
}

const handleGoogleLogin = () => {
  $q.notify({
    message: 'Login com Google em desenvolvimento...',
    type: 'info',
    position: 'top',
    timeout: 2000
  })
}

const handleForgotPassword = () => {
  $q.notify({
    message: 'Link de recuperação enviado para seu email',
    type: 'info',
    position: 'top',
    timeout: 3000
  })
}

const handleSignup = () => {
  showLoginDialog.value = false
  $q.notify({
    message: 'Redirecionando para criar conta...',
    type: 'info',
    position: 'top',
    timeout: 2000
  })
}

onMounted(() => {
  window.addEventListener('scroll', handleScroll)
})

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll)
})
</script>

<style lang="css" scoped>
.chat-homepage {
  min-height: 100vh;
  background: #000;
  color: #fff;
  overflow-x: hidden;
}

.hero-background {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background:
    radial-gradient(circle at 30% 50%, rgba(120, 119, 198, 0.15), transparent 70%),
    radial-gradient(circle at 70% 80%, rgba(168, 85, 247, 0.15), transparent 70%),
    linear-gradient(135deg, #000 0%, #111 100%);
  z-index: -1;
}

.nav-header {
  background: transparent;
  transition: all 0.3s ease;
}

.nav-header .nav-scrolled {
    background: rgba(0, 0, 0, 0.8);
    backdrop-filter: blur(20px);
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  }

.nav-toolbar {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 24px;
}

.nav-title {
  font-size: 1.5rem;
  font-weight: 700;
  background: linear-gradient(135deg, #fff 0%, #ccc 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.desktop-menu {
  display: none;
  align-items: center;
  gap: 32px;


}

@media (min-width: 768px) {
  .desktop-menu {
    display: flex;
  }
  .mobile-menu-btn {
    display: block;
  }
}



.nav-link {
  color: rgba(255, 255, 255, 0.8);
  font-weight: 500;
  transition: color 0.3s ease;
}

.nav-link:hover {
    color: #fff;
  }

.login-btn {
  background: #fff;
  color: #000;
  font-weight: 600;
  padding: 8px 24px;
  border-radius: 24px;
  transition: all 0.3s ease;
}

.login-btn:hover {
    background: #f0f0f0;
    transform: translateY(-2px);
  }

.mobile-menu-btn {
  display: block;
}

.mobile-menu {
  padding: 24px;

  .q-item {
    color: #fff;
    margin-bottom: 8px;
    border-radius: 12px;

    &:hover {
      background: rgba(255, 255, 255, 0.1);
    }
  }
}

.mobile-login-btn {
  background: #fff;
  color: #000;
  font-weight: 600;
  width: 100%;
  border-radius: 12px;
}

.hero-section {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 120px 24px 80px;
  text-align: center;
}

.hero-content {
  max-width: 800px;
  width: 100%;
}

.hero-badge {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 24px;
  padding: 8px 16px;
  font-size: 0.875rem;
  font-weight: 500;
  margin-bottom: 32px;
  backdrop-filter: blur(10px);
}

.hero-badge-icon {
  color: #fbbf24;
}

.hero-title {
  font-size: clamp(2.5rem, 8vw, 4.5rem);
  font-weight: 700;
  line-height: 1.1;
  margin-bottom: 24px;
  color: #fff;
}

.hero-title-gradient {
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.hero-subtitle {
  font-size: 1.25rem;
  color: rgba(255, 255, 255, 0.7);
  line-height: 1.6;
  margin-bottom: 48px;
  max-width: 600px;
  margin-left: auto;
  margin-right: auto;
}

.hero-actions {
  display: flex;
  gap: 24px;
  justify-content: center;
  flex-wrap: wrap;
}

.hero-btn-primary {
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  color: #fff;
  font-weight: 600;
  padding: 16px 32px;
  border-radius: 24px;
  transition: all 0.3s ease;
}

.hero-btn-primary:hover {
    transform: translateY(-3px);
    box-shadow: 0 20px 40px rgba(168, 85, 247, 0.3);
  }

.hero-btn-secondary {
  color: #fff;
  font-weight: 600;
  padding: 16px 32px;
  border-radius: 24px;
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
}

.hero-btn-secondary:hover {
    background: rgba(255, 255, 255, 0.1);
    transform: translateY(-2px);
  }


.features-section {
  padding: 120px 24px;
  background: rgba(255, 255, 255, 0.02);
}

.features-container {
  max-width: 1200px;
  margin: 0 auto;
}

.features-header {
  text-align: center;
  margin-bottom: 80px;
}

.features-title {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: 16px;
  color: #fff;
}

.features-subtitle {
  font-size: 1.25rem;
  color: rgba(255, 255, 255, 0.6);
  max-width: 600px;
  margin: 0 auto;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 32px;
}

.feature-card {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 24px;
  padding: 40px 32px;
  text-align: center;
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
}

.feature-card:hover {
    transform: translateY(-8px);
    background: rgba(255, 255, 255, 0.08);
    border-color: rgba(255, 255, 255, 0.2);
  }


.feature-icon {
  width: 64px;
  height: 64px;
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  border-radius: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 24px;

  .q-icon {
    font-size: 2rem;
    color: #fff;
  }
}

.feature-title {
  font-size: 1.5rem;
  font-weight: 600;
  margin-bottom: 16px;
  color: #fff;
}

.feature-description {
  color: rgba(255, 255, 255, 0.7);
  line-height: 1.6;
}

.cta-section {
  padding: 120px 24px;
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
}

.cta-container {
  max-width: 800px;
  margin: 0 auto;
  text-align: center;
}

.cta-title {
  font-size: 2.5rem;
  font-weight: 700;
  margin-bottom: 16px;
  color: #fff;
}

.cta-subtitle {
  font-size: 1.25rem;
  color: rgba(255, 255, 255, 0.9);
  margin-bottom: 48px;
  line-height: 1.6;
}

.cta-btn {
  background: #fff;
  color: #000;
  font-weight: 600;
  padding: 20px 40px;
  border-radius: 24px;
  font-size: 1.125rem;
  transition: all 0.3s ease;

  &:hover {
    transform: translateY(-3px);
    box-shadow: 0 20px 40px rgba(0, 0, 0, 0.2);
  }
}

.login-dialog {
  .q-dialog__inner {
    padding: 24px;
  }
}

.login-card {
  background: rgba(0, 0, 0, 0.95);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 32px;
  backdrop-filter: blur(40px);
  min-width: 420px;
  max-width: 90vw;
  position: relative;
  overflow: hidden;

  &::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 1px;
    background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
  }
}

.login-close-btn {
  position: absolute;
  top: 16px;
  right: 16px;
  color: rgba(255, 255, 255, 0.6);
  z-index: 1;

  &:hover {
    color: #fff;
    background: rgba(255, 255, 255, 0.1);
  }
}

.login-header {
  text-align: center;
  padding: 48px 40px 24px;
}

.login-avatar {
  width: 80px;
  height: 80px;
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  border-radius: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 24px;
  box-shadow: 0 20px 40px rgba(168, 85, 247, 0.3);
}

.login-avatar-icon {
  font-size: 2.5rem;
  color: #fff;
}

.login-title {
  font-size: 1.75rem;
  font-weight: 700;
  color: #fff;
  margin-bottom: 8px;
}

.login-subtitle {
  font-size: 1rem;
  color: rgba(255, 255, 255, 0.6);
  line-height: 1.5;
}

.login-form {
  padding: 0 40px 24px;
}

.login-input-group {
  margin-bottom: 24px;
}

.login-input {
  margin-bottom: 20px;

  &:last-child {
    margin-bottom: 0;
  }

  :deep(.q-field__control) {
    background: rgba(255, 255, 255, 0.05);
    border-radius: 16px;
    border: 1px solid rgba(255, 255, 255, 0.1);
    transition: all 0.3s ease;
    min-height: 56px;

    &:hover {
      border-color: rgba(255, 255, 255, 0.2);
      background: rgba(255, 255, 255, 0.08);
    }
  }

  :deep(.q-field--focused .q-field__control) {
    border-color: #a855f7;
    background: rgba(255, 255, 255, 0.08);
    box-shadow: 0 0 0 3px rgba(168, 85, 247, 0.1);
  }

  :deep(.q-field__label) {
    color: rgba(255, 255, 255, 0.6);
    font-weight: 500;
  }

  :deep(.q-field__native) {
    color: #fff;
    font-weight: 500;
  }

  &.login-input-error {
    :deep(.q-field__control) {
      border-color: #ef4444;
      background: rgba(239, 68, 68, 0.1);
    }
  }
}

.login-input-icon {
  color: rgba(255, 255, 255, 0.5);
  font-size: 1.25rem;
}

.login-password-toggle {
  color: rgba(255, 255, 255, 0.5);
}

.login-password-toggle:hover {
    color: #fff;
    background: rgba(255, 255, 255, 0.1);
  }

.login-options {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
}

.login-checkbox {
  :deep(.q-checkbox__label) {
    color: rgba(255, 255, 255, 0.7);
    font-weight: 500;
  }

  :deep(.q-checkbox__inner) {
    color: #a855f7;
  }
}

.login-forgot-btn {
  color: #a855f7;
  font-weight: 500;
  font-size: 0.875rem;

  &:hover {
    color: #c084fc;
  }
}

.login-actions {
  padding: 0 40px 40px;
}

.login-submit-btn {
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  color: #fff;
  font-weight: 600;
  width: 100%;
  padding: 16px 24px;
  border-radius: 16px;
  font-size: 1rem;
  margin-bottom: 24px;
  min-height: 56px;
  transition: all 0.3s ease;
}
.login-submit-btn:disabled {
    opacity: 0.7;
    transform: none;
}

.login-submit-btn:hover {
    transform: translateY(-2px);
    box-shadow: 0 20px 40px rgba(168, 85, 247, 0.4);
}


.login-divider {
  position: relative;
  text-align: center;
  margin: 24px 0;
}

.login-divider::before {
    content: '';
    position: absolute;
    top: 50%;
    left: 0;
    right: 0;
    height: 1px;
    background: rgba(255, 255, 255, 0.1);
  }

.login-divider-text {
  background: rgba(0, 0, 0, 0.95);
  color: rgba(255, 255, 255, 0.5);
  font-size: 0.875rem;
  font-weight: 500;
  padding: 0 16px;
  position: relative;
  z-index: 1;
}

.login-google-btn {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  color: #fff;
  font-weight: 500;
  width: 100%;
  padding: 16px 24px;
  border-radius: 16px;
  font-size: 1rem;
  margin-bottom: 24px;
  min-height: 56px;
  transition: all 0.3s ease;
}

.login-google-btn:hover {
    background: rgba(255, 255, 255, 0.1);
    border-color: rgba(255, 255, 255, 0.2);
    transform: translateY(-2px);
}

.login-signup {
  text-align: center;
}

.login-signup-text {
  color: rgba(255, 255, 255, 0.6);
  font-size: 0.875rem;
  margin-right: 8px;
}

.login-signup-btn {
  color: #a855f7;
  font-weight: 600;
  font-size: 0.875rem;

  &:hover {
    color: #c084fc;
  }
}
</style>
