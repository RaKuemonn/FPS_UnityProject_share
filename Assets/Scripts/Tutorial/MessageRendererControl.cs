using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MessageRendererControl : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    [SerializeField] private TextDataTable textDataTable;

    public UnityEvent CallBackCondExprEndExplanation = new UnityEvent();

    private int read_page_count = 0; // �ǂ񂾃y�[�W��
    
    void Update()
    {
        // ��Ɠr�� 13:41������u
        // �����ł�肽������ ���݂̃y�[�W�̃e�L�X�g���AtextMeshPro�Ɉꕶ�����\�����Ă����B
        // �Ȃɂ�����{�^���������ꂽ�Ƃ��A�y�[�W���̂��ׂĂ̕�����\���B�������͂��̃y�[�W�ֈړ�����B
        // �Ō�̃y�[�W�łڂ��񂪉����ꂽ�Ƃ��AStopFloor��CondExprEndExplanation�R���|�[�l���g�̃R�[���o�b�N�֐����Ăяo��
        
        // tutorial floor table data �̏��ɒǉ�����B
    }
}
