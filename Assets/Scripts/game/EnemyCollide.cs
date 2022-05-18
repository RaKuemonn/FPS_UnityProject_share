using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyCollide : MonoBehaviour
{
    [SerializeField] private float impulse_power;   // �a�������̏Ռ���

    private GameObject targetObject;    // ���̓����蔻��I�u�W�F�N�g�̉e����

    private Camera mainCamera;          // �J�������̎Q�Ɨp�H

    private RectTransform canvas_rect;  // �e�I�u�W�F�N�g�̈ʒu�Q�Ɨp

    private RectTransform rect;         // ���g�̌`�̎Q�Ɨp

    private float damage_timer;         // ���G���ԊǗ��p
    
    private GameObject player;          // �v���C���[�̈ʒu�Q�Ɨp (�������v�Z���邽��)

    private PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        rect = GetComponent<RectTransform>();

        canvas_rect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        
        player = GameObject.FindGameObjectWithTag("Player");

        playerStatus = GameObject.FindGameObjectWithTag("SceneSystem").GetComponent<MasterData>().PlayerStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if (damage_timer > 0f)
        {
            damage_timer += -Time.deltaTime;

            if (damage_timer < 0f)
            {
                damage_timer = 0f;
            }
        }


        if (CheckTarget()) return;

        UpdatePosition();

        UpdateCollisionCircleSize();
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }

    private void UpdatePosition()
    {
        if (targetObject == null) return;

        var screen_position = mainCamera.WorldToScreenPoint(
            targetObject.transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas_rect,
            screen_position,
            null,
            out var canvas_position);

        rect.anchoredPosition = canvas_position;

    }

    private void UpdateCollisionCircleSize()
    {
        if (player == null) return;


        var distance = Vector3.Distance(player.transform.position, targetObject.transform.position);

        var scale = -0.2f * distance + 3f;
        //var scale = 3.5f + 0.3f * distance;



        // �摜�̃X�P�[����ς����, �����蔻������Ă����
        rect.localScale = new Vector3(1f,1f,1f) * scale;


    }

    private bool CheckTarget()
    {
        // �Q�Ƃ��Ă���G�������ꂽ��
        if (targetObject) return false;

        //Debug.Log("delete enemy collide");

        // �������g������
        Destroy(gameObject);
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (damage_timer > 0f) return;

        // �G�����蔻��ɓ��������I�u�W�F�N�g�� Slash��
        if (collider.tag != "Slash") return;
        
        // �ԍ����ɓ����Ă��邩
        if (InSwordArea() == false) return;

        // Slash�̓����蔻�肪����ȏ�����Ȃ��悤�ɂ���
        collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;



#if UNITY_EDITOR
        Debug.Log("slash hit");
#endif


        // ���G���Ԃ̐ݒ�
        damage_timer = 1.0f;

        // �J�[�\����
        var cursor = GameObject.FindGameObjectWithTag("Cursor")
            .GetComponent<CursorController>();

        cursor.SetChainTimer();
        

        // �ؒf���鏈��   TODO : EnemyController�R���|�[�l���g���ɋL�q���ڂ��̂�����B
        if(false/* ���������L�q�@��:�̗͂�0�ȉ��Ȃ�Ƃ� */)
        {

            Vector3 normal;
            {
                var slash_ray =
                    collider.gameObject.
                        GetComponent<SlashImageController>().
                        SlashRay();
                
                var cursol_ray = cursor.CursolRay();


                const float distance = 5.0f;
                var origin_position = mainCamera.transform.position;
                var cursol_far = cursol_ray.GetPoint(distance);
                var slash_far = slash_ray.GetPoint(distance);

                var a = slash_far  - origin_position;
                var b = cursol_far - origin_position;

                normal = Vector3.Cross(a, b).normalized;
            }

            var result =
                MeshCut.CutMesh(
                    targetObject,                                   // �a��I�u�W�F�N�g
                    mainCamera.transform.position,    // ���ʏ�̈ʒu
                    normal                                          // ���ʂ̖@��
                    );

            // TODO : �a�������ɂ͂˂�悤�ɂ�����
            var impulse_copy = normal;

            // �a�����I�u�W�F�N�g�̎��㏈���A�폜�\��
            ObjectCutted(result.copy_normalside, false, impulse_copy);
            ObjectCutted(result.original_anitiNormalside, true, impulse_copy * -1.0f);
        }
        else
        {
            // ����ObjectCutted�֐�����R�s�y�����e�X�g�p����
            // Scarecrow��EnemyController�������Ȃ�
            //targetObject.GetComponent<EnemyController>().SetCutPerformance(false, new Vector3(0f,0f,0f));

            
            // �Ռ���^���鏈��
            {
                var impulse = cursor.CursolRay().direction * impulse_power;

                impulse.y += impulse_power; // ������ɔ����ɗ͂�������

                targetObject.GetComponent<BaseEnemy>().OnCutted(impulse);
            }

            const float const_destroy_time = 0.5f;
            Destroy(targetObject, const_destroy_time);
        }
        
    }

    private void ObjectCutted(GameObject object_, bool is_, Vector3 impulse_direction_)
    {
        if (object_ == null) return;

        var impulse_ = impulse_direction_ * 2.0f;
        impulse_.y += 0.1f;                          // ���˂�����
        object_.GetComponent<EnemyController>().SetCutPerformance(is_, impulse_);
        
        const float const_destroy_time = 0.5f;
        Destroy(object_, const_destroy_time);

    }

    private bool InSwordArea()
    {
        var distance = Vector3.Distance(player.transform.position, targetObject.transform.position);
        var sword_area_radius = playerStatus.sword_area_radius;

        return (distance <= sword_area_radius);
    }
}
