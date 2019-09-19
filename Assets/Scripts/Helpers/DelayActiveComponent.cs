using UnityEngine;
using UnityEngine.Events;

public class DelayActiveComponent : MonoBehaviour
{
    [SerializeField]
    private bool ScaledToGameSpeed;
    [SerializeField]
    private UnityEvent Activate;

    private float Timer;
    private float Delay;
    private bool Activated;

	public void Active(float delay)
    {
        Activated = false;
        Timer = 0f;
        Delay = delay;
    }

    private void Update()
    {
        if (Activated)
            return;

        if (Timer > Delay)
        {
            Activate.Invoke();
            Activated = true;
        }
        Timer += Time.deltaTime * (ScaledToGameSpeed ? GameController.GameSpeed : 1f);
    }
}