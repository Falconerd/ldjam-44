using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField]
  int health = 3;
  [SerializeField]
  GameObject bloodEffect;
  internal virtual int TakeDamage(int damage)
  {
    Debug.Log("damage: " + damage + ", " + "health: " + health);
    health -= damage;
    Debug.Log("Damage Taken " + health);
    Instantiate(bloodEffect, transform.position, Quaternion.identity);
    return health;
  }

  internal virtual int TakeDamage(int damage, GameObject attacker) { return health -= damage; }

  protected virtual void Update()
  {
    if (health <= 0)
      Destroy(gameObject);
  }
}