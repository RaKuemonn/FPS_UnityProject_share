using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEnemyController : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed;

    protected Vector3 velocity;

    protected Animator m_animator;

    [SerializeField]
    protected float width;
}
