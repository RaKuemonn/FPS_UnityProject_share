using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class OnCompleteFloorCondExprEvent : UnityEvent<BooleanClass> { }
[Serializable] public class OnFloorEnterEvent : UnityEvent { }

//public class StopFloorConditionExpr : MonoBehaviour
//{
//    [SerializeField] public UnityEvent<BooleanClass> ConditionExpression;
//
//    // 床の条件式をクリアした (true or false), 立ち止まる床から再び走り始めるための条件式 
//    //public bool OnCompleteFloorCondExpr()
//    //{
//    //    BooleanClass boolean = new BooleanClass();
//    //    ConditionExpression?.Invoke(boolean);
//    //    return boolean.Boolean;
//    //}
//}

[Serializable]
public class BooleanClass
{
    public bool Boolean = false;
}

[Serializable]
public abstract class BaseCondExpr : MonoBehaviour
{
    public BaseCondExprData data;
    public abstract void OnCompleteCondExpr(BooleanClass booleanClass);
}

[Serializable]
public class CondExprTimer : BaseCondExpr
{
    public float timer;

    public void Start()
    {
        timer = 0f;
    }

    public override void OnCompleteCondExpr(BooleanClass booleanClass)
    {
        timer += Time.deltaTime;

        booleanClass.Boolean = (timer > ((CondExprTimerData)data).idle_seconds);
    }
    
}

[Serializable]
public class CondExprAllKill : BaseCondExpr
{
    void Start()
    {
        var id_ten_times = gameObject.GetComponent<FloorInfo>().FloorData.id * 10;
        var areas = GameObject.FindGameObjectsWithTag("BattleArea");

        // 床と同じ位置にBattleAreaが存在するはずなので、それを探す
        foreach (var area in areas)
        {
            if((int)(area.transform.position.z) != id_ten_times /* idとワールド座標z軸の10の位が一緒なら */)
            { continue;}

            ((CondExprAllKillData)data).battleArea = area.GetComponent<BattleAreaTutorial>();
            break;
        }
        
    }
    
    public override void OnCompleteCondExpr(BooleanClass booleanClass)
    {
        var size = ((CondExprAllKillData)data).battleArea?.InAreaEnemySize();

        booleanClass.Boolean = (size <= 0);
    }

}



[Serializable]
public class CondExprEndExplanation : BaseCondExpr
{
    private bool m_end_flag = false;

    void Start()
    {
        // コールバック関数を該当オブジェクトに登録して、条件の真偽を決定させる。
        // UnityEvent.AddListener(OnEndExplanation);
        var component = GameObject
            .FindGameObjectWithTag("MessageRenderer")
            .GetComponent<MessageRendererControl>();

        component
            .CallBackCondExprEndExplanation
            .AddListener(OnEndExplanation);


    }

    public override void OnCompleteCondExpr(BooleanClass booleanClass)
    {
        booleanClass.Boolean = m_end_flag;
    }

    public void OnEndExplanation()
    {
        m_end_flag = true;

        var component = GameObject
            .FindGameObjectWithTag("MessageRenderer")
            .GetComponent<MessageRendererControl>();
    }
}