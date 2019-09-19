using UnityEngine;

public class DestroyItselfByTimer : MonoBehaviour {

    [SerializeField]
    public float Delay = 2f;

    private float Timer;

    private void Awake()
    {
        Timer = 0f;
    }

    private void Update()
    {
        if (Timer >= Delay)
            Destroy(gameObject);

        Timer += Time.deltaTime * GameController.GameSpeed;
    }
}