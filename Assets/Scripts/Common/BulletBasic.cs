using UnityEngine;

public class BulletBasic : MonoBehaviour {

    public float Speed = 100f;

	private void Update () {
        transform.position += transform.right * Time.deltaTime * Speed * GameController.GameSpeed;

        if (!GameController.AliveRect.Contains(transform.position))
            DestroyImmediate(gameObject);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealthController enemy = collision.GetComponent<EnemyHealthController>();
        if (enemy != null)
        {
            enemy.Damage();
            Destroy(gameObject);
        }
    }
}
