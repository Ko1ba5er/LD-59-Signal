using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResourcesPanel : MonoBehaviour
{
    public enum resource { iron, crystal, stone, organica };
    public static Dictionary<resource, UnityEvent<int>> onResChanged = new ();
    public static Dictionary<resource, int> resources = new ();
    public static Dictionary<resource, Sprite> pics = new ();

    [SerializeField] private Sprite[] _pics;
    [Space]
    [SerializeField] private RectTransform panelPrefab;
    [SerializeField] private RectTransform prefab;
    [SerializeField] private RectTransform Upgrade;

    private static ResourcesPanel instance;

    private void Awake()
    {
        instance = this;

        onResChanged = new();
        resources = new();
        pics = new();
        foreach (resource r in Enum.GetValues(typeof(resource)))
        {
            onResChanged.Add(r, new UnityEvent<int>());
            resources.Add(r, 0);
        }

        foreach (resource r in Enum.GetValues(typeof(resource)))
        {
            pics.Add(r, _pics[(int)r]);
        }
        HideUpgrade();
    }

    public static void Show(Vector3 position, params ResAmountPair[] vals)
    {
        var pp = Instantiate(instance.panelPrefab, position, Quaternion.identity);
        pp.sizeDelta = new Vector2(pp.sizeDelta.x, vals.Length * 60 + 20);
        for (int i = 0; i < vals.Length; i++)
        {
            var p = Instantiate(instance.prefab, pp);
            p.GetChild(0).GetComponentInChildren<Image>().sprite = ResourcesPanel.pics[vals[i].res];
            p.GetComponentInChildren<TMP_Text>().text = vals[i].amount.ToString();
            p.anchoredPosition = Vector3.up * (-60 * i - 40) + Vector3.right * 100;
        }
    }

    public static void ShowUpgrade(params ResAmountPair[] vals)
    {
        instance.Upgrade.sizeDelta = new Vector2(instance.Upgrade.sizeDelta.x, vals.Length * 60 + 70);

        for (int i = 1; i < instance.Upgrade.childCount; i++)
            Destroy(instance.Upgrade.GetChild(i).gameObject);

        for (int i = 0; i < vals.Length; i++)
        {
            var p = Instantiate(instance.prefab, instance.Upgrade);
            p.GetChild(0).GetComponentInChildren<Image>().sprite = ResourcesPanel.pics[vals[i].res];
            p.GetComponentInChildren<TMP_Text>().text = vals[i].amount.ToString();
            p.anchoredPosition = Vector3.up * (-60 * i - 90) + Vector3.right * 100;
        }

        instance.Upgrade.gameObject.SetActive(true);
    }

    public static void HideUpgrade()
    {
        instance.Upgrade.gameObject.SetActive(false);
    }

    public static void AddRes(resource res, int amount)
    {
        resources[res] += amount;
        onResChanged[res].Invoke(resources[res]);
    }
}