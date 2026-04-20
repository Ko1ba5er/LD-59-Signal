using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mine : MonoBehaviour
{
    private bool playerEntered = false;
    [SerializeField] private float TimeToGetRes;
    [SerializeField] private Transform Pickaxe;
    private float timer;
    private float animTimer;
    public int level;
    public ResAmountPair[][] levels =
    {
        new ResAmountPair[] { new (ResourcesPanel.resource.stone, 2) },
        new ResAmountPair[] { new(ResourcesPanel.resource.stone, 5) },
        new ResAmountPair[] { new (ResourcesPanel.resource.stone, 7), new (ResourcesPanel.resource.iron, 3) },
        new ResAmountPair[] { new(ResourcesPanel.resource.stone, 15), new(ResourcesPanel.resource.iron, 6) },
    };
    public ResAmountPair[][] levelsCost =
    {
        new ResAmountPair[] { new(ResourcesPanel.resource.stone, 15), new(ResourcesPanel.resource.iron, 3) },
        new ResAmountPair[] { new(ResourcesPanel.resource.stone, 45), new(ResourcesPanel.resource.iron, 20) },
        new ResAmountPair[] { new(ResourcesPanel.resource.iron, 20), new(ResourcesPanel.resource.crystal, 5) },
    };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerEntered = true;
            if (level < levelsCost.Length)
                ResourcesPanel.ShowUpgrade(levelsCost[level]);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerEntered = false;
            ResourcesPanel.HideUpgrade();
        }
    }

    private void Update()
    {
        if (playerEntered || Pickaxe.rotation.eulerAngles.z > 1f)
            Pickaxe.rotation = Quaternion.Euler(0, 0, Mathf.Abs(Mathf.Sin((animTimer += Time.deltaTime) * 5)) * 100 - 60);

        if (!playerEntered)
            return;

        if (level < levelsCost.Length && Keyboard.current.wKey.wasPressedThisFrame && levelsCost[level].All(lc => ResourcesPanel.resources[lc.res] >= lc.amount))
        {
            foreach (var lc in levelsCost[level])
            {
                ResourcesPanel.AddRes(lc.res, -lc.amount);
            }
            level++;

            if (level < levelsCost.Length)
                ResourcesPanel.ShowUpgrade(levelsCost[level]);
            else
                ResourcesPanel.HideUpgrade();
        }

        timer += Time.deltaTime;
        if (timer >= TimeToGetRes)
        {
            timer -= TimeToGetRes;
            foreach (var pair in levels[level])
                ResourcesPanel.AddRes(pair.res, pair.amount);

            ResourcesPanel.Show(transform.position, levels[level]);
        }
    }
}