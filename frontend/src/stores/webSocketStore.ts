import { defineStore } from 'pinia';
import { useMessageStore } from './messageStore';
import { Message } from 'primevue';

const BACKEND_BASE_URL =  "http://localhost:5098";//import.meta.env.VITE_BACKEND_BASE_URL?.replace(/"/g, '') ?? '';
const CHAT_API = "/ws_chat"; //import.meta.env.VITE_CHAT_API?.replace(/"/g, '') ?? '';

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
          `${BACKEND_BASE_URL}${CHAT_API}`
          );
        this.connected = true;

        this.connection.onmessage = (event) => {
          console.log("aa")

          const message = {
            id: Date.now(),
            content: event.data,
            timestamp: new Date(),
            isOwn: true
          }

          messageStore.addMessage(message)
          };
      } catch (err) {
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
      console.log(msg)
      if (this.connection != null && this.connected) {
        this.connection.send(msg);
      }
    }
  }
})
