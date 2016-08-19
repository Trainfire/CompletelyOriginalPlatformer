using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public interface IHUDWorldElement
{
    void Initialize(Canvas canvas);
}

/// <summary>
/// Represents a HUD element that is bound to a MonoBehaviour in the world.
/// </summary>
/// <typeparam name="TMonoBehaviour"></typeparam>
public abstract class HUDWorldElement<TMonoBehaviour> : MonoBehaviour, IHUDWorldElement where TMonoBehaviour : MonoBehaviour
{
    protected List<TMonoBehaviour> Elements { get; private set; }
    
    protected Canvas Canvas { get; private set; }

    void IHUDWorldElement.Initialize(Canvas canvas)
    {
        Canvas = canvas;

        Elements = new List<TMonoBehaviour>();

        FindObjectsOfType<TMonoBehaviour>()
            .ToList()
            .ForEach(x => Elements.Add(x));

        OnInitialize(Elements);
    }

    protected virtual void OnInitialize(List<TMonoBehaviour> elements) { }
}
