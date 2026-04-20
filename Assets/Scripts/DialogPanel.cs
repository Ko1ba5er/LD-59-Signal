using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    public class DialogAnswer
    {
        public string text;
        public Action callback;

        public DialogAnswer(string text, Action callback)
        {
            this.text = text;
            this.callback = callback;
        }
    }

    [SerializeField] private TMP_Text question;
    [SerializeField] private Transform answersBox;
    [SerializeField] private Button buttonPrefab;
    private DialogAnswer[] variants;

    public void Ask(string q, params DialogAnswer[] ans)
    {
        question.text = q;
        gameObject.SetActive(true);
        variants = ans;

        for (int i = 0; i < answersBox.childCount; i++)
        {
            Destroy(answersBox.GetChild(i).gameObject);
        }

        foreach (DialogAnswer answer in variants)
        {
            var b = Instantiate(buttonPrefab, answersBox);
            b.GetComponentInChildren<TMP_Text>().text = answer.text;
            b.onClick.AddListener(
                () =>
                {
                    gameObject.SetActive(false);
                    answer.callback();
                });
        }
    }
}