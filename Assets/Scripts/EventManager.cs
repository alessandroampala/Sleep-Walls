using UnityEngine.Events;

public static class EventManager
{
    public static readonly UnityEvent TileFade = new UnityEvent();
    public static readonly UnityEvent TileAppear = new UnityEvent();
    public static readonly UnityEvent ToMenu = new UnityEvent();
    public static readonly UnityEvent ToGame = new UnityEvent();
    public static readonly UnityEvent Death = new UnityEvent();
}
