using UnityEngine;
using static NewEnemyAI;

public class CoinDropSystem : MonoBehaviour
{
    public GameObject coinPrefab; // Префаб монетки
    private int coinsToDrop = 0;
    private NewEnemyAI enemyAI;

    private void Start()
    {
        enemyAI = GetComponent<NewEnemyAI>();
    }

    public void CoinDrop(EnemyType type)
    {        
        if (enemyAI != null)
        {
            switch (enemyAI.type)
            {
                case EnemyType.Weak:
                    coinsToDrop = 3 + PlayerBonusSystem.Magnet;
                    break;
                case EnemyType.Strong:
                    coinsToDrop = 12 + PlayerBonusSystem.Magnet;
                    break;
                case EnemyType.Boss:
                    coinsToDrop = 0;
                    break;
                default: // Normal
                    coinsToDrop = 8 + PlayerBonusSystem.Magnet;
                    break;
            }
        }
        else
        {
            coinsToDrop = 15; //дроп с чекпоинта
        }
        for (int i = 0; i < coinsToDrop; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(3f, 5f)), ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse);
            }

        }
    }

}
