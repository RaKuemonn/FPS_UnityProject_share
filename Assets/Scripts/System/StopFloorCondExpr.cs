using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFloorConditionExpr : MonoBehaviour
{
    [SerializeField] private Func<bool> ConditionExpression;

    // 床の条件式をクリアした (true or false), 立ち止まる床から再び走り始めるための条件式 
    public bool OnCompleteFloorCondExpr()
    {
        return ConditionExpression.Invoke();
    }
}
