using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CondExprData/CondExprSquareAllKillData", fileName = "CondExprSquareAllKillData")]
public class CondExprSquareAllKillData : BaseCondExprData
{
    public SquareBattleArea SquareBattleArea;

    public override string CondExprComponentName()
    {
        return "CondExprSquareAllKill";
    }
}