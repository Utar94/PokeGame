import Vue from 'vue'
import { library } from '@fortawesome/fontawesome-svg-core'
import {
  faBan,
  faCheck,
  faCog,
  faEnvelope,
  faExternalLinkAlt,
  faInfoCircle,
  faKey,
  faPlus,
  faRobot,
  faSave,
  faSignInAlt,
  faSignOutAlt,
  faSyncAlt,
  faTrashAlt,
  faUser,
  faVial
} from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(
  faBan,
  faCheck,
  faCog,
  faEnvelope,
  faExternalLinkAlt,
  faInfoCircle,
  faKey,
  faPlus,
  faRobot,
  faSave,
  faSignInAlt,
  faSignOutAlt,
  faSyncAlt,
  faTrashAlt,
  faUser,
  faVial
)

Vue.component('font-awesome-icon', FontAwesomeIcon)
