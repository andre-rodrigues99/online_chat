import { defineStore } from 'pinia';

export interface Message {
      id: number,
      content: string,
      timestamp: Date,
      isOwn: boolean
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
        content: msg.content,
        timestamp: msg.timestamp,
        isOwn: true
      })
    },

    addMessage(msg: Message) {
      this.messages.push({
        id: msg.id,
        content: msg.content,
        timestamp: msg.timestamp,
        isOwn: false
      })
    }
  }
});
