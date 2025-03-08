using System;
using UnityEngine;

public interface IInteractable
{
    public event Action OnDone;
    public void OnInteract(Transform other);
    public bool GetIsBusy();
}
