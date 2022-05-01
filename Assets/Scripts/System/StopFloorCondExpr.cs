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
//    // °‚ÌğŒ®‚ğƒNƒŠƒA‚µ‚½ (true or false), —§‚¿~‚Ü‚é°‚©‚çÄ‚Ñ‘–‚èn‚ß‚é‚½‚ß‚ÌğŒ® 
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