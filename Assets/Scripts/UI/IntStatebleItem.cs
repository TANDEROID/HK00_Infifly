using UnityEngine;
using UnityEngine.Events;

public class IntStatebleItem : MonoBehaviour
{
    private bool Active;
    private bool Init;

    [SerializeField]
    private UnityEvent OnActive;
    [SerializeField]
    private UnityEvent OnDeactive;
    [SerializeField]
    private UnityEventBool SetState;
    [SerializeField]
    private UnityEventBool SetStateInv;

    public void Set(bool isActive)
    {
        if (Active == isActive && Init)
            return;


        Init = true;
        Active = isActive;
        if (isActive)
            OnActive.Invoke();
        else
            OnDeactive.Invoke();

        SetState.Invoke(isActive);
        SetStateInv.Invoke(!isActive);
    }
}
