using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Table/EffectTable",fileName = "EffectTable")]
public class EffectTable : ScriptableObject
{
    public Effect[] effects;
}

[Serializable]
public class Effect
{
    public Sprite Sprite;
}