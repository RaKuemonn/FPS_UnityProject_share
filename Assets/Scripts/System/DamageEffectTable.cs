using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Table/DamageEffectTable",fileName = "DamageEffectTable")]
public class DamageEffectTable : ScriptableObject
{
    public Effect[] effects;
}

[Serializable]
public class Effect
{
    public Image Image;
}