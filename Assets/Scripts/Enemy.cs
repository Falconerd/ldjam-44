using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField]
  int health = 3;
  [SerializeField]
  GameObject bloodEffect;
  internal int TakeDamage(int damage)
  {
    Debug.LogFormat("Damage Taken $1", health);
    Instantiate(bloodEffect, transform.position, Quaternion.identity);
    return health -= damage;
  }

  void Update()
  {
    if (health <= 0)
      Destroy(gameObject);
  }
}