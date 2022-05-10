using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossCollider : MonoBehaviour
{
    private GameObject targetObject;    // ���̓����蔻��I�u�W�F�N�g�̉e����

    private Camera mainCamera;          // �J�������̎Q�Ɨp�H

    private RectTransform canvas_rect;  // �e�I�u�W�F�N�g�̈ʒu�Q�Ɨp

    private RectTransform rect;         // ���g�̌`�̎Q�Ɨp

    private float damage_timer;         // ���G���ԊǗ��p

    private GameObject player;          // �v���C���[�̈ʒu�Q�Ɨp (�������v�Z���邽��)

    [SerializeField]
    private float toleranceLevel; // �J�E���^�[�����͈�

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        rect = GetComponent<RectTransform>();

        canvas_rect = GameObject.Find("Canvas").GetComponent<RectTransform>();

        player = GameObject.FindGameObjectWithTag("Player");
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
        rect.localScale = new Vector3(1f, 1f, 1f) * scale;


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

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (damage_timer > 0f) return;

        // �����Ă���material�̐F���Ԃł͂Ȃ��΂ɂȂ��Ă�����
        if (targetObject.GetComponent<Renderer>().material.color.r > 0f) return;

        var slash = collider.gameObject;
        if (slash.tag != "Slash") return;
        slash.GetComponent<BoxCollider2D>().enabled = false;

        Debug.Log("slash hit");



        // ���G���Ԃ̐ݒ�
        damage_timer = 1.0f;
        // �J�[�\���̃Q�[���I�u�W�F�N�g����R���|�[�l���g���擾����B
        var cursor = GameObject.FindGameObjectWithTag("Cursor")
            .GetComponent<CursorController>();
        cursor.SetChainTimer();

        // �J�E���^�[�̉۔���
        {
            // TODO 1: slash(�摜)�̊p�x(angle)���擾���āA�P�ʃx�N�g��(Vector2)�����B              ( )
            var slash_ = GameObject.FindGameObjectWithTag("Slash");
            var slashImage = slash_.GetComponent<SlashImageController>();
            float slashAngle = slashImage.RadianAngle2D();
            Vector2 slashVec = MathHelpar.AngleToVector2(slashAngle);

            // TODO 2: ���̎w��R�}���h(Vector2)��slash�̃x�N�g��(Vector2)�̊p�x���Z�o����B   ( float resutl_angle = Vector2.Angle(�x�N�g��1, �x�N�g��2); )
            var bossControllr = GetComponent<EnemyBossController>();
            float sickleAngle = bossControllr.GetRadianSlashAngle();
            Vector2 sickleVec = MathHelpar.AngleToVector2(sickleAngle);

            // TODO 3: �p�x�����ȓ��Ȃ�A�J�E���^�[�����ɂ���B
            var dot = Vector2.Dot(slashVec, sickleVec);
            dot = Mathf.Acos(dot);
            if (toleranceLevel > dot && dot > -toleranceLevel)
            {
                bossControllr.SetDownFlag(true);
            }
            else
            {
                // TODO �ā@���s�������̏���������
            }
        }



        // TODO:�ؒf���鏈���̒ǉ�(��U�폜���Ă���)
        const float const_destroy_time = 0.5f;
        Destroy(targetObject, const_destroy_time);

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
}
