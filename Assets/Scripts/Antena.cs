using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Antena : MonoBehaviour
{
    private bool playerEntered = false;
    [SerializeField] private GameObject space;
    [SerializeField] private GameObject main;

    public static int level;
    public ResAmountPair[][] levelsCost =
    {
        new ResAmountPair[] { new(ResourcesPanel.resource.iron, 7) },
        new ResAmountPair[] { new(ResourcesPanel.resource.iron, 25), new(ResourcesPanel.resource.crystal, 7) },
        new ResAmountPair[] { new(ResourcesPanel.resource.iron, 43), new(ResourcesPanel.resource.crystal, 2), new(ResourcesPanel.resource.organica, 17) },
    };

    private void Awake()
    {
        level = 0;
        space.SetActive(false);
    }

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
        if (!playerEntered)
            return;

        if (Keyboard.current.eKey.isPressed)
        {
            space.SetActive(true);
            main.SetActive(false);
        }

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
    }
}