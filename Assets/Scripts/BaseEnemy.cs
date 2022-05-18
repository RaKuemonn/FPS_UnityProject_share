using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseEnemy : MonoBehaviour
{
    // �e���Őݒ�
    protected float m_hp;

    // �o�g���G���A�ɓ�������
    protected bool m_enter_battle_area;

    // BattleArea�̃R�[���o�b�N�֐�
    public event EventHandler OnDeadEvent;


    // ���� 
    protected float m_turnAngle = 1.0f;
    protected float m_turnSpeed = 3.0f;

    // �ړI�n
    protected Vector3 m_targetPosition;
    protected Vector3 m_territoryOrigin;
    protected float m_range = 5.0f;

    public bool IsDeath { set; get; } = false;

// �W��
    protected bool m_assemblyFlag = false;

    // �퓬�J�n
    protected bool m_battleFlag = false;

    protected Vector3 m_locationPosition;

    public void SetAssemblyFlag(bool set)
    {
        m_assemblyFlag = set;
    }

    public void SetBattleFlag(bool set)
    {
        m_battleFlag = set;
    }

    public void SetLocationPosition(Vector3 pos)
    {
        m_locationPosition = pos;
    }

    // �^�[�Q�b�g�ʒu�������_���ݒ�
    protected void SetRandamTargetPosition()
    {
        float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2) - Mathf.PI;
        float range = UnityEngine.Random.Range(0f, m_range);
        m_targetPosition.x = m_territoryOrigin.x + Mathf.Sin(theta) * range;
        m_targetPosition.y = m_territoryOrigin.y;
        m_targetPosition.z = m_territoryOrigin.z + Mathf.Cos(theta) * range;
    }

    protected void TurnRotation(Vector3 vec)
    {
        // �����̌����Ă����������G�̕����܂ł̊p�x���Z�o����
        var dir = transform.forward;
        dir.y = vec.y = 0.0f;
        dir.Normalize();
        vec.Normalize();
        float angle = Vector3.Angle(dir, vec);
        // �p�x�����ȏ�̏ꍇ�͐��񂷂�
        if (angle > m_turnAngle)
        {
            Quaternion rotation = Quaternion.LookRotation(vec);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, m_turnSpeed * Time.deltaTime);
            Debug.Log(transform.rotation);
        }
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

    // EnemyController����R�s�y���Ď����Ă���
    protected void CreateCollideOnCanvas()
    {
        // �a��ꂽ�Ƃ��ɐ������ꂽ�I�u�W�F�N�g�łȂ���΁iSetCutPerformance()�ŁA�ύX����Ă��Ȃ���΁j
        //if (is_create_collide == false) return;

        // �����蔻��p�̃I�u�W�F�N�g��Canvas���ɐ���
        GameObject obj = Instantiate(
            (GameObject)Resources.Load("EnemyCollideOnScreen")
        );
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.GetComponent<EnemyCollide>().SetTarget(gameObject);
    }

    // �퓬�G���A�ɓ�������R�[���o�b�N�����
    public void OnEnterBattleArea()
    {
        m_enter_battle_area = true;
    }



    ///
    ///
    /// �a��ꂽ�Ƃ��ɌĂ΂��(�蓮�ŏ�������ŌĂяo���Ă�)�֐�
    ///
    /// �قځB���S�����Ȃ̂ŁA�������������
    /// 
    /// 
    public void OnCutted(Vector3 impulse_)
    {

        var rigidbody = GetComponent<Rigidbody>();

        // rigidbody�v���p�e�B�̕ύX
        {
            // �d�͂ɏ]��
            rigidbody.useGravity = true;

            // �S���𖳂���
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        // �e����΂�
        rigidbody.AddForce(impulse_, ForceMode.Impulse);

        // �j������Œ莞��
        const float const_destroy_time = 0.5f;
        Destroy(gameObject, const_destroy_time);
    }

    void OnDestroy()
    {
        OnDead();
    }

    // ���S���� (private)
    private void OnDead()
    {
        OnDeadEvent?.Invoke(this,EventArgs.Empty);
    }
}
