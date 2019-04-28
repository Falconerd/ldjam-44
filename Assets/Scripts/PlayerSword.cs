using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
  Color color = Color.white;
  float power = 1;
  [SerializeField] float maxPower = 5;
  [SerializeField] SpriteRenderer spriteRenderer;

  void Update()
  {
    color.g = 1 - power / maxPower;
    color.b = 1 - power / maxPower;
    // When the power is max, we want the sword to be red... As it gets weaker, we want the redness
    // to faint...
    spriteRenderer.color = color;
  }

  internal float SetSwordPower(float value)
  {
    if (value > maxPower)
      value = maxPower;
    return power = value;
  }

  internal float DecreaseSwordPower()
  {
    if (power > 0)
      power--;
    return power;
  }
}
