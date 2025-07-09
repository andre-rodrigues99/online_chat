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
    addOwnMessage(content: string) {
      this.messages.push({
        id: Date.now(),
        content,
        timestamp: new Date(),
        isOwn: true
      })
    },

    addMessage(msg: Message) {
      this.messages.push({
        ...msg,
        isOwn: false,
        timestamp: new Date(msg.timestamp)
      })
    }
  }
});
