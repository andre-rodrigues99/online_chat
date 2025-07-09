import { defineStore } from 'pinia';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { sendMessage } from '@microsoft/signalr/dist/esm/Utils';

const BACKEND_BASE_URL = import.meta.env.VITE_BACKEND_BASE_URL?.replace(/"/g, '') ?? '';
const CHAT_API = import.meta.env.VITE_CHAT_API?.replace(/"/g, '') ?? '';


export const useWebSocketStore = defineStore('websocket', {
  state: () => ({
    connected: false,
    connection: null as HubConnection | null
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
    defineConnection() {
      this.connection = new HubConnectionBuilder()
        .withUrl(`${BACKEND_BASE_URL}${CHAT_API}`)
        .withAutomaticReconnect()
        .build();
    },
    async startConnection() {
      if (this.connection == null || this.connected) {
        return
      }

      try {
        await this.connection.start();
        this.connected = true;
      } catch (err) {
        return
      }
    },

    closeConnection() {
      if (this.connection == null || !this.connected) {
        return
      }
      this.connection.stop();
      this.connected = false;
    },
    sendMessage(msg: string) {
      if (this.connection != null && this.connected) {
        this.connection.send(msg);
      }
    }
  }
})
