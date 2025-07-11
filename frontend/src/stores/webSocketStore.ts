import { defineStore } from 'pinia';
import { SendMessage, useMessageStore } from './messageStore';

const BASE_URL =  import.meta.env.VITE_BASE_URL?.replace(/"/g, '') ?? 'http://localhost:5098'//'https://online-chat-backend.up.railway.app';
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

      const messageStore = useMessageStore();
      this.connection = new WebSocket(
        `${BASE_URL}${CHAT_API}`
        );

      this.connection.onopen = () => {
        const conec_msg: SendMessage =  {
          msg_to: 0,
          content: '0',
          timestamp: 0
        }

        this.connected = true;
        this.sendMessage(conec_msg);
      }

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
    },

    closeConnection() {
      if (this.connection == null || !this.connected) {
        return
      }
      this.connection.close();
      this.connected = false;
    },
    sendMessage(msg: SendMessage) {
      console.log(msg)
      if (this.connection != null && this.connected) {
        this.connection.send(new TextEncoder().encode(JSON.stringify(msg)));
      }
    }
  }
})
