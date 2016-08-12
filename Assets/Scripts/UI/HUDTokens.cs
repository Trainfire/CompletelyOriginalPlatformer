using UnityEngine;
using UnityEngine.UI;

public class HUDTokens : MonoBehaviour
{
    [SerializeField]
    private Text _collected;
    public int Collected
    {
        set { _collected.text = value.ToString(); }
    }

    [SerializeField]
    private Text _total;
    public int Total
    {
        set { _total.text = value.ToString(); }
    }
}
