using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceAntena : MonoBehaviour
{
    [SerializeField] private float speed;
    public static Star selectedStar;

    [SerializeField] private DialogPanel dialogPanel;
    [SerializeField] private GameObject space;
    [SerializeField] private GameObject main;
    [SerializeField] private float[] levelAngles;

    private void Awake()
    {
        selectedStar = null;  
    }

    void Update()
    {
        if (Keyboard.current.dKey.isPressed)
            transform.RotateAround(transform.position, Vector3.forward, -speed * Time.deltaTime);

        if (Keyboard.current.aKey.isPressed)
            transform.RotateAround(transform.position, Vector3.forward, speed * Time.deltaTime);

        if (Antena.level < levelAngles.Length)
        {
            if (transform.localRotation.eulerAngles.z < 10)
                transform.transform.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
            else if (transform.localRotation.eulerAngles.z < 360 - levelAngles[Antena.level])
                transform.transform.localRotation = Quaternion.AngleAxis(360 - levelAngles[Antena.level], Vector3.forward);
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && !dialogPanel.gameObject.activeSelf)
        {
            space.SetActive(false);
            main.SetActive(true);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame && selectedStar != null)
        {
            if (selectedStar.relationtips == -100) //first meeting
                dialogPanel.Ask(
                    $"AO C {selectedStar.Name}\nADE L C {selectedStar.star}\nADE LN{Translator.Number(selectedStar.planetNumber)} C {selectedStar.planet}\nFGCD C BQS SJ OID MA", //greetings
                    new("OID", () => { selectedStar.relationtips = 3; Nuke.NUKE(); selectedStar.TurnOn(); }), //enemy
                    new("BQS", () =>  //friend
                    {
                        selectedStar.relationtips = 0;
                        selectedStar.TurnOn();
                        Star.knownStars.Add(selectedStar.star, selectedStar);
                        //dialogPanel.Ask(
                        //    $"PABM GHS FGCD LN MA",
                        //    new DialogPanel.DialogAnswer("ADE LN6", () => //wrong
                        //    {
                        //        dialogPanel.Ask(
                        //            $"PABM AO QRQ FGCDE {Translator.Resource(selectedStar.greetingsGift.res)}{Translator.Number(selectedStar.greetingsGift.amount)}",
                        //             new DialogPanel.DialogAnswer("PABM", () => //okay
                        //             {
                        //             })
                        //        );
                        //    }),
                        //    new DialogPanel.DialogAnswer("ADE LN2", () => //right
                        //    {
                        dialogPanel.Ask(
                            $"PABM AO QRQ FGCDE {Translator.Resource(selectedStar.greetingsGift.res)}{Translator.Number(selectedStar.greetingsGift.amount)}",
                             new DialogPanel.DialogAnswer("PABM", () => //okay
                             {
                                 Rocket.GetResources(selectedStar.greetingsGift);
                             })
                        );
                        //})
                        //);
                    }));
            else if (selectedStar.relationtips >= 3)
                dialogPanel.Ask(
                    $"HOHOHOHO\nAO QRQ EAEDCOHO",
                     new DialogPanel.DialogAnswer("PABM", () => //okay
                     {
                         selectedStar.relationtips++;
                         if (selectedStar.relationtips >= 3)
                             Nuke.NUKE();
                     }));
            else if (selectedStar.waitedResource.amount > 0) //waiting resources = anger
            {
                dialogPanel.Ask(
                    $"HOHO\nOMJ FGCD QRQ ADE {Translator.Resource(selectedStar.waitedResource.res)}{Translator.Number(selectedStar.waitedResource.amount)} MA",
                     new DialogPanel.DialogAnswer("PABM", () => //okay
                     {
                         selectedStar.relationtips++;
                         if (selectedStar.relationtips >= 3)
                             Nuke.NUKE();
                     }),
                     new DialogPanel.DialogAnswer("IDC", () => //okay
                     {
                         selectedStar.relationtips = 3;
                         Nuke.NUKE();
                     })
                );
            }
            else //trade
            {
                string q = $"ADE GAHDI\n";
                List<DialogPanel.DialogAnswer> ans = new List<DialogPanel.DialogAnswer>();
                for (int i = 0; i < selectedStar.trades.Length; i += 2)
                {
                    var n = i;
                    q += $"{i / 2 + 1} {Translator.Resource(selectedStar.trades[i].res)}{Translator.Number(selectedStar.trades[i].amount)} QDA {Translator.Resource(selectedStar.trades[i + 1].res)}{Translator.Number(selectedStar.trades[i + 1].amount)}\n";
                    ans.Add(new DialogPanel.DialogAnswer((i / 2 + 1).ToString(), () =>
                    {
                        selectedStar.waitedResource = new ResAmountPair(selectedStar.trades[n].res, selectedStar.trades[n].amount);
                        Rocket.GetResources(new ResAmountPair(selectedStar.trades[n + 1].res, selectedStar.trades[n + 1].amount));
                    }));
                }

                q += "FGCDE GAHDI";
                ans.Add(new DialogPanel.DialogAnswer("IDC", () => //no
                {
                }));

                dialogPanel.Ask(q, ans.ToArray());
            }
        }
    }
}