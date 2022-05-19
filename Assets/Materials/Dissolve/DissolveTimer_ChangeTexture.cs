using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveTimer_ChangeTexture : MonoBehaviour
{
    // �Ώۂ̃V�F�[�_�[���K�p���ꂽGameObject��ݒ肷��
    [SerializeField] 
    private GameObject obj; 

    Renderer r;

    // �����l
    [SerializeField]
    float Initial_value = 2;
    // �I���l
    [SerializeField]
    float End_value = -1;

    // ������X�s�[�h
    [SerializeField]
    float LostSpeed = 0.5f;

    // �l
    float value;

    // �f�B�]���u�J�n����
    bool test = false;

    // �f�B�]���u����
    bool complete = false;

    // �o�ߎ��Ԃ��i�[����ϐ�
    float time = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        // �l�ύX����
        r = obj.GetComponent<Renderer>();
        // �f�B�]���u�ȊO�̃A���t�@�l��0�ɂ��Ĕ�\���ɂ���
        r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);
        
        // 0�Ԗڂɓ����Ă�}�e���A����albedo��normal���f�B�]���u�}�e���A���ɃR�s�[����
        r.materials[r.materials.Length - 1].SetTexture("Main_Texture", r.materials[0].GetTexture("_BaseMap"));
        r.materials[r.materials.Length - 1].SetTexture("Normal", r.materials[0].GetTexture("_BumpMap"));
        
        value = Initial_value;
    }
    void Update()
    {
        // �Ԋu
        float duration = 1;

        // �Q�[������
        time += Time.deltaTime;

        // �f�B�]���u�������������Ȃ��悤��
        //if (r.materials[r.materials.Length - 1].GetFloat("TransparencyLevel") < End_value)
        if (complete == false)
        {
            // �������ꂽ��f�B�]���u�J�n
            // ���͉���2�b��ɊJ�n����悤�ɂ��Ă���
            if (time > 2)
            {
                test = true;
            }

            if (test)
            //if (time > 2)
            {
                // �v�Z
                value -= Time.deltaTime * LostSpeed;
                var t = value / duration;

                // �f�B�]���u
                r.materials[r.materials.Length - 1].SetFloat("TransparencyLevel", t);

                // �f�B�]���u�������I�������true��
                if (r.materials[r.materials.Length - 1].GetFloat("TransparencyLevel") < End_value)
                {
                    complete = true;
                    // �G�����𓊝����n�߂Ă����悤�ɋ���
                    // ������ = true;
                }
            }
        }
    }
}
