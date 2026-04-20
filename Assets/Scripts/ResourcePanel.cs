using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image image;
    [SerializeField] private ResourcesPanel.resource Res;

    private void Start()
    {
        ResourcesPanel.onResChanged[Res].AddListener(OnResChanged);
        text.text = "0";
    }

    public void OnResChanged(int val)
    {
        text.text = val.ToString();
    }
}