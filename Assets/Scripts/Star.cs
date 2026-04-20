using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Star : MonoBehaviour
{
    public string Name;
    public string star;
    public string planet;
    public int planetNumber;
    public int moon;
    public int antenaLevel;

    public int relationtips = -100;

    [SerializeField] private TMP_Text starNameText;

    public ResAmountPair greetingsGift;
    public ResAmountPair[] trades;
    public ResAmountPair waitedResource = null;
    public static Dictionary<string, Star> knownStars = new Dictionary<string, Star>();

    private void Awake()
    {
        knownStars.Clear();
        if (greetingsGift == null)
            greetingsGift = new ResAmountPair((ResourcesPanel.resource)Random.Range(1, 3), Random.Range(1, 4));
        else if (greetingsGift.amount <= 0)
            greetingsGift.amount = Random.Range(1, 4);

        if (string.IsNullOrEmpty(Name))
            Name = Translator.GenerateName(4, 8);

        if (string.IsNullOrEmpty(star))
            star = Translator.GenerateName(4, 8);

        if (string.IsNullOrEmpty(planet))
            planet = Translator.GenerateName(4, 8);

        planetNumber = Random.Range(1, 12);

        GetComponentInChildren<SpriteRenderer>().enabled = false;
        starNameText.transform.parent.gameObject.SetActive(false);
    }

    public void TurnOn()
    {
        if (antenaLevel > Antena.level)
            return;

        GetComponentInChildren<SpriteRenderer>().enabled = true;
        starNameText.transform.parent.gameObject.SetActive(true);
        if (relationtips == -100)
            starNameText.text = "???";
        else
            starNameText.text = star;
    }

    public void TurnOff()
    {
        if (antenaLevel > Antena.level)
            return;

        GetComponentInChildren<SpriteRenderer>().enabled = false;
        starNameText.transform.parent.gameObject.SetActive(false);
    }

    public void Select()
    {
        if (antenaLevel > Antena.level)
            return;

        GetComponentInChildren<SpriteRenderer>().transform.localScale *= 1.3f;
        SpaceAntena.selectedStar = this;
    }

    public void Deselect()
    {
        if (antenaLevel > Antena.level)
            return;

        GetComponentInChildren<SpriteRenderer>().transform.localScale /= 1.3f;
        SpaceAntena.selectedStar = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.GetComponent<SpaceAntena>() != null)
            TurnOn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.GetComponent<SpaceAntena>() != null)
            TurnOff();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SpaceAntena>() != null)
            Select();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<SpaceAntena>() != null)
            Deselect();
    }
}