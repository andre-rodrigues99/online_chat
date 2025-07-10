import { defineStore } from 'pinia';
import { useMessageStore } from './messageStore';

const BASE_URL =  import.meta.env.VITE_BASE_URL?.replace(/"/g, '') ?? 'https://online-chat-backend.up.railway.app';
const CHAT_API = import.meta.env.VITE_CHAT_API?.replace(/"/g, '') ?? '/ws_chat';

export const useWebSocketStore = defineStore('websocket', {
  state: () => ({
    connected: false,
    connection: null as WebSocket | null
  }),

  getters: {
    isConnected(state) {
      return state.connected;
    },
    getConnection(state) {
      return state.connection
    }
  },

  actions: {
    async startConnection() {
      if (this.connection != null && this.connected) {
        return
      }

      try {
        const messageStore = useMessageStore();
        this.connection = new WebSocket(
          `${BASE_URL}${CHAT_API}`
          );
        this.connected = true;

        this.connection.onmessage = (event) => {
          const data = event.data;

          const message = {
            id: data.id,
            session_id: data.session_id,
            content: data.content,
            timestamp: data.timestamp,
            isOwn: data.isOwn
          }

          messageStore.addMessage(message)
          };
      } catch (err) {
        console.log('error: ', err)
        return
      }
    },

    closeConnection() {
      if (this.connection == null || !this.connected) {
        return
      }
      this.connection.close();
      this.connected = false;
    },
    sendMessage(msg: string) {
      if (this.connection != null && this.connected) {
        this.connection.send(msg);
      }
    }
  }
})
