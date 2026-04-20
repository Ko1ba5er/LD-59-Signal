using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    private bool playerEntered;
    [SerializeField] private TMP_Dropdown resDD;
    [SerializeField] private TMP_Dropdown starsDD;
    [SerializeField] private TMP_InputField amount;
    [SerializeField] private GameObject Dialog;
    [SerializeField] private Gift gift;
    [SerializeField] private float giftShift;


    private static Rocket instance;

    private void Awake()
    {
        instance = this;
        Dialog.SetActive(false);
    }

    private void Start()
    {
        resDD.AddOptions(((ResourcesPanel.resource[])Enum.GetValues(typeof(ResourcesPanel.resource))).Select(r => new TMP_Dropdown.OptionData(r.ToString(), ResourcesPanel.pics[r], Color.white)).ToList());
    }

    private void OnEnable()
    {
        starsDD.ClearOptions();
        starsDD.AddOptions(Star.knownStars.Select(s => new TMP_Dropdown.OptionData(s.Key)).ToList());
        giftShift = 0;
    }

    public static void GetResources(params ResAmountPair[] res)
    {
        var g = Instantiate(instance.gift, instance.transform);
        g.transform.position = Vector3.right * g.transform.position.x +  Vector3.up * (10 + instance.giftShift++ * 1.5f);
        g.vals = res;
    }

    public void SendButton()
    {
        SendResources(Star.knownStars[starsDD.captionText.text], new ResAmountPair((ResourcesPanel.resource)resDD.value, int.Parse(amount.text)));
    }
        
    public static void SendResources(Star star, params ResAmountPair[] res)
    {
        if (res.All(pair => ResourcesPanel.resources[pair.res] < pair.amount))
            return;

        foreach (ResAmountPair pair in res)
        {
            ResourcesPanel.AddRes(pair.res, -pair.amount);
            if (star.waitedResource.res == pair.res)
            {
                star.waitedResource.amount -= pair.amount;
                if (star.waitedResource.amount <= 0)
                {
                    star.waitedResource.amount = 0;
                    star.relationtips = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerEntered = true;
            Dialog.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            playerEntered = false;
            Dialog.SetActive(false);
        }
    }
}