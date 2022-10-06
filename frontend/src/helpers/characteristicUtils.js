const characteristics = {
  Attack: {
    0: 'ProudPower',
    1: 'LikesThrashAbout',
    2: 'LittleQuickTempered',
    3: 'LikesFight',
    4: 'QuickTempered'
  },
  Defense: {
    0: 'SturdyBody',
    1: 'CapableTakingHits',
    2: 'HighlyPersistent',
    3: 'GoodEndurance',
    4: 'GoodPerseverance'
  },
  HP: {
    0: 'LovesEat',
    1: 'TakesPlentySiestas',
    2: 'NodsOffLot',
    3: 'ScattersThingsOften',
    4: 'LikesRelax'
  },
  SpecialAttack: {
    0: 'HighlyCurious',
    1: 'Mischievous',
    2: 'ThoroughlyCunning',
    3: 'OftenLostThought',
    4: 'VeryFinicky'
  },
  SpecialDefense: {
    0: 'StrongWilled',
    1: 'SomewhatVain',
    2: 'StronglyDefiant',
    3: 'HatesLose',
    4: 'SomewhatStubborn'
  },
  Speed: {
    0: 'LikesRun',
    1: 'AlertSounds',
    2: 'ImpetuousSilly',
    3: 'SomewhatClown',
    4: 'QuickFlee'
  }
}

export function getCharacteristic(statistic, value) {
  return characteristics[statistic][value % 5]
}
