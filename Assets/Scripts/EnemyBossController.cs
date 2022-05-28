using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{

    [SerializeField] private GameObject DamageEffectPrefab;

    private float hp;
    public void SetHP(float hp_)
    {
        hp = hp_;
    }
    [SerializeField]
    private BossHPController bossHpController;
    public void OnDamaged(float damage_)
    {
        bossHpController.OnDamaged(damage_);
    }

    public float GetHP()
    {
        return hp;
    }


    [SerializeField] public float m_damage;


    // ���� 
    protected float m_turnAngle = 1.0f;
    protected float m_turnSpeed = 3.0f;

    // �_�E���t���O
    private bool downFlag;
    public void SetDownFlag(bool set) { downFlag = set; }

    // ���S�t���O
    private bool deathFlag;
    public void SetDeathFlag() { deathFlag = true; }
    public bool GetDeathFlag() { return deathFlag; }
    [SerializeField] private float deathAnimationTime;
    private float deathAnimationTimer;


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
    private static readonly int hashSpeed = Animator.StringToHash("Zensin");

    private bool attackThrowing = false;
    public bool GetAttackThrowing() { return attackThrowing; }

    private bool attackB = false;
    public bool GetAttackBar() { return attackB; }

    [SerializeField]
    private float hpMax;

    public float GetMaxHP() { return hpMax; }

    private int idleCount = 0;


    private Vector3[] targetPositions =
    {
        new Vector3(0f,0.05f, 17.118f),
        new Vector3(0f, 0.73f, 14.57f),
        new Vector3(0f, 1.737f, 9.789f),
        new Vector3(0f, 1.9f, 9.789f),
        new Vector3(0f, -0.5f, 5.4599f)
    };
    private float[] easingTimers =
    {
        2f,
        3f,
        2f,
        1f,
        1f
    };
    private Vector3 registerPosition;
    int batleStartCount = 0;
    int batleStartCountMax = 5;
    float easingTimer = 0.0f;

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
        BatleStart    // �o�g���J�n�O
    }
    private State state = State.Idle;
    public State GetState() { return state; }

    // Start is called before the first frame update
    void Start()
    {
        hp = hpMax;
        ConditionBatleStartState();

        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0) deathFlag = true;

        switch (state)
        {
            // �ҋ@
            case State.Idle: ConditionIdleUpdate(); break;
            // ������
            case State.SickleAttack: ConditionSickleAttackUpdate(); break;
            // ����ʓ���
            case State.SickleAttackBerserker: ConditionSickleAttackBerserkerUpdate(); break;
            // �����Ă���
            case State.AssaultAttack: ConditionAssaultAttackUpdate(); break;
            // �����Ă����Ƃ��ɏ����ҋ@���Ԃ�����
            case State.AssaultAttackAnim: ConditionAssaultAttackAnimUpdate(); break;
            // �����̂��Ƃ̈ʒu�ɖ߂�
            case State.BossComeBack: ConditionBossComeBackUpdate(); break;
            // �{�X�_�E�����
            case State.Down: ConditionDownUpdate(); break;
            // ���S
            case State.Death: ConditionDeathUpdate(); break;
            // �퓬�O
            case State.BatleStart: ConditionBatleStartUpdate(); break;
        }
        Debug.Log(state);

    }

    private void ConditionIdleState()
    {
        state = State.Idle;
        idleTimer = idleTimerMax;

        weaponReflect = true;

        idleCount++;
    }

    private void ConditionIdleUpdate()
    {
        float oldAngle = angle;

        var rotateCube = transform.parent.GetComponent<RotateCubeController>();

        if (idleCount > 2)
        {
            idleCount = 0;
            var paretState = rotateCube.GetState();
            if (paretState == RotateCubeController.State.Right) rotateCube.CentralSet();
            if (paretState == RotateCubeController.State.Left) rotateCube.CentralSet();
            if (paretState == RotateCubeController.State.Central)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0) rotateCube.RightSet();
                if (rand == 1) rotateCube.LeftSet();
            }
        }

        weaponReflect = true;

        // ���Ԃ��o�߂����犙�𓊂���
        if (idleTimer < 0)
        {
            if (rotateCube.GetRotateCheck())
            {
                if (hp < hpMax / 2)
                {
                    int test = Random.Range(0, 3);
                    if (test == 0) ConditionSickleAttackState();
                    if (test == 1) ConditionSickleAttackBerserkerState();
                    if (test == 2) ConditionAssaultAttackState();
                }
                else
                {
                    int test = Random.Range(0, 2);
                    if (test == 0) ConditionSickleAttackState();
                    if (test == 1) ConditionAssaultAttackState();
                }
            }
        }

        if (angle > Mathf.PI) flySpeed = flyDown;
        if (angle < -Mathf.PI) flySpeed = flyUp;


        angle += flySpeed * Mathf.PI * Time.deltaTime;

        float angleLerp = Mathf.Lerp(oldAngle, angle, 0.01f);

        transform.position = new Vector3(transform.position.x, transform.position.y + angleLerp * Time.deltaTime, transform.position.z);

        idleTimer -= Time.deltaTime;
    }

    private void ConditionSickleAttackState()
    {
        state = State.SickleAttack;
        kariTimer = 4.0f;

        //�����C���X�^���X������(��������)
        GameObject child = GameObject.FindWithTag("Dumy");//transform.Find("ThrowingKama").gameObject;

        //GameObject sl = Instantiate(sickleThrowing, child.transform.position, child.transform.rotation);
        GameObject[] sickles = GameObject.FindGameObjectsWithTag("SickleThrowing");

        //GameObject sl = Instantiate(sickleThrowing, new Vector3(0,0,0), Quaternion.identity, null);
        //sl.transform.position = this.transform.TransformPoint(child.transform.localPosition);,
        sickles[0].transform.position = child.transform.position;
        var throwing = sickles[0].GetComponent<SickleThrowingController>();
        throwing.Initilize();

        //var attack = sickleJudgeImage.GetComponent<CreatePointerController>();
        //attack.CreateTargetPointer(sickles[0]);

        weaponReflect = false;

        m_animator.SetTrigger(hashSickleAttack);

        attackThrowing = true;

    }

    private void ConditionSickleAttackUpdate()
    {
        // ���Ԃ��o�߂�����ҋ@�ɖ߂�
        if (kariTimer < 0) //0.05f && kariTimer > -0.05f) 
        {
            ConditionIdleState();
            m_animator.SetTrigger("Idle");
            attackThrowing = false;
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

        kariTimer = 9.0f;

        m_count = 0;

        // �����쐬
        directions.right = transform.right;
        directions.left = -transform.right;
        directions.top = new Vector3(0, 1, 0);
        directions.topRight = (directions.top + directions.right) / 2;
        directions.topLeft = (directions.top + directions.left) / 2;

        weaponReflect = false;

        generationTime = 0.0f;

        attackB = true;

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
            attackB = false;
            return;
        }


        kariTimer -= Time.deltaTime;
        generationTime += Time.deltaTime;
    }

    // ��������ꏊ
    private void SetSickle(int count)
    {
        GameObject[] sickles = GameObject.FindGameObjectsWithTag("Sickle");


        switch (count)
        {
            case 2: // �E
                //GameObject sl = Instantiate(sickle);
                sickles[0].transform.position = transform.position + (directions.right * sickleRange);
                sickles[0].transform.localEulerAngles = new Vector3(70, 90, -20);
                var controller = sickles[0].GetComponent<SickleController>();
                controller.Initilize(5.25f);


                //var attack = sickleJudgeImage.GetComponent<CreatePointerController>();
                //attack.CreateTargetPointer(sickles[0]);

                // GameObject sl5 = Instantiate(sickle);
                sickles[1].transform.position = transform.position + (directions.left * sickleRange);
                sickles[1].transform.localEulerAngles = new Vector3(-70, 90, 20);
                var controller2 = sickles[1].GetComponent<SickleController>();
                controller2.Initilize(4.5f);

                //var attack5 = sickleJudgeImage.GetComponent<CreatePointerController>();
                //attack5.CreateTargetPointer(sickles[1]);
                break;

            case 1: // �E�΂ߏ�
                //GameObject sl2 = Instantiate(sickle);
                sickles[2].transform.position = transform.position + (directions.topRight * (sickleRange + 1));
                sickles[2].transform.localEulerAngles = new Vector3(30, 90, -20);
                var controller3 = sickles[2].GetComponent<SickleController>();
                controller3.Initilize(3.75f);

                //var attack2 = sickleJudgeImage.GetComponent<CreatePointerController>();
                //attack2.CreateTargetPointer(sickles[2]);

                //GameObject sl4 = Instantiate(sickle);
                sickles[3].transform.position = transform.position + (directions.topLeft * (sickleRange + 1));
                sickles[3].transform.localEulerAngles = new Vector3(-30, 90, -20);
                var controller4 = sickles[3].GetComponent<SickleController>();
                controller4.Initilize(3.0f);

                //var attack4 = sickleJudgeImage.GetComponent<CreatePointerController>();
                //attack4.CreateTargetPointer(sickles[3]);
                break;

            case 0: // ��
                //GameObject sl3 = Instantiate(sickle);
                sickles[4].transform.position = transform.position + (directions.top * sickleRange);
                sickles[4].transform.localEulerAngles = new Vector3(0, 90, -20);
                var controller5 = sickles[4].GetComponent<SickleController>();
                controller5.Initilize(2.0f);

                //var attack3 = sickleJudgeImage.GetComponent<CreatePointerController>();
                //attack3.CreateTargetPointer(sickles[4]);
                break;
        }
    }



    private void ConditionAssaultAttackState()
    {
        state = State.AssaultAttack;
        startPosition = transform.position;
        m_animator.SetTrigger(hashSpeed);

    }
    private void ConditionAssaultAttackUpdate()
    {
        if (deathFlag)
        {
            ConditionDeathState();
            return;
        }
        if (downFlag)
        {
            ConditionDownState();
            return;
        }

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
        if (deathFlag)
        {
            ConditionDeathState();
            return;
        }
        // �a��A�j���[�V�����������܂Ői�񂾂Ƃ�
        if (downFlag && attackEndTimer > attackEndTimerMax / 2.0f)
        {
            ConditionDownState();
            return;
        }

        if (attackEndTimer < 0)
        {
            ConditionBossComeBackState();
            // �v���C���[�Ƀ_���[�W��^����
            GameObject
                .FindGameObjectWithTag("Player")
                .GetComponent<PlayerAutoControl>()
                .OnDamage(m_damage);
            AttackEffect(DamageEffect.DamageEffectType.Sickle);
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
        if (deathFlag)
        {
            ConditionDeathState();
            return;
        }


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
        //downPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, player.transform.position.z + 6);
        downPosition = transform.position +
                       new Vector3(0.0f, 0.5f, 0.0f) +
                       transform.forward * -1.0f * 3;

        m_animator.SetTrigger(hashDown);
    }
    private void ConditionDownUpdate()
    {
        if (deathFlag)
        {
            ConditionDeathState();
            return;
        }


        // �_�E�����ԉz�����猳�̏ꏊ�ɖ߂�
        if (downCountTimer > downTimeMax) ConditionBossComeBackState();

        // �m�b�N�o�b�N
        transform.position = Easing.SineInOut(downEasingTimer, 1.0f, downStartPosition, downPosition);

        // ���Ԍv�Z
        downCountTimer += Time.deltaTime;
        downEasingTimer += Time.deltaTime;

        if (downEasingTimer > 1.0f) downEasingTimer = 1.0f;
    }

    private void ConditionDeathState()
    {
        state = State.Death;

        m_animator.SetTrigger("Death");
        deathAnimationTimer = deathAnimationTime;
    }
    private void ConditionDeathUpdate()
    {
        deathAnimationTimer -= Time.deltaTime;

        if (deathAnimationTimer > 0.0f) return;

        GetComponent<ChangeScene>().Change();
    }

    //  �o�g���J�n�O
    private void ConditionBatleStartState()
    {
        state = State.BatleStart;
        easingTimer = 0.0f;
        registerPosition = batleStartCount == 0 ? transform.localPosition : targetPositions[batleStartCount - 1];
    }
    private void ConditionBatleStartUpdate()
    {
        var oldLocalPositon = transform.localPosition;

        var curPosition = Easing.SineInOut(easingTimer, easingTimers[batleStartCount], registerPosition, targetPositions[batleStartCount]);

        transform.localPosition = Vector3.Lerp(oldLocalPositon, curPosition, 0.7f);

        if (batleStartCount > 0)
        {
            var dir = player.transform.position - transform.position;
            dir.y = 0f;
            Turn(dir);

        }

        if (easingTimer > easingTimers[batleStartCount])
        {
            batleStartCount++;

            if (batleStartCount >= batleStartCountMax)
            {
                ConditionIdleState();
                return;
            }

            ConditionBatleStartState();
        }


        easingTimer += Time.deltaTime;
    }

    protected bool Turn(Vector3 vec)
    {
        bool check = false;

        // �����̌����Ă����������G�̕����܂ł̊p�x���Z�o����
        var dir = transform.forward;
        dir.y = vec.y = 0.0f;
        float angle = Vector3.Angle(dir, vec);
        // �p�x�����ȏ�̏ꍇ��OK
        if (angle < m_turnAngle)
        {
            check = true;
        }

        var rotate = Quaternion.LookRotation(vec);
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rotate, 0.01f);

        return check;
    }

    void AttackEffect(DamageEffect.DamageEffectType type_)
    {
        if (DamageEffectPrefab == null) return;

        var damageEffect = Instantiate(DamageEffectPrefab);

        damageEffect
            ?.GetComponent<DamageEffect>()
            .SelectRenderDamageEffect(type_, transform.position);

        var canvas = GameObject.Find("Canvas")?.transform;
        if (canvas == null) return;

        damageEffect.transform.SetParent(canvas);
    }
}
