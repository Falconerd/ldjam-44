using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidAttackSparks : MonoBehaviour
{
  [SerializeField] float duration = 0.1f;

  // Update is called once per frame
  void Update()
  {
    duration -= Time.deltaTime;
    if (duration <= 0)
      Destroy(gameObject);
  }
}
