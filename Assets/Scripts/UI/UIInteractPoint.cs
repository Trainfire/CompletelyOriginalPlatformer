using UnityEngine;
using UnityEngine.UI;
using Framework;

public class UIInteractPoint : MonoBehaviourEx
{
    [SerializeField] private Text _text;

    public void Show(string message)
    {
        _text.text = message;
        SetVisibility(true);
    }
}