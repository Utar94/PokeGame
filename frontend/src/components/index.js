import AppFooter from './AppFooter.vue'
import Navbar from './Navbar.vue'

export default {
  AbilityEdit: () => import(/* webpackChunkName: "abilityEdit" */ './Abilities/AbilityEdit.vue'),
  AbilityList: () => import(/* webpackChunkName: "abilityList" */ './Abilities/AbilityList.vue'),
  AppFooter,
  Home: () => import(/* webpackChunkName: "home" */ './Home.vue'),
  MoveEdit: () => import(/* webpackChunkName: "moveEdit" */ './Moves/MoveEdit.vue'),
  MoveList: () => import(/* webpackChunkName: "moveList" */ './Moves/MoveList.vue'),
  Navbar,
  Profile: () => import(/* webpackChunkName: "profile" */ './Account/Profile.vue'),
  SignIn: () => import(/* webpackChunkName: "signIn" */ './Account/SignIn.vue'),
  SignUp: () => import(/* webpackChunkName: "signUp" */ './Account/SignUp.vue'),
  UserInvite: () => import(/* webpackChunkName: "userInvite" */ './Users/UserInvite.vue'),
  UserList: () => import(/* webpackChunkName: "userList" */ './Users/UserList.vue')
}
