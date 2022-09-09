import AppFooter from './AppFooter.vue'
import Navbar from './Navbar.vue'

export default {
  AbilityEdit: () => import(/* webpackChunkName: "abilityEdit" */ './Abilities/AbilityEdit.vue'),
  AbilityList: () => import(/* webpackChunkName: "abilityList" */ './Abilities/AbilityList.vue'),
  AppFooter,
  BattleSimulator: () => import(/* webpackChunkName: "battleSimulator" */ './Battle/BattleSimulator.vue'),
  CreatePokemon: () => import(/* webpackChunkName: "createPokemon" */ './Pokemon/CreatePokemon.vue'),
  Home: () => import(/* webpackChunkName: "home" */ './Home.vue'),
  ItemEdit: () => import(/* webpackChunkName: "itemEdit" */ './Items/ItemEdit.vue'),
  ItemList: () => import(/* webpackChunkName: "itemList" */ './Items/ItemList.vue'),
  MoveEdit: () => import(/* webpackChunkName: "moveEdit" */ './Moves/MoveEdit.vue'),
  MoveList: () => import(/* webpackChunkName: "moveList" */ './Moves/MoveList.vue'),
  PokemonEdit: () => import(/* webpackChunkName: "pokemonEdit" */ './Pokemon/PokemonEdit.vue'),
  PokemonList: () => import(/* webpackChunkName: "pokemonList" */ './Pokemon/PokemonList.vue'),
  Navbar,
  Profile: () => import(/* webpackChunkName: "profile" */ './Account/Profile.vue'),
  SignIn: () => import(/* webpackChunkName: "signIn" */ './Account/SignIn.vue'),
  SignUp: () => import(/* webpackChunkName: "signUp" */ './Account/SignUp.vue'),
  SpeciesEdit: () => import(/* webpackChunkName: "speciesEdit" */ './Species/SpeciesEdit.vue'),
  SpeciesList: () => import(/* webpackChunkName: "speciesList" */ './Species/SpeciesList.vue'),
  TrainerEdit: () => import(/* webpackChunkName: "trainerEdit" */ './Trainers/TrainerEdit.vue'),
  TrainerList: () => import(/* webpackChunkName: "trainerList" */ './Trainers/TrainerList.vue'),
  UserInvite: () => import(/* webpackChunkName: "userInvite" */ './Users/UserInvite.vue'),
  UserList: () => import(/* webpackChunkName: "userList" */ './Users/UserList.vue')
}
