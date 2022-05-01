using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Fps_UnityProject_share/FloorDataTable", fileName = "FloorDataTable")]
public class FloorDataTable : ScriptableObject
{
    [SerializeField] public GameObject FloorPrefab;
    [SerializeField,Range(5f,10f)] public float floor_distance;
    public FloorData[] FloorDatas;
}

[Serializable]
public class FloorData  // 10m*10m�}�X�̏������f�[�^
{
    public int id = 0;                              // ���̎��ʎq (�蓮����U��)
    public FloorInfoMoveSpeed move_speed_state = FloorInfoMoveSpeed.Stop; // ���������Ă��鑬�x��� (���̏�Ԃ��v���C���[�������蔻���p���Ď󂯎�邱�ƂŁA�v���C���[�̑��x�𐧌䂳���Ă���)
    //public Func<bool> FloorConditionExpr;   // Stop�����瑖��o��������
    //public StopFloorConditionExpr FloorConditionExpr;   // Stop�����瑖��o��������
    public OnCompleteFloorCondExprEvent FloorConditionExpr;   // Stop�����瑖��o��������
}
public enum FloorInfoMoveSpeed
{
    Stop,       // �ҋ@����,���S��~���鏰
    SpeedUp,    // ���x���グ�n�߂鏰
    Run,        // �ő呬�x�̏�
    SpeedDown,  // ���x�𗎂Ƃ��n�߂鏰
}

[CustomPropertyDrawer(typeof(FloorData))]
public class FloorDatDrawer : PropertyDrawer
{
    //private FloorData _target;

    private class PropertyData
    {
        public SerializedProperty firstProperty;
        public SerializedProperty secondProperty;
        public SerializedProperty thirdProperty;
    }

    //private SerializedProperty _property_0;
    //private SerializedProperty _property_1;
    //private SerializedProperty _property_2;

    private Dictionary<string, PropertyData> _propertyDataPerPropertyPath = new Dictionary<string, PropertyData>();
    private PropertyData _property;

    private void Init(SerializedProperty property)
    {
        if (_propertyDataPerPropertyPath.TryGetValue(property.propertyPath, out _property))
        {
            return;

        }
        _property = new PropertyData();
        _property.firstProperty = property.FindPropertyRelative(nameof(FloorData.id));
        _property.secondProperty = property.FindPropertyRelative(nameof(FloorData.move_speed_state));
        _property.thirdProperty = property.FindPropertyRelative(nameof(FloorData.FloorConditionExpr));
        _propertyDataPerPropertyPath.Add(property.propertyPath, _property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {


        {
            Init(property);

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
                        EditorGUI.PropertyField(fieldRect, _property.firstProperty, true);
                        fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                        fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

                        // 2�ڂ̗v�f��`��
                        property.NextVisible(false);
                        if (property.depth != depth) // depth���ŏ��̗v�f�Ɠ������̂̂ݏ���
                        {
                            return;
                        }
                        EditorGUI.PropertyField(fieldRect, _property.secondProperty, true);
                        fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                        fieldRect.y += EditorGUIUtility.standardVerticalSpacing;


                        // _property.secondProperty(move_speed_state)���AFloorInfoMoveSpeed.Stop�̏ꍇ�̂�
                        if (_property.secondProperty.enumValueIndex != (int)FloorInfoMoveSpeed.Stop)
                        {
                            return;
                        }
                        // 3�ڂ̗v�f��`��
                        property.NextVisible(false);
                        if (property.depth != depth) // depth���ŏ��̗v�f�Ɠ������̂̂ݏ���
                        {
                            return;
                        } 
                        EditorGUI.PropertyField(fieldRect, _property.thirdProperty, true);
                        fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                        fieldRect.y += EditorGUIUtility.standardVerticalSpacing;
                    }
                }
            }

        }
        //PropertyDrawerUtility.DrawDefaultGUI(position, property, label);

        //Init(property);
        //var fieldRect = position;
        //// �C���f���g���ꂽ�ʒu��Rect���~������΂��������g��
        //var indentedFieldRect = EditorGUI.IndentedRect(fieldRect);
        //fieldRect.height = EditorGUIUtility.singleLineHeight;
        //
        //// Prefab��������v���p�e�B�ɕύX���������ۂɑ����ɂ����肷��@�\�������邽��PropertyScope���g��
        //using (new EditorGUI.PropertyScope(fieldRect, label, property))
        //{
        //    // ���x����\�����A���x���̉E���̃v���p�e�B��`�悷�ׂ��̈��position�𓾂�
        //    fieldRect = EditorGUI.PrefixLabel(fieldRect, GUIUtility.GetControlID(FocusType.Passive), label);
        //
        //    // ������Indent��0��
        //    var preIndent = EditorGUI.indentLevel;
        //    EditorGUI.indentLevel = 0;
        //
        //    // �v���p�e�B��`��
        //    var firstRect = fieldRect;
        //    //firstRect.width /= 3;
        //    EditorGUI.PropertyField(firstRect, _property.firstProperty, GUIContent.none);
        //    EditorGUI.indentLevel = preIndent;
        //
        //    //var dashRect = fieldRect;
        //    //dashRect.xMin += firstRect.width;
        //    //dashRect.width = 10;
        //    //EditorGUI.LabelField(dashRect, "-");
        //    //EditorGUI.indentLevel = preIndent;
        //
        //    var secondRect = fieldRect;
        //    //secondRect.xMin += firstRect.width + dashRect.width;
        //    //secondRect.width = fieldRect.width - (firstRect.width + dashRect.width);
        //    EditorGUI.PropertyField(secondRect, _property.secondProperty, GUIContent.none);
        //    EditorGUI.indentLevel = preIndent;
        //
        //
        //    var thirdRect = fieldRect;
        //    EditorGUI.PropertyField(thirdRect, _property.thirdProperty, GUIContent.none);
        //    EditorGUI.indentLevel = preIndent;
        //}
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //Init(property);

        //return EditorGUIUtility.singleLineHeight;
        return PropertyDrawerUtility.GetDefaultPropertyHeight(property, label);
    }

#if FALSE
    private float LineHeight { get { return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; } }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Init(property);
        var fieldRect = position;
        // �C���f���g���ꂽ�ʒu��Rect���~������΂��������g��
        var indentedFieldRect = EditorGUI.IndentedRect(fieldRect);
        fieldRect.height = LineHeight;


        // Prefab��������v���p�e�B�ɕύX���������ۂɑ����ɂ����肷��@�\�������邽��PropertyScope���g��
        using (new EditorGUI.PropertyScope(fieldRect, label, property))
        {
            // �v���p�e�B����\�����Đ܂��ݏ�Ԃ𓾂�
            property.isExpanded = EditorGUI.Foldout(new Rect(fieldRect), property.isExpanded, label);
            if (property.isExpanded)
            {

                using (new EditorGUI.IndentLevelScope())
                {
                    // Name��`��
                    fieldRect.y += LineHeight;
                    EditorGUI.PropertyField(new Rect(fieldRect), _property.firstProperty);

                    // Type��`��
                    fieldRect.y += LineHeight;
                    EditorGUI.PropertyField(new Rect(fieldRect), _property.secondProperty);

                    // Age��`��
                    fieldRect.y += LineHeight;
                    EditorGUI.PropertyField(new Rect(fieldRect), _property.thirdProperty);
                }
            }
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        Init(property);
        // (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) x �s�� �ŕ`��̈�̍��������߂�
        return LineHeight * (property.isExpanded ? 4 : 1);
    }
#endif
    
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