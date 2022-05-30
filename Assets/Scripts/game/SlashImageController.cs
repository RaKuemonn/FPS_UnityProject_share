using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SlashImageController : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    private Image image;

    [SerializeField] private Material cutSurfaceMaterial = null;

    public float damage;    // Slash�̐�������CursorController�Őݒ肳���B

    //[SerializeField] private RectTransform child_rect_transform;

    [SerializeField] private PlayerStatus playerStatus;
    
    [SerializeField] private float toleranceLevel; // �J�E���^�[�����͈�

    // Start is called before the first frame update
    void Start()
    {
        // �����Đ�
        //GetComponent<AudioSource>().PlayOneShot(clip);
        GameObject.Find("SoundManager")?.GetComponent<S_SoundManager>()?.PlaySE((clip));
        //Debug.Log(GameObject.Find("SoundManager").GetComponent<S_SoundManager>().SEAudioSource.volume);

        var color = GetComponent<Image>().color;
        image = GetComponent<Image>();
        image.color = new Color(color.r, color.g, color.b, 225.0f);
        //Debug.Log(gameObject.GetComponent<BoxCollider2D>().size);

        // �͋Z power
        {
            var rectTransform = GetComponent<RectTransform>();
            var image_screen_position = rectTransform.position;
            var image_width = rectTransform.sizeDelta.x * rectTransform.lossyScale.x;
            var image_radian = rectTransform.eulerAngles.z * Mathf.Deg2Rad;
            //var input_direction = new Vector2(Mathf.Cos(image_radian), Mathf.Sin(image_radian));

            var width_half = image_width / 2f;
            var width_div5 = image_width / 5f;

            // 2D��]�֐� �������_���ŏ��������� 
            Func<Vector2, float, Vector2> Rotate = (Vector2 vector_, float radian_) =>
            {
                return new Vector2(
                    vector_.x * Mathf.Cos(radian_) - vector_.y * Mathf.Sin(radian_),
                    vector_.x * Mathf.Sin(radian_) + vector_.y * Mathf.Cos(radian_)
                );
            };

            // Ray�̐����w��
            const int max_size = 6;
            //const int min_size = 2;
            //var size = Mathf.Min(
            //    min_size,
            //    (int)(max_size * rectTransform.lossyScale.x/* 0.2f ~ 1.0f */)
            //);
            var size = max_size;

            // �X�����x�N�g�� (�X�N���[����ł�Ray�̔��ˈʒu�v�Z�p)
            var vectors = new Vector2[size];
            for (int i = 0; i < size; ++i)
            {
                vectors[i] = Rotate(new Vector2(-width_half + width_div5 * i, 0f), image_radian);
            }


            //  (���Ԃ�ł���)�@�X���ɍ��킹���ʒu�ɂ���K�v������
            var screen_positions = new Vector2[size];
            for (int i = 0; i < size; ++i)
            {
                screen_positions[i] = new Vector2(
                    image_screen_position.x + vectors[i].x,
                    image_screen_position.y + vectors[i].y);
            }

            // ���˂���Ray�B
            var rays = new Ray[size];
            for (int i = 0; i < size; ++i)
            {
                rays[i] = Camera.main.ScreenPointToRay(screen_positions[i]);
            }

            Collider result_hit_collider = null;           // �ŏI�I�Ɏg�p���铖���蔻��̌���
            Ray result_hit_ray = new Ray();
            // �����蔻�菈��
            {
                float hit_distance = float.MaxValue;

                foreach (var ray in rays)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, playerStatus.sword_area_radius) /* Ray�𓊎� */ == false)
                    {
#if UNITY_EDITOR
                        Debug.Log("Miss");
#endif
                        continue;
                    }


#if UNITY_EDITOR
                    Debug.Log("CurrentHit:" + hit.collider?.gameObject?.name);
#endif


                    // Tag���G����
                    if (hit.collider?.CompareTag("Enemy") == false &&
                        hit.collider?.CompareTag("Sickle") == false &&
                        hit.collider?.CompareTag("SickleThrowing") == false &&
                        hit.collider?.CompareTag("Boss") == false) continue;

                    if (hit.collider.CompareTag("Sickle"))
                    {

                    }

                    // ������hit_distance��蒷���A�����ꏊ�ɂ���B
                    if (hit.distance > hit_distance) continue;

                    // �ŒZ���ʂ̓G
                    result_hit_collider = hit.collider;
                    hit_distance = hit.distance;
                    result_hit_ray = ray;
                }
            }


            // �������Ă��Ȃ���Α���return������
            if (result_hit_collider == null) return;

            


            // �������Ă���G�ɏ������s��
            BaseEnemy enemy = result_hit_collider.gameObject.GetComponent<BaseEnemy>();
            EnemyBossController boss = result_hit_collider.gameObject.GetComponent<EnemyBossController>();

            if (enemy)
            {
                var current_enemy_hp = enemy.GetHP() - damage;

                // �̗͂������
                if (current_enemy_hp > 0f)
                {
                    // �_���[�W��^���鏈��
                    // (�����֐��Ăяo�����A�R�[���o�b�N������B)
                    enemy.SetHP(current_enemy_hp);


                }
                // �̗͂��Ȃ����
                else
                {
                    enemy.OnDead();
                    enemy.SetHP(0f);



                    // �J�E���^�[����
                    if (enemy.tag == "Sickle")
                    {
                        var Sickle = ((SickleController)enemy);
                        Vector2 slashVec = new Vector2(
                            Mathf.Cos(RadianAngle2D()),
                            Mathf.Sin(RadianAngle2D())).normalized;
                        Vector2 sickleVec = new Vector2(
                            Mathf.Cos(Sickle.GetRadianSlashAngle()),
                            Mathf.Sin(Sickle.GetRadianSlashAngle())).normalized;

                        // TODO 3: �p�x�����ȓ��Ȃ�A�J�E���^�[�����ɂ���B
                        var dot = Vector2.Dot(slashVec, sickleVec);
                        dot = Mathf.Acos(dot);
                        if (dot < toleranceLevel)
                        {
                            Sickle.DisableMesh();
                        }
                        else
                        {
                            GameObject
                                .FindGameObjectWithTag("Player")
                                .GetComponent<PlayerAutoControl>()
                                .OnDamage(enemy.m_damage);

                            Sickle.DisableMesh();
                        }
                    }

                    else if (enemy.tag == "SickleThrowing")
                    {
                        var Sickle = ((SickleThrowingController)enemy);
                        Vector2 slashVec = MathHelpar.AngleToVector2(RadianAngle2D());
                        Vector2 sickleVec = MathHelpar.AngleToVector2((Sickle.GetRadianSlashAngle()));

                        // TODO 3: �p�x�����ȓ��Ȃ�A�J�E���^�[�����ɂ���B
                        var dot = Vector2.Dot(slashVec, sickleVec);
                        dot = Mathf.Acos(dot);
                        if (toleranceLevel > dot && dot > -toleranceLevel)
                        {
                            Sickle.DisableMesh();
                        }
                        else
                        {
                            GameObject
                                .FindGameObjectWithTag("Player")
                                .GetComponent<PlayerAutoControl>()
                                .OnDamage(enemy.m_damage);

                            Sickle.DisableMesh();
                        }
                    }

                    // �ؒf����
                    else
                    {
                        Cut(enemy, cutSurfaceMaterial, result_hit_ray, rays[0], rays[1]);
                    }


                }
            }
            else if(boss)
            {
                var current_boss_hp = boss.GetHP() - damage;

                // �̗͂������
                if (current_boss_hp > 0f)
                {
                    switch (boss.GetState())
                    {

                        case EnemyBossController.State.AssaultAttackAnim:
                            boss.SetDownFlag(true);
                            break;

                        case EnemyBossController.State.Down:
                            // �_���[�W��^���鏈��
                            // (�����֐��Ăяo�����A�R�[���o�b�N������B)
                            boss.OnDamaged(damage);
                            break;

                        default:
                            // nothing
                            break;
                    }

                }
                // �̗͂��Ȃ����
                else
                {
                    switch (boss.GetState())
                    {
                        case EnemyBossController.State.AssaultAttack:
                            boss.SetDeathFlag();
                            boss.OnDamaged(damage);
                            break;

                        case EnemyBossController.State.Down:
                            boss.SetDeathFlag();
                            boss.OnDamaged(damage);
                            break;

                        default:
                            // nothing
                            break;
                    }
                    
                }
            }


        }
    }
    

    // Update is called once per frame
    void Update()
    {

        if (image == null) return;

        var color = image.color;
        if (color.a > 0.1f)
        {
            float alpha = Mathf.Lerp(color.a, 0.0f, 10.0f * Time.deltaTime);
            image.color = new Color(color.r, color.g, color.b, alpha);
        }
        else
        {
            Destroy(gameObject);
        }

#if false
        {
            var rectTransform = GetComponent<RectTransform>();
            var image_screen_position = rectTransform.position;
            var image_width = rectTransform.sizeDelta.x * rectTransform.lossyScale.x;
            var image_radian = rectTransform.eulerAngles.z * Mathf.Deg2Rad;
            //var input_direction = new Vector2(Mathf.Cos(image_radian), Mathf.Sin(image_radian));

            var width_half = image_width / 2f;
            var width_div5 = image_width / 5f;

            // 2D��]�֐� �������_���ŏ��������� 
            Func<Vector2, float, Vector2> Rotate = (Vector2 vector_, float radian_) =>
            {
                return new Vector2(
                    vector_.x * Mathf.Cos(radian_) - vector_.y * Mathf.Sin(radian_),
                    vector_.x * Mathf.Sin(radian_) + vector_.y * Mathf.Cos(radian_)
                );
            };

            // Ray�̐����w��
            const int max_size = 6;
            //const int min_size = 2;
            //var size = Mathf.Min(
            //    min_size,
            //    (int)(max_size * rectTransform.lossyScale.x/* 0.2f ~ 1.0f */)
            //);
            var size = max_size;

            // �X�����x�N�g�� (�X�N���[����ł�Ray�̔��ˈʒu�v�Z�p)
            var vectors = new Vector2[size];
            for (int i = 0; i < size; ++i)
            {
                vectors[i] = Rotate(new Vector2(-width_half + width_div5 * i, 0f), image_radian);
            }


            //  (���Ԃ�ł���)�@�X���ɍ��킹���ʒu�ɂ���K�v������
            var screen_positions = new Vector2[size];
            for (int i = 0; i < size; ++i)
            {
                screen_positions[i] = new Vector2(
                    image_screen_position.x + vectors[i].x,
                    image_screen_position.y + vectors[i].y);
            }

            // ���˂���Ray�B
            var rays = new Ray[size];
            for (int i = 0; i < size; ++i)
            {
                rays[i] = Camera.main.ScreenPointToRay(screen_positions[i]);
            }

            foreach (var ray in rays)
            {
                Debug.DrawRay(ray.origin, ray.direction);
            }
        }
#endif

    }

    public Ray SlashRay()
    {
        return Camera.main.ScreenPointToRay(transform.position);
        //return Camera.main.ScreenPointToRay(child_rect_transform.position);
    }

    public float RadianAngle2D()
    {
        return image.rectTransform.eulerAngles.z * Mathf.Deg2Rad;
    }

    private float Angle2D()
    {
        return image.rectTransform.eulerAngles.z;
    }

    static void Cut(BaseEnemy enemy, Material cutSurfaceMaterial, Ray result_hit_ray, Ray need_to_culculate_ray_1, Ray need_to_culculate_ray_2)
    {

        Vector3 normal;
        {
            const float distance = 5.0f;
            var origin_position = result_hit_ray.origin;
            var far_left = need_to_culculate_ray_1.GetPoint(distance);
            var far_right = need_to_culculate_ray_2.GetPoint(distance);

            var left = far_left - origin_position;
            var right = far_right - origin_position;

            normal = Vector3.Cross(left, right).normalized;
        }

        var result =
            MeshCut.CutMesh(
                enemy.gameObject,                                          // �a��I�u�W�F�N�g
                Camera.main.transform.position,   // ���ʏ�̈ʒu
                normal,                                          // ���ʂ̖@��
                true,
                cutSurfaceMaterial
            );

        var original = result.original_anitiNormalside;
        var copy = result.copy_normalside;



        Action<GameObject, Vector3> Cutted = (GameObject object_, Vector3 normal_direction_) =>
        {

#if UNITY_EDITOR
            if (object_ == null)
            {
                Debug.Log("nulllllllllllllllllllllllll");
            }
#endif
            // ���S����
            const float impulse_power = 8f;
            var impulse = (result_hit_ray.direction + normal_direction_) * impulse_power;

            object_?.GetComponent<BaseEnemy>().OnCutted(impulse);
        };

        Cutted.Invoke(original, -1.0f * normal);
        Cutted.Invoke(copy, normal);
    }

}
