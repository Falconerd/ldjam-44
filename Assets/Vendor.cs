using UnityEngine;
using UnityEngine.SceneManagement;

public class Vendor : MonoBehaviour
{
  [SerializeField] int nextScene;
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player")
      SceneManager.LoadScene(nextScene);
  }
}
