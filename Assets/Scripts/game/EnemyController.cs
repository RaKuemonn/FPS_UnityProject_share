using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float move_speed;                // �ړ��X�s�[�h (���J���Ă���̂ŁAInspector�ŕύX�\)

    private GameObject player;              // �v���C���[�̈ʒu�Q�Ɨp

    private const float destroy_distance = 1f - float.Epsilon;

    private bool is_create_collide = true;  // �G�������ɁA�����蔻��p��EnemyCollideOnScreen�𐶐�����̂��B 

    private bool is_cutted = false;         // �a���Ă���̂��B �a���Ă�����ʏ�X�V�͂��Ȃ��B

    private bool is_original = true;        // �a��ꂽ�Ƃ��ɂ����炵���������ꂽ�����A���̂��̂����f����悤


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        CreateCollideOnCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (is_cutted) return;

        UpdatePosition();
        CollidePlayer();
    }

    public void SetCutPerformance(bool is_original_)
    {
        var rigid = transform.gameObject.AddComponent<Rigidbody>();
        is_cutted = true;
        is_create_collide = false;
        is_original = is_original_;

        float x = !is_original_ ? 1f : 0f;
        rigid.AddForce(new Vector3(x, 0.1f, 0.0f), ForceMode.Impulse);
    }

    private void UpdatePosition()
    {
        if (player == null) return;

        var to_player_vec = PlayerPosition() - transform.position;

        var to_player_dir = to_player_vec.normalized;
        transform.Translate(to_player_dir * move_speed * Time.deltaTime);
    }

    private void CollidePlayer()
    {
        var distance = Vector3.Distance(PlayerPosition(), transform.position);

        if (distance > destroy_distance) return;

        // ������߂����
        Destroy(gameObject);

        //// �Đ���
        //GameObject obj = Instantiate((GameObject)Resources.Load("Enemy_copy"));
        ////obj.transform.Translate();
        //obj.transform.SetPositionAndRotation(PlayerPosition() + new Vector3(0f, 1f, 10f), Quaternion.identity);
        Debug.Log("enemy die");
    }

    private Vector3 PlayerPosition()
    {
        if (player)
        {
            var p_pos = player.transform.position;
            return new Vector3(p_pos.x, p_pos.y + 0.5f, p_pos.z);
        }
        else
        {
            return new Vector3(0f, 0f, 0f);
        }

    }

    private void CreateCollideOnCanvas()
    {
        // �a��ꂽ�Ƃ��ɐ������ꂽ�I�u�W�F�N�g�łȂ���΁iSetCutPerformance()�ŁA�ύX����Ă��Ȃ���΁j
        if (is_create_collide == false) return;

        // �����蔻��p�̃I�u�W�F�N�g��Canvas���ɐ���
        GameObject obj = Instantiate(
            (GameObject)Resources.Load("EnemyCollideOnScreen")
        );
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.GetComponent<EnemyCollide>().SetTarget(gameObject);
    }

    void OnDestroy()
    {

        if (isQuitting) return;

        if (is_original == false) return;

        // �Đ���
        float random_x = Random.Range(-2f, 2f);

        GameObject e = Instantiate((GameObject)Resources.Load("Enemy_test"));
        e.transform.SetPositionAndRotation(
            GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(random_x, 1f, 5f),
            Quaternion.identity);
    }

    private static bool isQuitting = false;
    void OnApplicationQuit()
    {

        isQuitting = true;

    }
}
