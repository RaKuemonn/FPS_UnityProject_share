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
    //[SerializeField, Range(0f, 100f)] public float idle_seconds = 3f;
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