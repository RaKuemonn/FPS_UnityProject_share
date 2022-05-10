using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CondExprData/CondExprTimerData", fileName = "CondExprTimerData")]
//[CreateAssetMenu]
public class CondExprTimerData : BaseCondExprData
{
    [SerializeField, Range(0f, 100f)] public float idle_seconds = 3f;
    public override string CondExprComponentName()
    {
        return "CondExprTimer";
    }
}
