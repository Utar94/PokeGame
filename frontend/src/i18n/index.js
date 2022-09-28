import Vue from 'vue'
import VueI18n from 'vue-i18n'
import en from './en.js'

Vue.use(VueI18n)

export default new VueI18n({
  dateTimeFormats: {
    en: {
      card: {
        year: 'numeric',
        month: '2-digit',
        day: 'numeric'
      },
      medium: {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        second: 'numeric'
      }
    },
    fr: {
      medium: {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: 'numeric',
        minute: 'numeric',
        second: 'numeric'
      }
    }
  },
  numberFormats: {
    en: {
      percent: {
        style: 'percent'
      }
    }
  },
  locale: 'en',
  fallbackLocale: 'en',
  messages: { en }
})
