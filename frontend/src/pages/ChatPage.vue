<template>
  <q-page class="chat-screen">
    <!-- Background Effects -->
    <div class="chat-background"></div>

    <!-- Main Chat Layout -->
    <div class="chat-layout">
      <!-- Sidebar -->
      <div class="chat-sidebar" :class="{ 'sidebar-mobile-hidden': !showSidebar }">
        <!-- Sidebar Header -->
        <div class="sidebar-header">
          <div class="sidebar-user">
            <q-avatar size="40px" class="sidebar-avatar">
              <img src="https://cdn.quasar.dev/img/avatar.png" alt="Avatar">
            </q-avatar>
            <div class="sidebar-user-info">
              <div class="sidebar-user-name">João Silva</div>
              <div class="sidebar-user-status">Online</div>
            </div>
          </div>
          <q-btn
            icon="add"
            flat
            round
            dense
            class="sidebar-add-btn"
            @click="showNewChatDialog = true"
          />
        </div>

        <!-- Search Bar -->
        <div class="sidebar-search">
          <q-input
            v-model="searchQuery"
            placeholder="Buscar conversas..."
            outlined
            dense
            class="search-input"
          >
            <template v-slot:prepend>
              <q-icon name="search" class="search-icon" />
            </template>
          </q-input>
        </div>

        <!-- Chat List -->
        <div class="chat-list">
          <div
            v-for="chat in filteredChats"
            :key="chat.id"
            class="chat-item"
            :class="{ 'chat-item-active': activeChat?.id === chat.id }"
            @click="selectChat(chat)"
          >
            <q-avatar size="48px" class="chat-avatar">
              <img :src="chat.avatar" :alt="chat.name">
              <q-badge
                v-if="chat.online"
                color="positive"
                floating
                rounded
                class="chat-online-badge"
              />
            </q-avatar>
            <div class="chat-info">
              <div class="chat-name">{{ chat.name }}</div>
              <div class="chat-last-message">{{ chat.lastMessage }}</div>
            </div>
            <div class="chat-meta">
              <div class="chat-time">{{ formatTime(chat.lastMessageTime) }}</div>
              <q-badge
                v-if="chat.unreadCount > 0"
                color="primary"
                rounded
                class="chat-unread-badge"
              >
                {{ chat.unreadCount }}
              </q-badge>
            </div>
          </div>
        </div>
      </div>

      <!-- Chat Area -->
      <div class="chat-area">
        <div v-if="!activeChat" class="chat-empty">
          <div class="empty-illustration">
            <q-icon name="chat" size="80px" class="empty-icon" />
          </div>
          <h3 class="empty-title">Selecione uma conversa</h3>
          <p class="empty-subtitle">Escolha uma conversa na barra lateral para começar</p>
        </div>

        <div v-else class="chat-container">
          <!-- Chat Header -->
          <div class="chat-header">
            <q-btn
              icon="menu"
              flat
              round
              dense
              class="mobile-menu-btn"
              @click="showSidebar = !showSidebar"
            />
            <div class="chat-header-info">
              <q-avatar size="40px" class="chat-header-avatar">
                <img :src="activeChat.avatar" :alt="activeChat.name">
                <q-badge
                  v-if="activeChat.online"
                  color="positive"
                  floating
                  rounded
                  class="chat-online-badge"
                />
              </q-avatar>
              <div class="chat-header-details">
                <div class="chat-header-name">{{ activeChat.name }}</div>
                <div class="chat-header-status">
                  {{ activeChat.online ? 'Online' : 'Offline' }}
                </div>
              </div>
            </div>
            <div class="chat-header-actions">
              <q-btn icon="videocam" flat round dense class="header-action-btn" />
              <q-btn icon="call" flat round dense class="header-action-btn" />
              <q-btn icon="more_vert" flat round dense class="header-action-btn" />
            </div>
          </div>

          <!-- Messages Container -->
          <div class="messages-container" ref="messagesContainer">
            <div class="messages-list">
              <div
                v-for="message in messages"
                :key="message.id"
                class="message-wrapper"
                :class="{ 'message-own': message.isOwn }"
              >
                <div class="message-bubble">
                  <div class="message-content">{{ message.content }}</div>
                  <div class="message-time">{{ formatTime(message.timestamp) }}</div>
                </div>
              </div>
            </div>
          </div>

          <!-- Message Input -->
          <div class="message-input-container">
            <div class="message-input-wrapper">
              <q-btn
                icon="attach_file"
                flat
                round
                dense
                class="input-action-btn"
              />
              <q-input
                v-model="newMessage"
                placeholder="Digite sua mensagem..."
                outlined
                dense
                class="message-input"
                @keydown.enter="sendMessage"
              >
                <template v-slot:append>
                  <q-btn
                    icon="emoji_emotions"
                    flat
                    round
                    dense
                    class="input-action-btn"
                  />
                </template>
              </q-input>
              <q-btn
                icon="send"
                color="primary"
                round
                class="send-btn"
                :disabled="!newMessage.trim()"
                @click="sendMessage"
              />
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- New Chat Dialog -->
    <q-dialog v-model="showNewChatDialog" class="new-chat-dialog">
      <q-card class="new-chat-card">
        <q-card-section class="new-chat-header">
          <div class="new-chat-title">Nova Conversa</div>
        </q-card-section>

        <q-card-section class="new-chat-form">
          <q-input
            v-model="newChatName"
            label="Nome do usuário"
            outlined
            class="new-chat-input"
          />
        </q-card-section>

        <q-card-actions class="new-chat-actions">
          <q-btn
            flat
            no-caps
            class="new-chat-cancel-btn"
            @click="showNewChatDialog = false"
          >
            Cancelar
          </q-btn>
          <q-btn
            class="new-chat-create-btn"
            no-caps
            unelevated
            @click="createNewChat"
          >
            Criar
          </q-btn>
        </q-card-actions>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script setup>
