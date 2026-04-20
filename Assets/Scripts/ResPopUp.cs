using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResPopUp : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}