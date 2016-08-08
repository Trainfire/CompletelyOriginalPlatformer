using System;
using Framework;

public class ZoneManager<T>
{
    public ZoneListener<T> Listener { get; private set; }

    private IZoneHandler<T> _handler;

    public T Zone { get; private set; }

    public ZoneManager()
    {
        Listener = new ZoneListener<T>();
        _handler = Listener;
    }

    public void SetZone(T zone)
    {
        Zone = zone;
        _handler.OnZoneChanged(Zone);
    }
}

public interface IZoneHandler<T>
{
    void OnZoneChanged(T zone);
}

public class ZoneListener<T> : IZoneHandler<T>
{
    public event Action<T> ZoneChanged;

    void IZoneHandler<T>.OnZoneChanged(T zone)
    {
        ZoneChanged.InvokeSafe(zone);
    }
}