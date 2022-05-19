using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private float cursor_speed = Screen.width;

    [SerializeField] private float min_damage;
    [SerializeField] private float max_damage;

    [SerializeField] private GameObject SlashPrefab;

    [SerializeField] private Camera currentCamera;


    private RectTransform rect;

    private Vector2 offset;

    private float old_angle;

    private InputAction moveL;
    private InputAction moveR;

    private bool now_clip_played;
    private float chain_kill_timer; // 数値が正の値の時,切り返すことが出来る

 

    // Start is called before the first frame update
    void Start()
    {
        rect    = GetComponent<RectTransform>();
        transform.localPosition 
                = new Vector2(Screen.width / 2f, Screen.height / 2f);
        offset  = new Vector2(rect.sizeDelta.x / 2.0f, rect.sizeDelta.y / 2.0f);


        moveL   = GetComponent<PlayerInput>().actions["Move"];
        moveR   = GetComponent<PlayerInput>().actions["Direction"]; ;
        now_clip_played = false;
        chain_kill_timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        chain_kill_timer += -Time.deltaTime;
        
        CursorControl();

        SlashControl();

    }

    private void CursorControl()
    {
        if (moveL == null) return;

        var _input = moveL.ReadValue<Vector2>();

        if (_input == Vector2.zero) return;

        var position = rect.anchoredPosition + new Vector2(_input.x, _input.y).normalized * cursor_speed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -Screen.width * 0.5f + offset.x, Screen.width * 0.5f - offset.x);
        position.y = Mathf.Clamp(position.y, -Screen.height * 0.5f + offset.y, Screen.height * 0.5f - offset.y);

        rect.anchoredPosition = position;
    }

    private void SlashControl()
    {
        Ray ray_ = CursolRay();
        //Debug.Log(ray_.origin);
        Debug.DrawRay(ray_.origin , ray_.direction);


        if (moveR == null) return;

        var _input = moveR.ReadValue<Vector2>().normalized;


        // ���͂��Ȃ����
        if (_input == Vector2.zero)
        {
            // ���t���[����Slash����t���O�𗧂āAreturn������
            now_clip_played = false;
            return;
        }

        // �O��a�������͕����ƍ�����͕����̊p�x����Z�o
        var old_input   = new Vector2(Mathf.Cos(old_angle * Mathf.Deg2Rad), Mathf.Sin(old_angle * Mathf.Deg2Rad)).normalized;
        var dif_degree  = Mathf.Acos(Vector2.Dot(_input, old_input)) * Mathf.Rad2Deg;

        // 斬り返した方向が前回の100度 
        if (dif_degree > 100f)
        {
            // Slashする
            now_clip_played = false;
        }

        // Slashする
        if (now_clip_played == false)
        {
            now_clip_played = true;

#if false
            // たおす敵がいるか (一体だけ消す)
            foreach (RaycastHit hit_ in Physics.RaycastAll(CursolRay(), 10.0f))
            {
                GameObject enemy = hit_.transform.gameObject;

                // tagがEnemyで
                if (enemy.tag != "Enemy") continue;
                // 持っているmaterialの色が赤ではなく緑になっていたら
                if (enemy.GetComponent<Renderer>().material.color.r > 0f) continue;

                // 消す
                Destroy(enemy);
                Debug.Log("killed");

                // タイマー設定
                chain_kill_timer = 1.0f;

                // 再生成
                float random_x = Random.Range(-2f, 2f);
                GameObject e = Instantiate((GameObject)Resources.Load("Enemy_copy"));
                e.transform.SetPositionAndRotation(
                    GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(random_x, 1f, 5f),
                    Quaternion.identity);

                // 一体消したらbreak
                break;
            }
#endif
            // ����̓��͒l��p�x(degree)�Ƃ��ĎZ�o����
            var radian = (_input.y > 0f) ?
            Mathf.Acos(Vector2.Dot(_input, Vector2.right)) :
            Mathf.Acos(Vector2.Dot(_input, Vector2.left)) + Mathf.PI;
            var degree = radian * Mathf.Rad2Deg;
            // 角度を保存
            old_angle = degree;

            // Slash�̐���
            //GameObject obj = Instantiate((GameObject)Resources.Load("Slash"));
            GameObject obj = Instantiate(SlashPrefab);
            obj.GetComponent<SlashImageController>().damage = (chain_kill_timer >= 0.4f) ? max_damage : min_damage;
            var obj_rect = obj.GetComponent<RectTransform>();
            obj_rect.anchoredPosition   = new Vector2(transform.position.x, transform.position.y);
            obj_rect.eulerAngles        = new Vector3(0f, 0f, degree);
            var local_scale = obj_rect.localScale;

            if (chain_kill_timer > 0f)
            {
                obj_rect.localScale = local_scale;
            }
            else
            {
                obj_rect.localScale = new Vector3(2f * local_scale.x, 1f, 1f);
            }
            // �e�q�֌W�̐ݒ�
            obj.transform.SetParent(transform.parent);
        }

    }

    public Ray CursolRay()
    {
        return Camera.main.ScreenPointToRay(rect.position);
    }


    private bool IsFindEnemy(Vector2 cursol_position, Vector2 input_direction)
    {
        GameObject[] enemy_collides = GameObject.FindGameObjectsWithTag("EnemyCollide");

        if (enemy_collides == null) return false;

        foreach (var enemy_collide in enemy_collides)
        {
            var collide_position = enemy_collide.GetComponent<RectTransform>().anchoredPosition;

            //var cursol_to_collide_dir = new Vector2(collide_position - cursol_position).normalized;


        }

        return true;
    }

    public void SetChainTimer()
    {
        chain_kill_timer = 0.5f;
    }
}
