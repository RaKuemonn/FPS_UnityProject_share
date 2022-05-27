using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowStopControl : BaseEnemy
{

    [SerializeField] private CommandSpecifyUI commandSpecifyUi;

    // Start is called before the first frame update
    void Start()
    {
        // ìñÇΩÇËîªíËÇÃê∂ê¨
        //CreateCollideOnCanvas();
    }

    public override void OnDead()
    {
        base.OnDead();

        commandSpecifyUi?.Cut();
    }

}
