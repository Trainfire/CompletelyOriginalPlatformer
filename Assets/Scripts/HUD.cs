using UnityEngine;
using Framework.UI;

public class HUD : MonoBehaviour
{
    public Canvas _canvas;

    [SerializeField] private UIWorldPopup _worldPopupPrototype;

    private bool _popupVisible;
    private WorldPopup _popup;

    public void Awake()
    {
        _worldPopupPrototype = UIUtility.Add<UIWorldPopup>(transform, _worldPopupPrototype.gameObject);
        _worldPopupPrototype.gameObject.SetActive(false);
    }

    public void ShowPopup(WorldPopup popup, Vector2 worldPosition)
    {
        _worldPopupPrototype.Content = popup.Content;
        _worldPopupPrototype.SetVisibility(true);
        _popup = popup;
        _popupVisible = true;
    }

    public void Update()
    {
        if (_popupVisible && _popup != null)
        {
            var canvasRect = _canvas.pixelRect;
            var worldToView = Camera.main.WorldToViewportPoint(_popup.transform.position);
            var viewToScreen = new Vector2(worldToView.x * canvasRect.size.x - canvasRect.size.x * 0.5f, worldToView.y * canvasRect.size.y - canvasRect.size.y * 0.5f);

            var rect = _worldPopupPrototype.transform as RectTransform;
            rect.anchoredPosition = viewToScreen;
        }
    }

    public void HidePopup()
    {
        _worldPopupPrototype.SetVisibility(false);
    }
}
