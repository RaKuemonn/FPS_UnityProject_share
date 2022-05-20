using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTimerMaterialChange : MonoBehaviour
{
    [SerializeField] private GameObject obj; // �Ώۂ̃V�F�[�_�[���K�p���ꂽGameObject��ݒ肷��

    //Renderer r;

    Material[] materials = new Material[2];
    //GMaterial[] materials = new Material[3];
    
    // �����l
    [SerializeField]
    float initial_value = -2.0f;

    // �I���l
    [SerializeField]
    float End_value = 0.5f;

    // ������X�s�[�h
    [SerializeField]
    float LostSpeed = 2.0f;

    //�����t�]
    float sign;

    // �l
    float value;

    // �e���|�[�g�J�n����
    bool test = false;

    // �e���|�[�g����
    bool complete = false;

    // �o�ߎ��Ԃ��i�[����ϐ�
    float time = 0.0f; 


    // Start is called before the first frame update
    void Start()
    {
        // �l�ύX����
        //r = obj.GetComponent<Renderer>();
        

        // OnDead�֐��Ɉړ�
        //r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);

        // 0�Ԗڂɓ����Ă�}�e���A����albedo��normal���f�B�]���u�}�e���A���ɃR�s�[����
        //r.materials[r.materials.Length - 1].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        //r.materials[r.materials.Length - 1].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        //sign = r.materials[r.materials.Length - 1].GetFloat("_sign");



        //�����t�]���Ă��邩�ǂ���

        //value = initial_value;
        //test = false;
        //time = 0.0f;
    }
    void Update()
    {
        // �Ԋu
        float duration = 1;

        time += Time.deltaTime;

        // �f�B�]���u�������������Ȃ��悤��
        if (complete == false)
        {
            // ���͉���2�b��ɊJ�n����悤�ɂ��Ă���
            //if (time > 2 && test == false)
            //{
            //    test = true;
            //}


            if (test)
            {
                //�����t�]���ĂȂ��Ƃ�
                if (sign == 0)
                {
                    value -= Time.deltaTime * LostSpeed;
                    var t = value / duration;
                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i].SetFloat("_Slider", t);
                    }
                }
                //�����t�]���Ă�Ƃ�
                else if (sign == 1)
                {
                    value += Time.deltaTime * LostSpeed;
                    var t = value / duration;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i].SetFloat("_Slider", t);
                    }
                }

                // �f�B�]���u�������I�������true��
                //if (materials[materials.Length - 1].GetFloat("_Slider") > End_value)
                if (materials[0].GetFloat("_Slider") > End_value)
                {
                    complete = true;
                    // �G�����𓊝����n�߂Ă����悤�ɋ���
                    // ������ = true;
                }
            }
        }
    }

    public void OnDead()
    {
        test = true;

        //var r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();
        // 
        //r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);
        //
        //// 0�Ԗڂɓ����Ă�}�e���A����albedo��normal���f�B�]���u�}�e���A���ɃR�s�[����
        //r.materials[1].SetTexture("_MainTex",
        //    r.materials[0].GetTexture("_BaseMap"));
        //r.materials[1].SetTexture("_BumpMap",
        //    r.materials[0].GetTexture("_BumpMap"));
        //
        //sign = r.materials[1].GetFloat("_sign");
        //
        //materials[0] = r.sharedMaterials[1];
        //materials[1] = r.sharedMaterials[2];
        //
        //
        ////r.materials[1].CopyPropertiesFromMaterial(r.materials[0]);


#if true

        // SlashHigh���g���ā@����
        // materials��3�ɕς���

        var r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();
        r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);

        // 0�Ԗڂɓ����Ă�}�e���A����albedo��normal���f�B�]���u�}�e���A���ɃR�s�[����
        r.materials[1].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        r.materials[1].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        sign = r.materials[1].GetFloat("_sign");


        materials[0] = r.sharedMaterials[1];
        materials[1] = r.sharedMaterials[1];

        value = initial_value;

#else
        var r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();
        r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);

        // 0�Ԗڂɓ����Ă�}�e���A����albedo��normal���f�B�]���u�}�e���A���ɃR�s�[����
        r.materials[1].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        r.materials[1].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        sign = r.materials[1].GetFloat("_sign");

        r.materials[1].CopyPropertiesFromMaterial(r.materials[0]);

        materials[0] = r.sharedMaterials[1];
        materials[1] = r.sharedMaterials[1];
        materials[2] = r.sharedMaterials[2];



        value = initial_value;
#endif
    }
}