import { ref, computed, onMounted, nextTick } from 'vue'
import { useQuasar } from 'quasar'

const $q = useQuasar()

// Reactive data
const showSidebar = ref(true)
const searchQuery = ref('')
const activeChat = ref(null)
const newMessage = ref('')
const showNewChatDialog = ref(false)
const newChatName = ref('')
const messagesContainer = ref(null)

// Mock data
const chats = ref([
  {
    id: 1,
    name: 'Maria Santos',
    avatar: 'https://cdn.quasar.dev/img/avatar2.jpg',
    lastMessage: 'Olá! Como você está?',
    lastMessageTime: new Date(Date.now() - 5 * 60 * 1000),
    unreadCount: 2,
    online: true
  },
  {
    id: 2,
    name: 'Pedro Costa',
    avatar: 'https://cdn.quasar.dev/img/avatar3.jpg',
    lastMessage: 'Vamos nos encontrar hoje?',
    lastMessageTime: new Date(Date.now() - 30 * 60 * 1000),
    unreadCount: 0,
    online: false
  },
  {
    id: 3,
    name: 'Ana Silva',
    avatar: 'https://cdn.quasar.dev/img/avatar4.jpg',
    lastMessage: 'Obrigada pela ajuda!',
    lastMessageTime: new Date(Date.now() - 2 * 60 * 60 * 1000),
    unreadCount: 1,
    online: true
  },
  {
    id: 4,
    name: 'Carlos Oliveira',
    avatar: 'https://cdn.quasar.dev/img/avatar5.jpg',
    lastMessage: 'Até logo!',
    lastMessageTime: new Date(Date.now() - 24 * 60 * 60 * 1000),
    unreadCount: 0,
    online: false
  }
])

