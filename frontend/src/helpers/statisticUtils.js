export function getAccuracyEvasionModifier(stage) {
  if (stage < 0) {
    return 3 / (3 + -1 * stage)
  } else if (stage > 0) {
    return (3 + stage) / 3
  }
  return 1
}

export function getStatisticModifier(stage) {
  if (stage < 0) {
    return 2 / (2 + -1 * stage)
  } else if (stage > 0) {
    return (2 + stage) / 2
  }
  return 1
}
