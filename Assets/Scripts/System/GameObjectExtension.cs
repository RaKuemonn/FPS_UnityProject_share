//  GameObjectExtension.cs
//  http://kan-kikuchi.hatenablog.com/entry/GetComponentInParentAndChildren
//
//  Created by kikuchikan on 2015.08.25.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// GameObject�̊g���N���X
/// </summary>
public static class GameObjectExtension
{

    /// <summary>
    /// �e��q�I�u�W�F�N�g���܂߂��͈͂���w��̃R���|�[�l���g���擾����
    /// </summary>
    public static T GetComponentInParentAndChildren<T>(this GameObject gameObject) where T : UnityEngine.Component
    {

        if (gameObject.GetComponentInParent<T>() != null)
        {
            return gameObject.GetComponentInParent<T>();
        }
        if (gameObject.GetComponentInChildren<T>() != null)
        {
            return gameObject.GetComponentInChildren<T>();
        }

        return gameObject.GetComponent<T>();
    }

    /// <summary>
    /// �e��q�I�u�W�F�N�g���܂߂��͈͂���w��̃R���|�[�l���g��S�Ď擾����
    /// </summary>
    public static List<T> GetComponentsInParentAndChildren<T>(this GameObject gameObject) where T : UnityEngine.Component
    {
        List<T> _list = new List<T>(gameObject.GetComponents<T>());

        _list.AddRange(new List<T>(gameObject.GetComponentsInChildren<T>()));
        _list.AddRange(new List<T>(gameObject.GetComponentsInParent<T>()));

        return _list;
    }

}