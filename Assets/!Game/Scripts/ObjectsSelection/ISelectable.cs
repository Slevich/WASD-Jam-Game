using UnityEngine;

public interface ISelectable
{
    public void Select ();
    public Beater ReturnBeater ();
    public void Deselect ();
}
