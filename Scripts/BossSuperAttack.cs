using UnityEngine;

public class BossSuperAttack : MonoBehaviour
{

    public GameObject bullet;
    public Transform bulletPos;

    private float timer;

    private NewEnemyAI enemyAI;

    public SoundManager soundManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAI = GetComponent<NewEnemyAI>();
        soundManager = GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAI.isChasing == true)
        {
            timer += Time.deltaTime;

            if (timer > 3)
            {
                timer = 0;
                BossShooting();
            }
        }
        
    }

    public void BossShooting()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
        soundManager.PlaySound(5);
    }
}
