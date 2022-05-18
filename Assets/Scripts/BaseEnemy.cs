using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    // �e���Őݒ�
    [SerializeField]
    protected float m_hp;
    public float GetHP() { return m_hp; }
    public void SetHP(float hp) { m_hp = hp; }

    // ����
    protected float m_turnAngle = 1.0f;
    protected float m_turnSpeed = 3.0f;

    // �ړI�n
    protected Vector3 m_targetPosition;
    protected Vector3 m_territoryOrigin;
    protected float m_range = 5.0f;

    protected bool m_death;

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
        float theta = Random.Range(0f, Mathf.PI * 2) - Mathf.PI;
        float range = Random.Range(0f, m_range);
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
}
