<template>
  <tr :class="{ 'table-info': Boolean(winner) }">
    <td>
      <b-form-checkbox :checked="Boolean(winner)" size="lg" @change="toggleBattleExperienceWinner(pokemon)" />
    </td>
    <td><pokemon-icon :pokemon="pokemon" /></td>
    <td>
      <template v-if="pokemon.surname">
        {{ pokemon.surname }}
        <br />
      </template>
      <gender-icon :gender="pokemon.gender" /> {{ pokemon.species.name }} {{ $t('pokemon.levelFormat', { level: pokemon.level }) }}
      <template v-if="pokemon.isShiny">
        {{ ' ' }}
        <font-awesome-icon icon="star" />
      </template>
      <br />
      <template v-if="trainer"><gender-icon :gender="trainer.gender" /> {{ trainer.name }} ({{ pokemon.position }})</template>
      <template v-else><font-awesome-icon icon="paw" /> {{ $t('pokemon.wild') }}</template>
    </td>
    <td><pokemon-condition :pokemon="pokemon" /></td>
    <td>
      <ability-info :ability="pokemon.ability" />
      <template v-if="pokemon.heldItem">
        <br />
        <item-info :item="pokemon.heldItem" />
      </template>
    </td>
    <td>
      <b-form-checkbox :checked="winner && winner.hasParticipated" :disabled="!winner" @change="toggleExperienceWinnerParticipation(pokemon.id)">
        {{ $t('battle.experience.hasParticipated') }}
      </b-form-checkbox>
      <b-form-checkbox :checked="winner && winner.canEvolve" :disabled="!winner" @change="toggleExperienceWinnerEvolution(pokemon.id)">
        {{ $t('battle.experience.canEvolve') }}
      </b-form-checkbox>
    </td>
    <td>
      <template v-if="hasNoModifier">&mdash;</template>
      <template v-else>
        <div v-if="isTrainerBattle" v-t="'battle.experience.trainerBattle'" />
        <div v-if="isTraded" v-t="'battle.experience.traded'" />
        <div v-if="isHoldingLuckyEgg" v-t="'battle.experience.luckyEgg'" />
        <div v-if="highFriendship" v-t="'battle.experience.highFriendship'" />
      </template>
    </td>
    <td>
      <form-field
        :disabled="!winner"
        hideLabel
        :id="`otherModifiers_${pokemon.id}`"
        label="battle.experience.otherModifiers"
        :minValue="0"
        :maxValue="999"
        required
        :step="0.001"
        type="number"
        :value="otherModifiers"
        @input="updateExperienceWinnerOtherModifiers({ id: pokemon.id, value: Number($event) })"
      >
        <template #prepend>
          <b-input-group-prepend is-text>&times;</b-input-group-prepend>
        </template>
      </form-field>
    </td>
    <td v-if="winner">
      {{ experience }}
      <b-badge class="mx-1" v-if="experience >= pokemon.experienceToNextLevel" variant="warning">
        <font-awesome-icon icon="level-up-alt" /> {{ $t('battle.experience.levelUp') }}
      </b-badge>
    </td>
    <td v-else>&mdash;</td>
  </tr>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import AbilityInfo from './AbilityInfo.vue'
import ItemInfo from './ItemInfo.vue'
import PokemonCondition from './PokemonCondition.vue'

export default {
  name: 'WinningPokemonRow',
  components: {
    AbilityInfo,
    ItemInfo,
    PokemonCondition
  },
  props: {
    pokemon: {
      type: Object,
      required: true
    }
  },
  computed: {
    ...mapGetters(['battleExperienceDefeatedPokemon', 'battleExperienceWinners', 'isTrainerBattle']),
    experience() {
      const { level, species } = this.battleExperienceDefeatedPokemon
      let experience = ((species.baseExperienceYield ?? 0) * level) / 5
      if (!this.winner.hasParticipated) {
        experience /= 2
      }
      experience *= Math.pow((2 * level + 10) / (level + this.pokemon.level + 10), 2.5)
      experience += 1
      if (this.isTrainerBattle) {
        experience *= 1.5
      }
      if (this.isTraded) {
        experience *= 1.5
      }
      if (this.isHoldingLuckyEgg) {
        experience *= 1.5
      }
      if (this.winner.canEvolve && this.highFriendship) {
        experience *= 1.5
      } else if (this.winner.canEvolve || this.highFriendship) {
        experience = experience * Math.sqrt(1.5)
      }
      experience *= this.winner.otherModifiers
      return Math.floor(experience)
    },
    hasNoModifier() {
      return !this.winner || (!this.isTrainerBattle && !this.isTraded && !this.isHoldingLuckyEgg && !this.highFriendship)
    },
    highFriendship() {
      return this.pokemon.friendship >= 220
    },
    isHoldingLuckyEgg() {
      return this.pokemon.heldItem?.kind === 'LuckyEgg'
    },
    isTraded() {
      return this.pokemon.originalTrainer?.id !== this.trainer?.id
    },
    otherModifiers() {
      return this.winner?.otherModifiers ?? 1
    },
    trainer() {
      return this.pokemon.history?.trainer ?? null
    },
    winner() {
      return this.battleExperienceWinners[this.pokemon.id] ?? null
    }
  },
  methods: {
    ...mapActions([
      'toggleBattleExperienceWinner',
      'toggleExperienceWinnerEvolution',
      'toggleExperienceWinnerParticipation',
      'updateExperienceWinnerOtherModifiers'
    ])
  }
}
</script>
