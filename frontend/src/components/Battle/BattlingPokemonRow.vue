<template>
  <tr :class="{ 'table-info': active }">
    <td v-text="speed" />
    <td><pokemon-icon :pokemon="pokemon" /></td>
    <td>
      <template v-if="pokemon.surname">
        {{ pokemon.surname }}
        <br />
      </template>
      <gender-icon :gender="pokemon.gender" />
      {{ ' ' }}
      <b-link :href="`/pokemon/${pokemon.id}`" target="_blank">
        {{ pokemon.species.name }} {{ $t('pokemon.levelFormat', { level: pokemon.level }) }} <font-awesome-icon icon="external-link-alt" />
      </b-link>
      <br />
      <template v-if="trainer">
        <gender-icon :gender="trainer.gender" />
        {{ ' ' }}
        <b-link :href="`/trainers/${trainer.id}`" target="_blank">
          {{ trainer.name }} ({{ pokemon.position }}) <font-awesome-icon icon="external-link-alt" />
        </b-link>
      </template>
      <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
    </td>
    <td>
      <b-link href="#" v-b-modal="`editCondition_${pokemon.id}`"><pokemon-condition :pokemon="pokemon" /></b-link>
      <condition-modal :id="`editCondition_${pokemon.id}`" :pokemon="pokemon" />
    </td>
    <td>
      <ability-info :ability="pokemon.ability" />
      <template v-if="pokemon.heldItem">
        <br />
        <item-info :item="pokemon.heldItem" />
      </template>
    </td>
    <td>
      <icon-button
        v-if="active && !hasEnded && pokemon.currentHitPoints > 0"
        class="mx-1"
        icon="magic"
        text="battle.makeMove.label"
        variant="danger"
        @click="makeBattleMove(pokemon)"
      />
      <switch-pokemon v-if="isTrainerBattle || team === 'players'" :active="active" :pokemon="pokemon" />
      <icon-button
        v-if="pokemon.currentHitPoints === 0 && team === 'opponents'"
        class="mx-1"
        icon="level-up-alt"
        text="battle.experience.label"
        variant="info"
        @click="distributeExperience(pokemon)"
      />
    </td>
  </tr>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import AbilityInfo from './AbilityInfo.vue'
import ConditionModal from './ConditionModal.vue'
import ItemInfo from './ItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'
import SwitchPokemon from './SwitchPokemon.vue'
import { getStatisticModifier } from '@/helpers/statisticUtils'

export default {
  name: 'BattlingPokemonRow',
  components: {
    AbilityInfo,
    ConditionModal,
    ItemInfo,
    PokemonCondition,
    SwitchPokemon
  },
  props: {
    active: {
      type: Boolean,
      required: true
    },
    pokemon: {
      type: Object,
      required: true
    },
    team: {
      type: String,
      required: true
    }
  },
  computed: {
    ...mapGetters(['battleStatus', 'hasEnded', 'isTrainerBattle']),
    speed() {
      const status = this.battleStatus[this.pokemon.id] ?? {}
      return Math.floor(this.pokemon.speed * getStatisticModifier(status.speed ?? 0))
    },
    trainer() {
      return this.pokemon.history?.trainer ?? null
    }
  },
  methods: {
    ...mapActions(['distributeExperience', 'makeBattleMove'])
  }
}
</script>
