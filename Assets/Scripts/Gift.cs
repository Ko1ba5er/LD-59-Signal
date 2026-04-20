using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    private bool disappearing = false;
    public ResAmountPair[] vals;
    private float timer = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        disappearing = true;
        ResourcesPanel.Show(transform.position + Vector3.up * 0.5f, vals);
        foreach (ResAmountPair pair in vals)
            ResourcesPanel.AddRes(pair.res, pair.amount);
    }

    private void Update()
    {
        if (disappearing)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, timer -= Time.deltaTime);
            if (GetComponent<SpriteRenderer>().color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}