using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CondExprData/CondExprAllKillData", fileName = "CondExprAllKillData")]
public class CondExprAllKillData : BaseCondExprData
{ 
    [SerializeField] public GameObject BattleAreaPrefab;
    public BattleAreaTutorial BattleArea;

    public override string CondExprComponentName()
    {
        return "CondExprAllKill";
    }
}
