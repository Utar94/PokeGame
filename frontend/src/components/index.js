import AppFooter from './AppFooter.vue'
import Navbar from './Navbar.vue'

export default {
  AbilityEdit: () => import(/* webpackChunkName: "abilityEdit" */ './Abilities/AbilityEdit.vue'),
  AbilityList: () => import(/* webpackChunkName: "abilityList" */ './Abilities/AbilityList.vue'),
  AppFooter,
  Home: () => import(/* webpackChunkName: "home" */ './Home.vue'),
  Navbar,
  Profile: () => import(/* webpackChunkName: "profile" */ './Account/Profile.vue'),
  SignIn: () => import(/* webpackChunkName: "signIn" */ './Account/SignIn.vue')
}