const messages = ref([
  {
    id: 1,
    content: 'Olá! Tudo bem?',
    timestamp: new Date(Date.now() - 10 * 60 * 1000),
    isOwn: false
  },
  {
    id: 2,
    content: 'Oi! Tudo ótimo e você?',
    timestamp: new Date(Date.now() - 8 * 60 * 1000),
    isOwn: true
  },
  {
    id: 3,
    content: 'Também estou bem! Obrigada por perguntar 😊',
    timestamp: new Date(Date.now() - 5 * 60 * 1000),
    isOwn: false
  },
  {
    id: 4,
    content: 'Que bom! Vamos nos falar mais tarde?',
    timestamp: new Date(Date.now() - 2 * 60 * 1000),
    isOwn: true
  }
])

// Computed
const filteredChats = computed(() => {
  if (!searchQuery.value) return chats.value
  return chats.value.filter(chat =>
    chat.name.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
})

// Methods
const selectChat = (chat) => {
  activeChat.value = chat
  chat.unreadCount = 0
  if (window.innerWidth < 768) {
    showSidebar.value = false
  }
}

const sendMessage = () => {
  if (!newMessage.value.trim()) return

  const message = {
    id: Date.now(),
    content: newMessage.value,
    timestamp: new Date(),
    isOwn: true
  }

  messages.value.push(message)
  newMessage.value = ''

  // Scroll to bottom
  nextTick(() => {
    scrollToBottom()
  })

  // Simulate response
  setTimeout(() => {
    const response = {
      id: Date.now() + 1,
      content: 'Obrigado pela mensagem! 👍',
      timestamp: new Date(),
      isOwn: false
    }
    messages.value.push(response)

    nextTick(() => {
      scrollToBottom()
    })
  }, 1000)
}

const scrollToBottom = () => {
  if (messagesContainer.value) {
    messagesContainer.value.scrollTop = messagesContainer.value.scrollHeight
  }
}

const formatTime = (date) => {
  const now = new Date()
  const diff = now - date
  const hours = Math.floor(diff / (1000 * 60 * 60))
  const minutes = Math.floor(diff / (1000 * 60))

  if (hours >= 24) {
    return date.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit' })
  } else if (hours >= 1) {
    return `${hours}h`
  } else {
    return `${minutes}m`
  }
}

const createNewChat = () => {
  if (!newChatName.value.trim()) return

  const newChat = {
    id: Date.now(),
    name: newChatName.value,
    avatar: 'https://cdn.quasar.dev/img/avatar.png',
    lastMessage: 'Nova conversa iniciada',
    lastMessageTime: new Date(),
    unreadCount: 0,
    online: Math.random() > 0.5
  }

  chats.value.unshift(newChat)
  newChatName.value = ''
  showNewChatDialog.value = false

  $q.notify({
    message: 'Nova conversa criada!',
    type: 'positive',
    position: 'top',
    timeout: 2000
  })
}

// Handle responsive behavior
const handleResize = () => {
  if (window.innerWidth >= 768) {
    showSidebar.value = true
  }
}

onMounted(() => {
  window.addEventListener('resize', handleResize)
  handleResize()

  // Select first chat by default
  if (chats.value.length > 0) {
    selectChat(chats.value[0])
  }
})
</script>

<style lang="scss" scoped>
.chat-screen {
  min-height: 100vh;
  background: #000;
  color: #fff;
  padding: 0;
}

.chat-background {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background:
    radial-gradient(circle at 20% 30%, rgba(120, 119, 198, 0.1), transparent 60%),
    radial-gradient(circle at 80% 70%, rgba(168, 85, 247, 0.1), transparent 60%),
    linear-gradient(135deg, #000 0%, #111 100%);
  z-index: -1;
}

.chat-layout {
  display: flex;
  height: 100vh;
}

// Sidebar
.chat-sidebar {
  width: 320px;
  background: rgba(255, 255, 255, 0.05);
  border-right: 1px solid rgba(255, 255, 255, 0.1);
  backdrop-filter: blur(20px);
  display: flex;
  flex-direction: column;
  transition: transform 0.3s ease;

  @media (max-width: 767px) {
    position: fixed;
    top: 0;
    left: 0;
    bottom: 0;
    z-index: 100;

    &.sidebar-mobile-hidden {
      transform: translateX(-100%);
    }
  }
}

.sidebar-header {
  padding: 20px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.sidebar-user {
  display: flex;
  align-items: center;
  gap: 12px;
}

.sidebar-avatar {
  border: 2px solid rgba(255, 255, 255, 0.1);
}

.sidebar-user-info {
  flex: 1;
}

.sidebar-user-name {
  font-weight: 600;
  color: #fff;
  font-size: 1rem;
}

.sidebar-user-status {
  font-size: 0.875rem;
  color: rgba(255, 255, 255, 0.6);
}

.sidebar-add-btn {
  color: rgba(255, 255, 255, 0.7);
  background: rgba(255, 255, 255, 0.1);

  &:hover {
    color: #fff;
    background: rgba(255, 255, 255, 0.15);
  }
}

.sidebar-search {
  padding: 0 20px 20px;
}

.search-input {
  :deep(.q-field__control) {
    background: rgba(255, 255, 255, 0.05);
    border-radius: 12px;
    border: 1px solid rgba(255, 255, 255, 0.1);
  }

  :deep(.q-field__native) {
    color: #fff;
  }

  :deep(.q-field__label) {
    color: rgba(255, 255, 255, 0.6);
  }
}

.search-icon {
  color: rgba(255, 255, 255, 0.5);
}

.chat-list {
  flex: 1;
  overflow-y: auto;
  padding: 0 12px;

  &::-webkit-scrollbar {
    width: 4px;
  }

  &::-webkit-scrollbar-track {
    background: transparent;
  }

  &::-webkit-scrollbar-thumb {
    background: rgba(255, 255, 255, 0.2);
    border-radius: 2px;
  }
}

.chat-item {
  display: flex;
  align-items: center;
  padding: 12px 8px;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.2s ease;
  margin-bottom: 4px;

  &:hover {
    background: rgba(255, 255, 255, 0.05);
  }

  &.chat-item-active {
    background: rgba(168, 85, 247, 0.2);
    border: 1px solid rgba(168, 85, 247, 0.3);
  }
}

.chat-avatar {
  position: relative;
  margin-right: 12px;
}

.chat-online-badge {
  width: 12px;
  height: 12px;
  border: 2px solid #000;
}

.chat-info {
  flex: 1;
  min-width: 0;
}

.chat-name {
  font-weight: 600;
  color: #fff;
  font-size: 0.9rem;
  margin-bottom: 4px;
}

.chat-last-message {
  color: rgba(255, 255, 255, 0.6);
  font-size: 0.8rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.chat-meta {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 4px;
}

.chat-time {
  color: rgba(255, 255, 255, 0.5);
  font-size: 0.75rem;
}

.chat-unread-badge {
  min-width: 18px;
  height: 18px;
  font-size: 0.7rem;
  font-weight: 600;
}

// Chat Area
.chat-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  background: rgba(255, 255, 255, 0.02);
}

.chat-empty {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 40px;
}

.empty-illustration {
  margin-bottom: 32px;
}

.empty-icon {
  color: rgba(255, 255, 255, 0.3);
}

.empty-title {
  color: #fff;
  font-size: 1.5rem;
  font-weight: 600;
  margin-bottom: 8px;
}

.empty-subtitle {
  color: rgba(255, 255, 255, 0.6);
  font-size: 1rem;
}

.chat-container {
  flex: 1;
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.chat-header {
  display: flex;
  align-items: center;
  padding: 16px 20px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(10px);
}

.mobile-menu-btn {
  display: none;
  margin-right: 12px;
  color: rgba(255, 255, 255, 0.7);

  @media (max-width: 767px) {
    display: block;
  }
}

.chat-header-info {
  display: flex;
  align-items: center;
  gap: 12px;
  flex: 1;
}

.chat-header-avatar {
  position: relative;
}

.chat-header-details {
  flex: 1;
}

.chat-header-name {
  font-weight: 600;
  color: #fff;
  font-size: 1rem;
}

.chat-header-status {
  font-size: 0.875rem;
  color: rgba(255, 255, 255, 0.6);
}

.chat-header-actions {
  display: flex;
  gap: 8px;
}

.header-action-btn {
  color: rgba(255, 255, 255, 0.7);

  &:hover {
    color: #fff;
    background: rgba(255, 255, 255, 0.1);
  }
}

// Messages
.messages-container {
  flex: 1;
  overflow-y: auto;
  padding: 20px;

  &::-webkit-scrollbar {
    width: 4px;
  }

  &::-webkit-scrollbar-track {
    background: transparent;
  }

  &::-webkit-scrollbar-thumb {
    background: rgba(255, 255, 255, 0.2);
    border-radius: 2px;
  }
}

.messages-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.message-wrapper {
  display: flex;

  &.message-own {
    justify-content: flex-end;
  }
}

.message-bubble {
  max-width: 70%;
  padding: 12px 16px;
  border-radius: 18px;
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.1);

  .message-own & {
    background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
    border-color: transparent;
  }
}

.message-content {
  color: #fff;
  font-size: 0.9rem;
  line-height: 1.4;
  margin-bottom: 4px;
}

.message-time {
  font-size: 0.75rem;
  color: rgba(255, 255, 255, 0.6);
  text-align: right;
}

// Message Input
.message-input-container {
  padding: 20px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.02);
}

.message-input-wrapper {
  display: flex;
  align-items: center;
  gap: 12px;
}

.input-action-btn {
  color: rgba(255, 255, 255, 0.7);

  &:hover {
    color: #fff;
    background: rgba(255, 255, 255, 0.1);
  }
}

.message-input {
  flex: 1;

  :deep(.q-field__control) {
    background: rgba(255, 255, 255, 0.05);
    border-radius: 20px;
    border: 1px solid rgba(255, 255, 255, 0.1);
    min-height: 44px;
  }

  :deep(.q-field__native) {
    color: #fff;
    padding: 0 16px;
  }

  :deep(.q-field__label) {
    color: rgba(255, 255, 255, 0.6);
  }
}

.send-btn {
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  color: #fff;
  width: 44px;
  height: 44px;

  &:disabled {
    opacity: 0.5;
  }
}

// New Chat Dialog
.new-chat-dialog {
  .q-dialog__inner {
    padding: 24px;
  }
}

.new-chat-card {
  background: rgba(0, 0, 0, 0.95);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 24px;
  backdrop-filter: blur(40px);
  min-width: 400px;
  max-width: 90vw;
}

.new-chat-header {
  text-align: center;
  padding: 32px 32px 16px;
}

.new-chat-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #fff;
}

.new-chat-form {
  padding: 16px 32px;
}

.new-chat-input {
  :deep(.q-field__control) {
    background: rgba(255, 255, 255, 0.05);
    border-radius: 12px;
  }

  :deep(.q-field__label) {
    color: rgba(255, 255, 255, 0.7);
  }

  :deep(.q-field__native) {
    color: #fff;
  }
}

.new-chat-actions {
  padding: 16px 32px 32px;
  gap: 16px;
}

.new-chat-cancel-btn {
  color: rgba(255, 255, 255, 0.7);
  flex: 1;

  &:hover {
    color: #fff;
  }
}

.new-chat-create-btn {
  background: linear-gradient(135deg, #a855f7 0%, #3b82f6 100%);
  color: #fff;
  font-weight: 600;
  flex: 1;
  padding: 12px 24px;
  border-radius: 12px;
}
</style>