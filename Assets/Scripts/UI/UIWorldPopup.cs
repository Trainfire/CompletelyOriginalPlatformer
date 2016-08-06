using UnityEngine;
using UnityEngine.UI;
using Framework;

public class UIWorldPopup : MonoBehaviourEx
{
    [SerializeField] private Text _content;

    public string Content
    {
        set { _content.text = value; }
    }
}
