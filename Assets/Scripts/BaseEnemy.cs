using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    // 各自で設定
    protected float m_hp;

    // 旋回
    protected float m_turnAngle = 5.0f;
    protected float m_turnSpeed = 3.0f;

    // 目的地
    protected Vector3 m_targetPosition;
    protected Vector3 m_territoryOrigin;
    protected float m_range = 5.0f;

    protected bool m_death;

    // 集合
    protected bool m_assemblyFlag = false;

    // 戦闘開始
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

    // ターゲット位置をランダム設定
    protected void SetRandamTargetPosition()
    {
        float theta = Random.Range(0f, Mathf.PI * 2) - Mathf.PI;
        float range = Random.Range(0f, m_range);
        m_targetPosition.x = m_territoryOrigin.x + Mathf.Sin(theta) * range;
        m_targetPosition.y = m_territoryOrigin.y;
        m_targetPosition.z = m_territoryOrigin.z + Mathf.Cos(theta) * range;
    }

    protected Quaternion TurnRotation(Vector3 forward, Vector3 dir, float turnAngle, float angularSpeed)
    {
        var forward_ = forward;
        var dir_ = dir;

        dir_.y = forward_.y = 0.0f;
        float angle = Vector3.Angle(dir_.normalized, forward_.normalized);

        Quaternion rotation = Quaternion.LookRotation(forward_.normalized);

        // 角度が一定以上なら旋回
        if (angle > turnAngle)
        {
            Quaternion rotation2 = Quaternion.LookRotation(dir_.normalized);
            rotation = Quaternion.RotateTowards(rotation, rotation2, angularSpeed  * Time.deltaTime);
        }

        return rotation;
    }
}
