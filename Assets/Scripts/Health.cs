using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
  [SerializeField] int health;

  [SerializeField] Image[] nodes;
  [SerializeField] Sprite sprite;
  [SerializeField] Sprite depletedSprite;

  void Update()
  {
    if (health <= 0)
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    if (health > nodes.Length)
      health = nodes.Length;

    for (int i = 0; i < nodes.Length; i++)
    {
      if (i < health)
      {
        nodes[i].sprite = sprite;
      }
      else
      {
        nodes[i].sprite = depletedSprite;
      }
    }
  }

  internal int DecreaseHealth(int amount)
  {
    return health -= amount;
  }

  internal int IncreaseHealth(int amount)
  {
    return health += amount;
  }

  internal int SetHealth(int amount)
  {
    if (amount > nodes.Length)
      amount = nodes.Length;
    if (amount < 0)
      amount = 0;
    return health = amount;
  }

  internal int ModifyHealth(int amount)
  {
    return health += amount;
  }
}
