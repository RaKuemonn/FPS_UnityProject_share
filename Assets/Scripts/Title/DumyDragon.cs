using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumyDragon : TitleEnemyController
{
    private static readonly int hashMove = Animator.StringToHash("MoveSpeed");

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();

        m_animator.SetFloat(hashMove, 0.3f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
