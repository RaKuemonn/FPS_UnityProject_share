using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerCreateEnemy : MonoBehaviour
{
    // ��������ꏊ
    [SerializeField] private Transform Enemies;

    // �������鐔
    public int m_createSnakeNum;
    public GameObject m_snake;

    public int m_createDragonNum;
    public GameObject m_dragon;

    public int m_createBatNum;
    public GameObject m_bat;


    public float m_createLength;

    // �������݂ē��Ԋu�ɐ������邽��
    public float m_widthSize;

    private int m_enemyNum;

    // Start is called before the first frame update
    void Start()
    {
        m_enemyNum += m_createSnakeNum;
        m_enemyNum += m_createDragonNum;
        m_enemyNum += m_createBatNum;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var objectWidth = m_widthSize + m_widthSize;
            var widthInterval = objectWidth / (m_enemyNum + 1);

            var leftPosX = transform.position.x - m_widthSize;

            float createPosX = widthInterval;

            // �R�E����
            for (int i = 0; i < m_createBatNum; i++)
            {
                GameObject bat = Instantiate(m_bat);
                SetParentToEnemies(bat);
                bat.transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z + m_createLength);
                EnemyBatController batController = bat.GetComponent<EnemyBatController>();

                Vector3 pos = new Vector3(createPosX + leftPosX, transform.position.y, transform.position.z + m_createLength);
                createPosX += widthInterval;

                batController.SetLocationPosition(pos);
            }

            // �h���S��
            for (int i = 0; i < m_createDragonNum; i++)
            {
                GameObject dragon = Instantiate(m_dragon);
                SetParentToEnemies(dragon);
                dragon.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + m_createLength);

                EnemyDragonController dragonController = dragon.GetComponent<EnemyDragonController>();

                Vector3 pos = new Vector3(createPosX + leftPosX, transform.position.y, transform.position.z + m_createLength);
                createPosX += widthInterval;

                dragonController.SetLocationPosition(pos);
            }

            // ��
            for (int i = 0; i < m_createSnakeNum; i++)
            {
                GameObject snake = Instantiate(m_snake);
                SetParentToEnemies(snake);
                snake.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + m_createLength);

                EnemySnakeController snakeController = snake.GetComponent<EnemySnakeController>();

                Vector3 pos = new Vector3(createPosX + leftPosX, transform.position.y, transform.position.z + m_createLength);
                createPosX += widthInterval;

                snakeController.SetLocationPosition(pos);
            }

            Destroy(gameObject);
        }
    }

    private void SetParentToEnemies(GameObject object_)
    {
        if(Enemies == null) return;

        object_.transform.SetParent(Enemies.transform);
    }
}
