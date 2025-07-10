import { defineStore } from 'pinia';
import { SendMessage, useMessageStore } from './messageStore';

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
          const data = JSON.parse(event.data);
          console.log(data)

          const message = {
            id: data.id,
            session_id: 0,
            content: data.content,
            timestamp: data.timestamp,
            isOwn: false
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
    sendMessage(msg: SendMessage) {
      if (this.connection != null && this.connected) {
        this.connection.send(new TextEncoder().encode(JSON.stringify(msg)));
      }
    }
  }
})
