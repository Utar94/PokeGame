import Vue from 'vue'
import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faCheck,
  faCog,
  faExternalLinkAlt,
  faInfoCircle,
  faKey,
  faPlus,
  faSave,
  faSignInAlt,
  faSignOutAlt,
  faSyncAlt,
  faUser,
  faVial
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(faCheck, faCog, faExternalLinkAlt, faInfoCircle, faKey, faPlus, faSave, faSignInAlt, faSignOutAlt, faSyncAlt, faUser, faVial)

Vue.component('font-awesome-icon', FontAwesomeIcon)
