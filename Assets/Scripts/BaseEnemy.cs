using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseEnemy : MonoBehaviour
{
    // 各自で設定
    protected float m_hp;

    // バトルエリアに入ったか
    protected bool m_enter_battle_area;

    // BattleAreaのコールバック関数
    public event EventHandler OnDeadEvent;


    // 旋回 
    protected float m_turnAngle = 1.0f;
    protected float m_turnSpeed = 3.0f;

    // 目的地
    protected Vector3 m_targetPosition;
    protected Vector3 m_territoryOrigin;
    protected float m_range = 5.0f;

    public bool IsDeath { set; get; } = false;

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
        float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2) - Mathf.PI;
        float range = UnityEngine.Random.Range(0f, m_range);
        m_targetPosition.x = m_territoryOrigin.x + Mathf.Sin(theta) * range;
        m_targetPosition.y = m_territoryOrigin.y;
        m_targetPosition.z = m_territoryOrigin.z + Mathf.Cos(theta) * range;
    }

    protected void TurnRotation(Vector3 vec)
    {
        // 自分の向いている方向から敵の方向までの角度を算出する
        var dir = transform.forward;
        dir.y = vec.y = 0.0f;
        dir.Normalize();
        vec.Normalize();
        float angle = Vector3.Angle(dir, vec);
        // 角度が一定以上の場合は旋回する
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

        // 自分の向いている方向から敵の方向までの角度を算出する
        var dir = transform.forward;
        dir.y = vec.y = 0.0f;
        float angle = Vector3.Angle(dir, vec);
        // 角度が一定以上の場合はOK
        if (angle < m_turnAngle)
        {
            check = true;
        }

        var rotate = Quaternion.LookRotation(vec);
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, rotate, 0.01f);

        return check;
    }

    // EnemyControllerからコピペして持ってきた
    protected void CreateCollideOnCanvas()
    {
        // 斬られたときに生成されたオブジェクトでなければ（SetCutPerformance()で、変更されていなければ）
        //if (is_create_collide == false) return;

        // 当たり判定用のオブジェクトをCanvas下に生成
        GameObject obj = Instantiate(
            (GameObject)Resources.Load("EnemyCollideOnScreen")
        );
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.GetComponent<EnemyCollide>().SetTarget(gameObject);
    }

    // 戦闘エリアに入ったらコールバックされる
    public void OnEnterBattleArea()
    {
        m_enter_battle_area = true;
    }



    ///
    ///
    /// 斬られたときに呼ばれる(手動で書き込んで呼び出してる)関数
    ///
    /// ほぼ。死亡処理なので、それも書き込む
    /// 
    /// 
    public void OnCutted(Vector3 impulse_)
    {

        var rigidbody = GetComponent<Rigidbody>();

        // rigidbodyプロパティの変更
        {
            // 重力に従う
            rigidbody.useGravity = true;

            // 拘束を無くす
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        // 弾き飛ばす
        rigidbody.AddForce(impulse_, ForceMode.Impulse);

        // 破棄する固定時間
        const float const_destroy_time = 0.5f;
        Destroy(gameObject, const_destroy_time);
    }

    void OnDestroy()
    {
        OnDead();
    }

    // 死亡処理 (private)
    private void OnDead()
    {
        OnDeadEvent?.Invoke(this,EventArgs.Empty);
    }
}
