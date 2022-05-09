using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(menuName = "Fps_UnityProject_share/FloorDataTable", fileName = "FloorDataTable")]
public class FloorDataTable : ScriptableObject
{
    public GameObject FloorPrefab;
    [SerializeField,Range(5f,10f)] public float floor_distance;
    public FloorData[] FloorDatas;
}

[Serializable]
public class FloorData  // 10m*10mマスの床が持つデータ
{
    public int id = 0;                              // 床の識別子 (要素順で自動設定)
    public FloorInfoMoveSpeed move_speed_state = FloorInfoMoveSpeed.Stop; // 床が持っている速度状態 (この状態をプレイヤーが当たり判定を用いて受け取ることで、プレイヤーの速度を制御させている)
    public string CondExprName;                 // 追加する条件コンポーネントのクラス名
    //public BaseCondExpr CondExprPrefab = null;
    public OnCompleteFloorCondExprEvent CompleteFloorConditionExpr; // Stop床から走り出す条件式
}
public enum FloorInfoMoveSpeed
{
    Stop,       // 待機する,完全停止する床
    SpeedUp,    // 速度を上げ始める床
    Run,        // 最大速度の床
    SpeedDown,  // 速度を落とし始める床
}

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
                // 折り畳み表示
                property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, label);


                fieldRect.y += EditorGUIUtility.singleLineHeight;
                fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

                if (property.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        // 最初の要素を描画
                        property.NextVisible(true);
                        var depth = property.depth;
                        //EditorGUI.PropertyField(fieldRect, _property.firstProperty, true);
                        //fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                        //fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

                        // 2つめ
                        property.NextVisible(false);
                        // depthが最初の要素と同じもののみ処理
                        if (property.depth == depth)
                        {
                            EditorGUI.PropertyField(fieldRect, property, true);
                            fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                            fieldRect.y += EditorGUIUtility.standardVerticalSpacing;
                        }

                        //var _enumproperty = property.FindPropertyRelative(nameof(FloorData.move_speed_state));
                        // secondProperty(move_speed_state)が、FloorInfoMoveSpeed.Stopの場合のみ
                        if (property.enumValueIndex == (int)FloorInfoMoveSpeed.Stop)
                        {
                            var i = 2; // (0,1,2,3)
                            // それ以降の要素を描画
                            while (property.NextVisible(false))
                            {
                                // forthProperty は break
                                //if (i >= 3) { break; }

                                i++;

                                // depthが最初の要素と同じもののみ処理
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

        //PropertyDrawerUtility.DrawDefaultGUI(position, property, label);

        //Init(property);
        //var fieldRect = position;
        //// インデントされた位置のRectが欲しければこっちを使う
        //var indentedFieldRect = EditorGUI.IndentedRect(fieldRect);
        //fieldRect.height = EditorGUIUtility.singleLineHeight;
        //
        //// Prefab化した後プロパティに変更を加えた際に太字にしたりする機能を加えるためPropertyScopeを使う
        //using (new EditorGUI.PropertyScope(fieldRect, label, property))
        //{
        //    // ラベルを表示し、ラベルの右側のプロパティを描画すべき領域のpositionを得る
        //    fieldRect = EditorGUI.PrefixLabel(fieldRect, GUIUtility.GetControlID(FocusType.Passive), label);
        //
        //    // ここでIndentを0に
        //    var preIndent = EditorGUI.indentLevel;
        //    EditorGUI.indentLevel = 0;
        //
        //    // プロパティを描画
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
                // 子要素があれば折り畳み表示
                property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, label);
            }
            else
            {
                // 子要素が無ければラベルだけ表示
                EditorGUI.LabelField(fieldRect, label);
                return;
            }
            fieldRect.y += EditorGUIUtility.singleLineHeight;
            fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

            if (property.isExpanded)
            {

                using (new EditorGUI.IndentLevelScope())
                {
                    // 最初の要素を描画
                    property.NextVisible(true);
                    var depth = property.depth;
                    EditorGUI.PropertyField(fieldRect, property, true);
                    fieldRect.y += EditorGUI.GetPropertyHeight(property, true);
                    fieldRect.y += EditorGUIUtility.standardVerticalSpacing;

                    // それ以降の要素を描画
                    while (property.NextVisible(false))
                    {

                        // depthが最初の要素と同じもののみ処理
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

        // プロパティ名
        height += EditorGUIUtility.singleLineHeight;
        height += EditorGUIUtility.standardVerticalSpacing;

        if (!property.hasChildren)
        {
            // 子要素が無ければラベルだけ表示
            return height;
        }

        if (property.isExpanded)
        {

            // 最初の要素
            property.NextVisible(true);
            var depth = property.depth;
            height += EditorGUI.GetPropertyHeight(property, true);
            height += EditorGUIUtility.standardVerticalSpacing;

            // それ以降の要素
            while (property.NextVisible(false))
            {
                // depthが最初の要素と同じもののみ処理
                if (property.depth != depth)
                {
                    break;
                }
                height += EditorGUI.GetPropertyHeight(property, true);
                height += EditorGUIUtility.standardVerticalSpacing;
            }
            // 最後はスペース不要なので削除
            height -= EditorGUIUtility.standardVerticalSpacing;
        }

        return height;
    }
}