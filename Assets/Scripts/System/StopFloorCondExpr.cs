using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFloorConditionExpr : MonoBehaviour
{
    [SerializeField] private Func<bool> ConditionExpression;

    // ���̏��������N���A���� (true or false), �����~�܂鏰����Ăё���n�߂邽�߂̏����� 
    public bool OnCompleteFloorCondExpr()
    {
        return ConditionExpression.Invoke();
    }
}
