using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFloorConditionExpr : MonoBehaviour
{
    [SerializeField] private Func<bool> ConditionExpression;

    // °‚ÌğŒ®‚ğƒNƒŠƒA‚µ‚½ (true or false), —§‚¿~‚Ü‚é°‚©‚çÄ‚Ñ‘–‚èn‚ß‚é‚½‚ß‚ÌğŒ® 
    public bool OnCompleteFloorCondExpr()
    {
        return ConditionExpression.Invoke();
    }
}
