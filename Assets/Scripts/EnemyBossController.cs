using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    [SerializeField]
    public float hp;

    // �_�E���t���O
    private bool downFlag;
    public void SetDownFlag(bool set) { downFlag = set; }

    // ����\��Flag
    public bool weaponReflect = true;

    // ���V����悤
    private float angle = 0;
    // ���V�X�s�[�h
    private float flySpeed = 2.0f;
    private float flyUp = 2.0f;
    private float flyDown = -2.0f;

    // �{�X�̊J�n�ʒu
    private Vector3 startPosition;

    // ����ɗ���Ƃ��̈ړ��X�s�[�h
    [SerializeField]
    private float moveSpeed;
    // ����ɗ��鋗��
    private float attackRange = 1.0f;

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

    // �� ���F
    public GameObject sickleThrowing;

    // ����̉~
    public GameObject sickleJudgeImage;

    // ����ʐ������̋�����
    private float sickleRange = 4;
    private float interval = 0.6f;
    private float generationTime = 0.0f;
    private int m_count = 0;

    // �ˌ����Ă�����̓����Ȃ�����
    private float attackEndTimer = 0;
    private float attackEndTimerMax = 5;

    // ���̈ʒu�ɖ߂�
    private float totalTime = 2.0f;
    private float backTimer = 0.0f;
    private Vector3 backStartPosition;

    // �����肵�ɗ���Ƃ��p
    private float slashAngle = 0.0f;
    public float GetRadianSlashAngle() { return slashAngle * Mathf.Deg2Rad; }
    public float GetSlashAngle() { return slashAngle; }

    private Animator m_animator;
    private static readonly int hashAttack = Animator.StringToHash("Attack");
    private static readonly int hashSickleAttack = Animator.StringToHash("Attack2");
    private static readonly int hashSickleAttackBerserker = Animator.StringToHash("Attack3");
    private static readonly int hashDown = Animator.StringToHash("Down");
    private static readonly int hashRevival = Animator.StringToHash("Revival");
    private static readonly int hashSpeed = Animator.StringToHash("velocity");

    // �{�X�_�E�����
    //private bool m_down = false;

    [SerializeField]
    private float downTimeMax; // �_�E������
    private float downCountTimer = 0.0f;
    private float downEasingTimer = 0.0f; // �_�E�����̂̂����΂�
    private Vector3 downPosition; // �m�b�N�o�b�N�ʒu
    private Vector3 downStartPosition; // �m�b�N�o�b�N�J�n�ʒu

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
    public enum State
    {
        Idle, // �ҋ@
        SickleAttack, // ����������Ă��� 
        SickleAttackBerserker, // ��ʂɓ����Ă�����
        AssaultAttack, // ����ɗ�����
        AssaultAttackAnim, // ����ɂ��đҋ@����
        BossComeBack, // �����̂��Ƃ̈ʒu�ɖ߂�
        Down,  // �{�X�_�E��
        Death, // ���S
    }
    private State state = State.Idle;
    public State GetState() { return state; }

    // Start is called before the first frame update
    void Start()
    {
        ConditionIdleState();

        m_animator = GetComponent<Animator>();
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
            case State.BossComeBack: ConditionBossComeBackUpdate();  break;
            // �{�X�_�E�����
            case State.Down: ConditionDownUpdate( ); break;
        }
        Debug.Log(state);
      
        
    }

    private void ConditionIdleState()
    {
        state = State.Idle;
        idleTimer = idleTimerMax;

        weaponReflect = true;

    }

    private void ConditionIdleUpdate()
    {
        weaponReflect = true;

        // ���Ԃ��o�߂����犙�𓊂���
        if (idleTimer < 0)
        {
            //ConditionAssaultAttackState();
            int test = Random.Range(0, 3);
            if(test == 0) ConditionSickleAttackState();
            if (test == 1) ConditionSickleAttackBerserkerState();
            if (test == 2) ConditionAssaultAttackState();
        }

        if (angle > Mathf.PI) flySpeed = flyDown;
        if (angle < -Mathf.PI) flySpeed = flyUp;

        angle += flySpeed * Mathf.PI * Time.deltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y + angle * Time.deltaTime, transform.position.z);

        idleTimer -= Time.deltaTime;
    }

    private void ConditionSickleAttackState()
    {
        state = State.SickleAttack;
        kariTimer = 1.0f;

        //�����C���X�^���X������(��������)
        GameObject child = GameObject.FindWithTag("Dumy");//transform.Find("ThrowingKama").gameObject;

        GameObject sl = Instantiate(sickleThrowing, child.transform.position, child.transform.rotation);
        //GameObject sl = Instantiate(sickleThrowing, new Vector3(0,0,0), Quaternion.identity, null);
        //sl.transform.position = this.transform.TransformPoint(child.transform.localPosition);,
        sl.transform.position =child.transform.position;

    
        var attack = sickleJudgeImage.GetComponent<CreatePointerController>();
        attack.CreateTargetPointer(sl);

        weaponReflect = false;

        m_animator.SetTrigger(hashSickleAttack);
    }

    private void ConditionSickleAttackUpdate()
    {
        // ���Ԃ��o�߂�����ҋ@�ɖ߂�
        if (kariTimer < 0) //0.05f && kariTimer > -0.05f) 
        {
            ConditionIdleState();
            m_animator.SetTrigger("Idle");
            return;
        }
        // if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("standby"))
        // {
        //     ConditionIdleState();
        //     return;
        // }

        kariTimer -= Time.deltaTime;
    }

    private void ConditionSickleAttackBerserkerState()
    {
        state = State.SickleAttackBerserker;

        kariTimer = 3.0f;

        m_count = 0;

        // �����쐬
        directions.right = transform.right;
        directions.left = -transform.right;
        directions.top = new Vector3(0, 1, 0);
        directions.topRight = (directions.top + directions.right) / 2;
        directions.topLeft = (directions.top + directions.left) / 2;

        weaponReflect = false;

        generationTime = 0.0f;


        m_animator.SetTrigger(hashSickleAttackBerserker);
    }

    private void ConditionSickleAttackBerserkerUpdate()
    {
        //�����C���X�^���X������(��������)�Ƃ肠�������艟��
        if (generationTime > interval)
        {
            SetSickle(m_count);
            generationTime = 0.0f;
            m_count++;
        }
        


        // ���Ԃ��o�߂�����ҋ@�ɖ߂�
        if (kariTimer < 0) //0.05f && kariTimer > -0.05f) 
        {
            ConditionIdleState();
            m_animator.SetTrigger("Idle");

            return;
        }
    

        kariTimer -= Time.deltaTime;
        generationTime += Time.deltaTime;
    }

    // ��������ꏊ
    private void SetSickle(int count)
    {
        

        switch(count)
        {
            case 2: // �E
                GameObject sl = Instantiate(sickle);
                sl.transform.position = transform.position + (directions.right * sickleRange);
                sl.transform.eulerAngles = new Vector3(70, -90, -20);

                var attack = sickleJudgeImage.GetComponent<CreatePointerController>();
                attack.CreateTargetPointer(sl);

                GameObject sl5 = Instantiate(sickle);
                sl5.transform.position = transform.position + (directions.left * sickleRange);
                sl5.transform.eulerAngles = new Vector3(-70, -90, 20);

                var attack5 = sickleJudgeImage.GetComponent<CreatePointerController>();
                attack5.CreateTargetPointer(sl5);
                break;

            case 1: // �E�΂ߏ�
                GameObject sl2 = Instantiate(sickle);
                sl2.transform.position = transform.position + (directions.topRight * (sickleRange + 1));
                sl2.transform.eulerAngles = new Vector3(30, -90, -20);

                var attack2 = sickleJudgeImage.GetComponent<CreatePointerController>();
                attack2.CreateTargetPointer(sl2);

                GameObject sl4 = Instantiate(sickle);
                sl4.transform.position = transform.position + (directions.topLeft * (sickleRange + 1));
                sl4.transform.eulerAngles = new Vector3(-30, -90, -20);

                var attack4 = sickleJudgeImage.GetComponent<CreatePointerController>();
                attack4.CreateTargetPointer(sl4);

                break;

            case 0: // ��
                GameObject sl3 = Instantiate(sickle);
                sl3.transform.position = transform.position + (directions.top * sickleRange);
                sl3.transform.eulerAngles = new Vector3(0, -90, -20);

                var attack3 = sickleJudgeImage.GetComponent<CreatePointerController>();
                attack3.CreateTargetPointer(sl3);

                break;

            case 3: // ���΂ߏ�
                //GameObject sl4 = Instantiate(sickle);
                //sl4.transform.position = transform.position + (directions.topLeft * (sickleRange+ 1));
                //sl4.transform.eulerAngles = new Vector3(-30, -90, -20);

                //var attack4 = sickleJudgeImage.GetComponent<CreatePointerController>();
                //attack4.CreateTargetPointer(sl4);
                break;

            case 4: // ��
                //GameObject sl5 = Instantiate(sickle);
                //sl5.transform.position = transform.position + (directions.left * sickleRange);
                //sl5.transform.eulerAngles = new Vector3(-70, -90, 20);

                //var attack5 = sickleJudgeImage.GetComponent<CreatePointerController>();
                //attack5.CreateTargetPointer(sl5);
                break;
        }
    }



    private void ConditionAssaultAttackState()
    {
        state = State.AssaultAttack;
        startPosition = transform.position;

    }
    private void ConditionAssaultAttackUpdate()
    {
        m_animator.SetFloat(hashSpeed, 0.5f);

        // �v���C���[�Ɍ���������
        var dir = player.transform.position - transform.position;
        float lengthSq = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;

        if (attackRange * attackRange > lengthSq)
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

        slashAngle = Random.Range(0.0f, 360.0f);

        m_animator.SetTrigger(hashAttack);
    }

    private void ConditionAssaultAttackAnimUpdate()
    {
        if (downFlag)
        {
            ConditionDownState();
            return;
        }

        if (attackEndTimer < 0)
        {
            ConditionBossComeBackState();
        }

        attackEndTimer -= Time.deltaTime;
    }

    private void ConditionBossComeBackState()
    {
        state = State.BossComeBack;
        backTimer = 0.0f;
        backStartPosition = transform.position;

        m_animator.SetTrigger(hashRevival);
    }
    private void ConditionBossComeBackUpdate()
    {
        // �A��ړI�n�̌���
        var pos = Easing.SineInOut(backTimer / 1.0f, totalTime, backStartPosition, startPosition);

        transform.position = new Vector3(pos.x, pos.y, pos.z);

        var vec = startPosition - pos;
        float length = Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
        if (length < 0.01f)
        {
            ConditionIdleState();
            m_animator.SetTrigger("Idle");
        }
        backTimer += Time.deltaTime;
    }

    private void ConditionDownState()
    {
        downFlag = false;

        state = State.Down;
        downEasingTimer = 0.0f;
        downCountTimer = 0.0f;

        downStartPosition = transform.position;
        downPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, player.transform.position.z + 6);

        m_animator.SetTrigger(hashDown);
    }
    private void ConditionDownUpdate()
    {
        // �_�E�����ԉz�����猳�̏ꏊ�ɖ߂�
        if (downCountTimer > downTimeMax) ConditionBossComeBackState();

        // �m�b�N�o�b�N
        transform.position = Easing.SineInOut(downEasingTimer, 1.0f, downStartPosition, downPosition);

        // ���Ԍv�Z
        downCountTimer += Time.deltaTime;
        downEasingTimer += Time.deltaTime;

        if (downEasingTimer > 1.0f) downEasingTimer = 1.0f;
    }
}
