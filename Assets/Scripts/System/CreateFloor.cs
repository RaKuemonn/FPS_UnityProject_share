using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFloor : MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]  // シーン読み込み前に関数を実行する属性
    private static void InitializeBeforeSceneLoad()
    {
        //var floorManager = Instantiate(FloorM);

    }
}
  