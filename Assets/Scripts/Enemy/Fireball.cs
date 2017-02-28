using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    private float speed = 8.0f;
    [SerializeField]
    private int damage = 10;

    void Update()
    {
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        ReactiveTarget enemy = other.GetComponent<ReactiveTarget>();

        if (null != player)
        {
            Debug.Log("Player hit!");
            player.Hurt(damage);
        }
        else if (null != enemy)
        {
            Debug.Log("Enemy die!");
            enemy.ReactToHit();

            Messenger.Broadcast(GameEvent.ENEMY_HIT);
        }

        Destroy(this.gameObject);
    }
}
