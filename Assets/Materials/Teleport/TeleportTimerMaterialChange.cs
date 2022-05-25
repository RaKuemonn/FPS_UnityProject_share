using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTimerMaterialChange : MonoBehaviour
{
    [SerializeField] private GameObject obj; // �Ώۂ̃V�F�[�_�[���K�p���ꂽGameObject��ݒ肷��

    // update�Ŏg�����߂ɃR�����g���� ���}�e���A�������Ƃ��͂��߂������H
    Renderer r;
    // �C���X�y�N�^�[������e���|�[�g�}�e���A���̒ǉ�����
    [SerializeField]
    Material[] materials = new Material[1];
    //Material[] materials = new Material[2];
    //GMaterial[] materials = new Material[3];
    
    // �����l
    [SerializeField]
    float[] initial_value = { -2.0f, 0.5f};

    // �I���l
    [SerializeField]
    float[] End_value = { 0.5f, -0.5f};

    // ������X�s�[�h
    [SerializeField]
    float[] LostSpeed = { 2.0f, 0.5f };

    //�����t�]
    float sign;

    // �l
    float[] value = {0,0};

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

        // �e���|�[�g�������������Ȃ��悤��
        if (complete == false)
        {
            // ���͉���2�b��ɊJ�n����悤�ɂ��Ă���
            //if (time > 2 && test == false)
            //{
            //    test = true;
            //}


            if (test)
            {
                for (int i = 0; i < r.materials.Length; i++)
                {
                    //�����t�]���ĂȂ��Ƃ�
                    if (r.materials[i].GetFloat("_sign") == 0)
                    {
                        value[i] -= Time.deltaTime * LostSpeed[i];
                        var t = value[i] / duration;
                        //for (int i = 0; i < materials.Length; i++)
                        //{
                        //    materials[i].SetFloat("_Slider", t);
                        //}
                        //for (int i = 0; i < r.materials.Length; i++)
                        {
                            // �����ڂɊւ��̂�1�����Ȃ̂�0�ł���
                            r.materials[i].SetFloat("_Slider", t);
                        }
                    }
                    //�����t�]���Ă�Ƃ�
                    else if (r.materials[i].GetFloat("_sign") == 1)
                    {
                        value[i] += Time.deltaTime * LostSpeed[i];
                        var t = value[i] / duration;

                        //for (int i = 0; i < materials.Length; i++)
                        //{
                        //    materials[i].SetFloat("_Slider", t);
                        //}
                        //for (int i = 0; i < r.materials.Length; i++)
                        {
                            // �����ڂɊւ��̂�1�����Ȃ̂�0�ł���
                            r.materials[i].SetFloat("_Slider", t);
                        }
                    }

                    // �e���|�[�g�������I�������true��
                    //if (materials[materials.Length - 1].GetFloat("_Slider") > End_value)
                    if (r.materials[0].GetFloat("_Slider") > End_value[0] && r.materials[0].GetFloat("_Slider") < End_value[1])
                    {
                        complete = true;
                        // ��������
                        // ������ = true;
                    }
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


#if false

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
        //var r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();
        //r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g, r.materials[0].color.b, 0);

        //// 0�Ԗڂɓ����Ă�}�e���A����albedo��normal���f�B�]���u�}�e���A���ɃR�s�[����
        //r.materials[1].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        //r.materials[1].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        //sign = r.materials[1].GetFloat("_sign");

        //r.materials[1].CopyPropertiesFromMaterial(r.materials[0]);

        //materials[0] = r.sharedMaterials[1];
        //materials[1] = r.sharedMaterials[1];
        //materials[2] = r.sharedMaterials[2];

        // �����Ŗ���(2�ԖځA r.materials[1])�ɍ��}�e���A�����ǉ������
        r = gameObject.GetComponentInParentAndChildren<MeshRenderer>();

        // r.materials[0]���g���̂ŃR�����g�A�E�g
        //r.materials[0].color = new Color(r.materials[0].color.r, r.materials[0].color.g,r.materials[0].color.b, 0);

        // 0�Ԗڂɓ����Ă�}�e���A����albedo��norma���e���|�[�g�}�e���A���ɃR�s�[����
        materials[0].SetTexture("_MainTex", r.materials[0].GetTexture("_BaseMap"));
        materials[0].SetTexture("_BumpMap", r.materials[0].GetTexture("_BumpMap"));

        //�e���|�[�g�}�e���A���̏�񂩂�����t�]���Ă��邩�擾
        //r.materials[1].SetFloat("_sign", materials[0].GetFloat("_sign"));

        //r.materials[1].CopyPropertiesFromMaterial(r.materials[0]);
        //materials[0] = r.sharedMaterials[1];
        //materials[1] = r.sharedMaterials[1];
        //materials[2] = r.sharedMaterials[2];

        // �����ڂ�S��r.materials[0]�Ƀe�N�X�`�����R�s�[���Ă���e���|�[�g�}�e���A��������
        r.materials[0].shader = materials[0].shader;
        r.materials[0].CopyPropertiesFromMaterial(materials[0]);

        //r.materials[1].SetFloat("_sign", 1);
        value[0] = initial_value[0];
        value[1] = initial_value[1];
#endif
    }
}
