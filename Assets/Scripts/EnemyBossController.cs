using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    // ���V����悤
    private float angle = 0;
    // ���V�X�s�[�h
    private float flySpeed = 2.0f;

    // �{�X�̊J�n�ʒu
    [SerializeField]
    private Vector3 startPosition;

    // ����ɗ���Ƃ��̈ړ��X�s�[�h
    [SerializeField]
    private float moveSpeed;
    // ����ɗ��鋗��
    private float attackRange = 3.0f;

    // �ҋ@����
    [SerializeField]
    private float idleTimerMax;
    private float idleTimer = 0.0f;

    // ��
    private float kariTimer = 3.0f;

    //�v���C���[�ʒu�ق���
    [SerializeField]
    private GameObject player;

    // ��
    public GameObject sickle;

    // ����ʐ������̋�����
    private float sickleRange = 4;

    // �ˌ����Ă�����̓����Ȃ�����
    private float attackEndTimer = 0;
    private float attackEndTimerMax = 5;

    // boss�̎���
    private struct Directions
    {
        public Vector3 right;
        public Vector3 left;
        public Vector3 top;
        public Vector3 topLeft;
        public Vector3 topRight;
    }
    private Directions directions;

    // �{�X�X�e�[�g
    enum State
    {
        Idle, // �ҋ@
        SickleAttack, // ����������Ă��� 
        SickleAttackBerserker, // ��ʂɓ����Ă�����
        AssaultAttack, // ����ɗ�����
        AssaultAttackAnim, // ����ɂ��đҋ@����
        BossComeBack, // �����̂��Ƃ̈ʒu�ɖ߂�
    }
    State state = State.Idle;

    // Start is called before the first frame update
    void Start()
    {
        ConditionIdleState();
    }  

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            // �ҋ@
            case State.Idle: ConditionIdleUpdate();  break;
            // ������
            case State.SickleAttack: ConditionSickleAttackUpdate(); break;
            // ����ʓ���
            case State.SickleAttackBerserker: ConditionSickleAttackBerserkerUpdate(); break;
            // �����Ă���
            case State.AssaultAttack: ConditionAssaultAttackUpdate(); break;
            // �����Ă����Ƃ��ɏ����ҋ@���Ԃ�����
            case State.AssaultAttackAnim: ConditionAssaultAttackAnimUpdate(); break;
            // �����̂��Ƃ̈ʒu�ɖ߂�
            case State.BossComeBack: break;
        }
        Debug.Log(state);
      
        
    }

    private void ConditionIdleState()
    {
        state = State.Idle;
        idleTimer = idleTimerMax;
    }

    private void ConditionIdleUpdate()
    {
        // ���Ԃ��o�߂����犙�𓊂���
        if (idleTimer < 0)
        {
            int test = Random.Range(0, 1);
            if(test == 0) ConditionSickleAttackState();
            if (test == 1) ConditionSickleAttackBerserkerState();
        }

        if (angle > Mathf.PI) flySpeed = -flySpeed;
        if (angle < -Mathf.PI) flySpeed = -flySpeed;

        angle += flySpeed * Mathf.PI * Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y + angle * Time.deltaTime, transform.position.z);

        idleTimer -= Time.deltaTime;
    }

    private void ConditionSickleAttackState()
    {
        state = State.SickleAttack;
        kariTimer = 10.0f;

        //�����C���X�^���X������(��������)
        GameObject sl = Instantiate(sickle);
        sl.transform.position =
            new Vector3(transform.position.x,
            transform.position.y, transform.position.z);
      
    }

    private void ConditionSickleAttackUpdate()
    {
        // ���Ԃ��o�߂�����ҋ@�ɖ߂�
        if (kariTimer < 0) 
            ConditionIdleState();

        kariTimer -= Time.deltaTime;
    }

    private void ConditionSickleAttackBerserkerState()
    {
        state = State.SickleAttackBerserker;

        kariTimer = 10.0f;

        // �����쐬
        directions.right = transform.right;
        directions.left = -transform.right;
        directions.top = new Vector3(0, 1, 0);
        directions.topRight = (directions.top + directions.right) / 2;
        directions.topLeft = (directions.top + directions.left) / 2;

        //�����C���X�^���X������(��������)�Ƃ肠�������艟��
        {
            GameObject sl = Instantiate(sickle);
            sl.transform.position = transform.position + (directions.right * sickleRange);
            GameObject sl2 = Instantiate(sickle);
            sl2.transform.position = transform.position + (directions.topRight * sickleRange);
            GameObject sl3 = Instantiate(sickle);
            sl3.transform.position = transform.position + (directions.top * sickleRange);
            GameObject sl4 = Instantiate(sickle);
            sl4.transform.position = transform.position + (directions.topLeft * sickleRange); 
            GameObject sl5 = Instantiate(sickle);
            sl5.transform.position = transform.position + (directions.left * sickleRange);
        }
    }

    private void ConditionSickleAttackBerserkerUpdate()
    {
        // ���Ԃ��o�߂�����ҋ@�ɖ߂�
        if (kariTimer < 0)
            ConditionIdleState();

        kariTimer -= Time.deltaTime;
    }

    private void ConditionAssaultAttackState()
    {
        state = State.AssaultAttack;
    }
    private void ConditionAssaultAttackUpdate()
    {
        // �v���C���[�Ɍ���������
        var dir = player.transform.position - transform.position;
        float lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;

        if (attackRange * attackRange < lengthSq)
        {
            ConditionAssaultAttackAnimState();
            return;
        }

        // �m�[�}���C�Y����
        dir.Normalize();
        
        // �ړ��X�s�[�h���|����
        dir *= moveSpeed * Time.deltaTime;

        dir += transform.position;

        // �ړ���
        transform.position = new Vector3(dir.x, dir.y, dir.z);
    }

    private void ConditionAssaultAttackAnimState()
    {
        state = State.AssaultAttackAnim;
        attackEndTimer = attackEndTimerMax;
    }

    private void ConditionAssaultAttackAnimUpdate()
    {
        if (attackEndTimer < 0)
        {
            ConditionBossComeBackState();
        }

        attackEndTimer -= Time.deltaTime;
    }

    private void ConditionBossComeBackState()
    {
        state = State.BossComeBack;
    }
    private void ConditionBossComeBackUpdate()
    {
        // �A��ړI�n�̌���
        var dir = startPosition - transform.position;
    }
}
