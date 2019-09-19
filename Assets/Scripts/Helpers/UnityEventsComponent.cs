using UnityEngine;
using UnityEngine.Events;

public class UnityEventsComponent : MonoBehaviour {

    public UnityEvent OnStartEvent;
    public UnityEvent OnEnableEvent;
    public UnityEvent OnDisableEvent;

    private void Start()
    {
        OnStartEvent.Invoke();
    }

    private void OnEnable()
    {
        OnEnableEvent.Invoke();
    }

    private void OnDisable()
    {
        OnDisableEvent.Invoke();
    }
}
