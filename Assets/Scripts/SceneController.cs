using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    private GameObject _enemy;

    void Update()
    {
        if (null == _enemy)
        {
            _enemy = Instantiate(enemyPrefab) as GameObject;

            _enemy.transform.position = new Vector3(0, 1, 8);
            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0);
        }
    }
}
