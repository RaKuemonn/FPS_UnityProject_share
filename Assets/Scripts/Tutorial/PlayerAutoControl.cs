using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoControl : MonoBehaviour
{
    [SerializeField] private GameObject m_player;                    // �v���C���[
    [SerializeField, Range(0f, 1f)] public float speed_rate = 1f;    // �ړ����x�̔{�� (1.0f ~ 0.0f)
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
