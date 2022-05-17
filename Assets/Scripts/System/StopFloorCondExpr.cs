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
//    // ���̏��������N���A���� (true or false), �����~�܂鏰����Ăё���n�߂邽�߂̏����� 
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
        var id = gameObject.GetComponent<FloorInfo>().FloorData.id;

        GameObject battleArea = null;
        {
            var position = new Vector3(0f, 0f, id * 10f /*id��10��Z����ƁA����z�l�ɂȂ�*/);

            // Stop���Ɠ����ʒu�ɐ���
            battleArea = Instantiate(
                ((CondExprAllKillData)data).BattleAreaPrefab,
                position,
                Quaternion.identity);

            // Enviroments�Q�[���I�u�W�F�N�g��e�ɂ���
            var Enviroments = GameObject.FindGameObjectWithTag("Enviroments");
            battleArea.transform.SetParent(Enviroments.transform);
        }

        ((CondExprAllKillData)data).BattleArea = battleArea.GetComponent<BattleAreaTutorial>();

    }
    
    public override void OnCompleteCondExpr(BooleanClass booleanClass)
    {
        var size = ((CondExprAllKillData)data).BattleArea?.InAreaEnemySize();

        booleanClass.Boolean = (size <= 0);
    }
    
}



[Serializable]
public class CondExprEndExplanation : BaseCondExpr
{
    private bool m_end_flag = false;

    void Start()
    {
        // �R�[���o�b�N�֐����Y���I�u�W�F�N�g�ɓo�^���āA�����̐^�U�����肳����B
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