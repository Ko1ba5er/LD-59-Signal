using UnityEngine;
using UnityEngine.SceneManagement;

public class Nuke : MonoBehaviour
{
    private static Nuke instance;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene(0);
    }

    public static void NUKE()
    {
        instance.gameObject.SetActive(true);
    }
}
