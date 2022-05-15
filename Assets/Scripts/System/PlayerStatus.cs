using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameObjectData/PlayerStatus", fileName = "PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    [SerializeField, Range(0f, 100f)] public float max_hp;

    public float current_hp;

    [SerializeField, Range(3f, 10f)] public float sword_area_radius;
}
