import { defineStore } from 'pinia';

export interface Message {
      id: number,
      session_id: number,
      content: string,
      timestamp: Date,
      isOwn: boolean
}

export interface SendMessage {
      msg_to: number
      content: string,
      timestamp: number,
}

export const useMessageStore = defineStore('messages', {
  state: (): { messages: Message[] } => ({
      messages: []
  }),

  getters: {
    getMessages(state) {
      return state.messages
    }
  },
  actions: {
    addOwnMessage(msg: Message) {
      this.messages.push({
        id: msg.id,
        session_id: 0,
        content: msg.content,
        timestamp: msg.timestamp,
        isOwn: true
      })
    },

    addMessage(msg: Message) {
      this.messages.push({
        id: msg.id,
        session_id: msg.session_id,
        content: msg.content,
        timestamp: msg.timestamp,
        isOwn: false
      })
    }
  }
});
