<template>
  <b-modal :id="id" size="xl" :title="$t('battle.makeMove.title')" @shown="refresh">
    <h6 v-t="'moves.title'" />
    <make-move-table :moves="moves" :selected="move" @toggled="onMoveToggled" />
    <h6 v-t="'battle.makeMove.targets'" />
    <move-target-table :selected="selectedTargets" :targets="targets" @toggled="onTargetToggled" />
    <template v-if="move && move.move.category !== 'Status'">
      <h6 v-t="'battle.makeMove.damage'" />
      <make-move-damage :move="move" v-model="damage" />
    </template>
    <template #modal-footer="{ cancel, ok }">
      <icon-button icon="ban" text="actions.cancel" @click="reset(cancel)" />
      <icon-button disabled icon="magic" :loading="loading" text="battle.makeMove.title" variant="danger" @click="ok()" />
    </template>
  </b-modal>
</template>

<script>
import Vue from 'vue'
import { mapState } from 'vuex'
import MakeMoveDamage from './MakeMoveDamage.vue'
import MakeMoveTable from './MakeMoveTable.vue'
import MoveTargetTable from './MoveTargetTable.vue'
import { getPokemonList } from '@/api/pokemon'

export default {
  name: 'MakeMoveModal',
  components: { MakeMoveDamage, MakeMoveTable, MoveTargetTable },
  props: {
    id: {
      type: String,
      default: 'makeMove'
    },
    pokemon: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      damage: {
        burn: false,
        critical: false,
        offensiveStatistic: 0,
        other: 0,
        power: 0,
        random: 0,
        stab: 0
      },
      loading: false,
      move: null,
      pokemonIndex: {},
      selectedTargets: []
    }
  },
  computed: {
    ...mapState(['battle']),
    moves() {
      return this.orderBy(this.pokemon.moves, 'position')
    },
    targets() {
      return Object.values(this.pokemonIndex).filter(({ id }) => this.battle.activePokemon.includes(id))
    }
  },
  methods: {
    onMoveToggled(selected) {
      this.move = this.move?.move.id === selected.move.id ? null : selected
    },
    onTargetToggled({ id }) {
      const index = this.selectedTargets.findIndex(x => x === id)
      if (index >= 0) {
        Vue.delete(this.selectedTargets, index)
      } else {
        this.selectedTargets.push(id)
      }
    },
    async refresh() {
      try {
        const { data } = await getPokemonList()
        this.pokemonIndex = {}
        for (const item of data.items) {
          Vue.set(this.pokemonIndex, item.id, item)
        }
      } catch (e) {
        this.handleError(e)
      }
    },
    reset(callback = null) {
      this.move = null
      this.targets = []
      if (typeof callback === 'function') {
        callback()
      }
    }
  },
  watch: {
    move: {
      deep: true,
      handler(value) {
        const move = value?.move ?? null
        this.damage.power = move?.power ?? 0
        switch (move?.category) {
          case 'Physical':
            this.damage.offensiveStatistic = this.pokemon.attack
            break
          case 'Special':
            this.damage.offensiveStatistic = this.pokemon.specialAttack
            break
          default:
            this.damage.offensiveStatistic = 0
            break
        }
        this.damage.burn = this.pokemon.statusCondition === 'Burn' && move?.category === 'Physical'
        this.damage.random = 82 + this.roll('3d6')
        this.damage.stab = move?.type === this.pokemon.species.primaryType || move?.type === this.pokemon.species.secondaryType ? 1.5 : 1.0
      }
    }
  }
}
</script>
