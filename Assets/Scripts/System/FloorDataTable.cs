using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Fps_UnityProject_share/FloorDataTable", fileName = "FloorDataTable")]
public class FloorDataTable : ScriptableObject
{
    public GameObject FloorPrefab;
    public const float floor_distance = 10f;
    public FloorData[] FloorDatas;
}

[Serializable]
public class FloorData  // 10m*10m�}�X�̏������f�[�^
{
    public int id = 0;                                                      // ���̎��ʎq (�v�f���Ŏ����ݒ�)
    public FloorInfoMoveSpeed move_speed_state = FloorInfoMoveSpeed.Stop;   // ���������Ă��鑬�x��� (���̏�Ԃ��v���C���[�������蔻���p���Ď󂯎�邱�ƂŁA�v���C���[�̑��x�𐧌䂳���Ă���)
    [SerializeField]public BaseCondExprData CondExprData;                   // �ǉ���������R���|�[�l���g�̃f�[�^ (���̃f�[�^���g���āA�R���|�[�l���g��Floor�I�u�W�F�N�g�ɒǉ�����)
    public OnCompleteFloorCondExprEvent CompleteFloorConditionExpr;         // Stop�����瑖��o��������
}





public enum FloorInfoMoveSpeed
{
    Stop,       // �ҋ@����,���S��~���鏰
    SpeedUp,    // ���x���グ�n�߂鏰
    Run,        // �ő呬�x�̏�
    SpeedDown,  // ���x�𗎂Ƃ��n�߂鏰
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(FloorData))]
public class FloorDatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        {
            var fieldRect = position;
            fieldRect.height = EditorGUIUtility.singleLineHeight;
            using (new EditorGUI.PropertyScope(fieldRect, label, property))
            {
                // �܂��ݕ\��
                property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, label);


                fieldRect.y += EditorGUIUtility.singleLineHeight;
                fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

                if (property.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        // �ŏ��̗v�f��`��
                        property.NextVisible(true);
                        var depth = property.depth;
                        //EditorGUI.PropertyField(fieldRect, _property.firstProperty, true);
                        //fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                        //fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

                        // 2��
                        property.NextVisible(false);
                        // depth���ŏ��̗v�f�Ɠ������̂̂ݏ���
                        if (property.depth == depth)
                        {
                            EditorGUI.PropertyField(fieldRect, property, true);
                            fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                            fieldRect.y += EditorGUIUtility.standardVerticalSpacing;
                        }

                        //var _enumproperty = property.FindPropertyRelative(nameof(FloorData.move_speed_state));
                        // secondProperty(move_speed_state)���AFloorInfoMoveSpeed.Stop�̏ꍇ�̂�
                        if (property.enumValueIndex == (int)FloorInfoMoveSpeed.Stop)
                        {
                            var i = 2; // (0,1,2,3)
                            // ����ȍ~�̗v�f��`��
                            while (property.NextVisible(false))
                            {
                                // forthProperty �� break
                                //if (i >= 3) { break; }

                                i++;

                                // depth���ŏ��̗v�f�Ɠ������̂̂ݏ���
                                if (property.depth != depth)
                                {
                                    break;
                                }
                                EditorGUI.PropertyField(fieldRect, property, true);
                                fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                                fieldRect.y += EditorGUIUtility.standardVerticalSpacing;
                            }
                        }

                        
                    }
                }
            }

        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return PropertyDrawerUtility.GetDefaultPropertyHeight(property, label);
    }
    
}



public static class PropertyDrawerUtility
{
    public static void DrawDefaultGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property = property.serializedObject.FindProperty(property.propertyPath);
        var fieldRect = position;
        fieldRect.height = EditorGUIUtility.singleLineHeight;

        using (new EditorGUI.PropertyScope(fieldRect, label, property))
        {
            if (property.hasChildren)
            {
                // �q�v�f������ΐ܂��ݕ\��
                property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, label);
            }
            else
            {
                // �q�v�f��������΃��x�������\��
                EditorGUI.LabelField(fieldRect, label);
                return;
            }
            fieldRect.y += EditorGUIUtility.singleLineHeight;
            fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

            if (property.isExpanded)
            {

                using (new EditorGUI.IndentLevelScope())
                {
                    // �ŏ��̗v�f��`��
                    property.NextVisible(true);
                    var depth = property.depth;
                    EditorGUI.PropertyField(fieldRect, property, true);
                    fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                    fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

                    // ����ȍ~�̗v�f��`��
                    while (property.NextVisible(false))
                    {

                        // depth���ŏ��̗v�f�Ɠ������̂̂ݏ���
                        if (property.depth != depth)
                        {
                            break;
                        }
                        EditorGUI.PropertyField(fieldRect, property, true);
                        fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                        fieldRect.y += EditorGUIUtility.standardVerticalSpacing;
                    }
                }
            }
        }
    }

    public static float GetDefaultPropertyHeight(SerializedProperty property, GUIContent label)
    {
        property = property.serializedObject.FindProperty(property.propertyPath);
        var height = 0.0f;

        // �v���p�e�B��
        height += EditorGUIUtility.singleLineHeight;
        height += EditorGUIUtility.standardVerticalSpacing;

        if (!property.hasChildren)
        {
            // �q�v�f��������΃��x�������\��
            return height;
        }

        if (property.isExpanded)
        {

            // �ŏ��̗v�f
            property.NextVisible(true);
            var depth = property.depth;
            height += EditorGUI.GetPropertyHeight(property, true);
            height += EditorGUIUtility.standardVerticalSpacing;

            // ����ȍ~�̗v�f
            while (property.NextVisible(false))
            {
                // depth���ŏ��̗v�f�Ɠ������̂̂ݏ���
                if (property.depth != depth)
                {
                    break;
                }
                height += EditorGUI.GetPropertyHeight(property, true);
                height += EditorGUIUtility.standardVerticalSpacing;
            }
            // �Ō�̓X�y�[�X�s�v�Ȃ̂ō폜
            height -= EditorGUIUtility.standardVerticalSpacing;
        }

        return height;
    }
}

#endif