using UnityEngine;
using UnityEngine.Events;

public interface IHitReceiving
{
    public bool AlreadyHitted { get; set; }

    public void HitThis ();
}
