using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class OnCompleteFloorCondExprEvent : UnityEvent<BooleanClass> { }

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