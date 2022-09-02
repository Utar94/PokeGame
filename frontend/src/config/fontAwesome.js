import Vue from 'vue'
import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faBan,
  faCheck,
  faCog,
  faDice,
  faEnvelope,
  faExternalLinkAlt,
  faIdCard,
  faInfoCircle,
  faKey,
  faMagic,
  faPaw,
  faPlus,
  faRobot,
  faSave,
  faShoppingCart,
  faSignInAlt,
  faSignOutAlt,
  faSyncAlt,
  faTools,
  faTrashAlt,
  faUser,
  faUsers,
  faVial
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(
  faBan,
  faCheck,
  faCog,
  faDice,
  faEnvelope,
  faExternalLinkAlt,
  faIdCard,
  faInfoCircle,
  faKey,
  faMagic,
  faPaw,
  faPlus,
  faRobot,
  faSave,
  faShoppingCart,
  faSignInAlt,
  faSignOutAlt,
  faSyncAlt,
  faTools,
  faTrashAlt,
  faUser,
  faUsers,
  faVial
)

Vue.component('font-awesome-icon', FontAwesomeIcon)
