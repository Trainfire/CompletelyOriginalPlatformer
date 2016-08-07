using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ZoneManager<T>
{
    private IZoneChangeHandler<T> _listener;
    private T _zone;

    public ZoneManager(IZoneChangeHandler<T> listener)
    {
        _listener = listener;
    }

    public void SetZone(T zone)
    {
        _zone = zone;
        _listener.OnZoneChanged(_zone);
    }
}

public interface IZoneChangeHandler<T>
{
    void OnZoneChanged(T zone);
}

public class ZoneListener<T> : IZoneChangeHandler<T>
{
    public event Action<T> ZoneChanged;

    void IZoneChangeHandler<T>.OnZoneChanged(T zone)
    {
        if (ZoneChanged != null)
            ZoneChanged(zone);
    }
}