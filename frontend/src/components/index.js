import AppFooter from './AppFooter.vue'
import Navbar from './Navbar.vue'

export default {
  AbilityEdit: () => import(/* webpackChunkName: "abilityEdit" */ './Abilities/AbilityEdit.vue'),
  AbilityList: () => import(/* webpackChunkName: "abilityList" */ './Abilities/AbilityList.vue'),
  AppFooter,
  BattleSimulator: () => import(/* webpackChunkName: "battleSimulator" */ './Battle/BattleSimulator.vue'),
  CreatePokemon: () => import(/* webpackChunkName: "createPokemon" */ './Pokemon/CreatePokemon.vue'),
  Game: () => import(/* webpackChunkName: "game" */ './Game/Game.vue'),
  ItemEdit: () => import(/* webpackChunkName: "itemEdit" */ './Items/ItemEdit.vue'),
  ItemList: () => import(/* webpackChunkName: "itemList" */ './Items/ItemList.vue'),
  MoveEdit: () => import(/* webpackChunkName: "moveEdit" */ './Moves/MoveEdit.vue'),
  MoveList: () => import(/* webpackChunkName: "moveList" */ './Moves/MoveList.vue'),
  Navbar,
  PokemonEdit: () => import(/* webpackChunkName: "pokemonEdit" */ './Pokemon/PokemonEdit.vue'),
  PokemonList: () => import(/* webpackChunkName: "pokemonList" */ './Pokemon/PokemonList.vue'),
  Profile: () => import(/* webpackChunkName: "profile" */ './Account/Profile.vue'),
  RecoverPassword: () => import(/* webpackChunkName: "recoverPassword" */ './Account/RecoverPassword.vue'),
  ResetPassword: () => import(/* webpackChunkName: "resetPassword" */ './Account/ResetPassword.vue'),
  SignIn: () => import(/* webpackChunkName: "signIn" */ './Account/SignIn.vue'),
  SignUp: () => import(/* webpackChunkName: "signUp" */ './Account/SignUp.vue'),
  SpeciesEdit: () => import(/* webpackChunkName: "speciesEdit" */ './Species/SpeciesEdit.vue'),
  SpeciesList: () => import(/* webpackChunkName: "speciesList" */ './Species/SpeciesList.vue'),
  Startup: () => import(/* webpackChunkName: "startup" */ './Startup.vue'),
  TrainerEdit: () => import(/* webpackChunkName: "trainerEdit" */ './Trainers/TrainerEdit.vue'),
  TrainerList: () => import(/* webpackChunkName: "trainerList" */ './Trainers/TrainerList.vue'),
  UserInvite: () => import(/* webpackChunkName: "userInvite" */ './Users/UserInvite.vue'),
  UserList: () => import(/* webpackChunkName: "userList" */ './Users/UserList.vue')
}
