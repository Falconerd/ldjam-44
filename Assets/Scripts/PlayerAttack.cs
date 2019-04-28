using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
  [SerializeField]
  int damage = 1;
  [SerializeField]
  float startTimeBtwAttack = 0.3f;
  [SerializeField]
  float attackRange;
  [SerializeField]
  Transform attackTransform;
  [SerializeField]
  LayerMask layerMask;
  float timeBtwAttack;

  void Update()
  {
    if (timeBtwAttack <= 0)
    {
      if (Input.GetButtonDown("Attack"))
      {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackTransform.position, attackRange, layerMask);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
          enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
        }
      }
      timeBtwAttack = startTimeBtwAttack;
    }
    else
      timeBtwAttack -= Time.deltaTime;
  }

  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(attackTransform.position, attackRange);
  }
}
