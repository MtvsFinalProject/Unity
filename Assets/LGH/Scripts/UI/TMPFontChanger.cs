using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TMPFontChanger : MonoBehaviour
{
//    public TMP_FontAsset newFont; // ���� ������ ��Ʈ
//    public TMP_FontAsset targetFont; // �����ϰ��� �ϴ� ���� ��Ʈ

//    [ContextMenu("Change All TMP Fonts In Scene")]
//    public void ChangeAllFontsInScene()
//    {
//        // ���� ���� ��� TMP �ؽ�Ʈ ������Ʈ ã��
//        TextMeshProUGUI[] sceneTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
//        TextMeshPro[] sceneTMP = Resources.FindObjectsOfTypeAll<TextMeshPro>();

//        int changedCount = 0;

//        // UI �ؽ�Ʈ(TextMeshProUGUI) ��Ʈ ����
//        foreach (TextMeshProUGUI text in sceneTexts)
//        {
//            if (targetFont == null || text.font == targetFont)
//            {
//                Undo.RecordObject(text, "Change Font");
//                text.font = newFont;
//                changedCount++;
//            }
//        }

//        // World �ؽ�Ʈ(TextMeshPro) ��Ʈ ����
//        foreach (TextMeshPro tmp in sceneTMP)
//        {
//            if (targetFont == null || tmp.font == targetFont)
//            {
//                Undo.RecordObject(tmp, "Change Font");
//                tmp.font = newFont;
//                changedCount++;
//            }
//        }

//        Debug.Log($"Changed {changedCount} texts to new font: {newFont.name}");
//    }

//    [ContextMenu("Change All TMP Fonts In Project")]
//    public void ChangeAllFontsInProject()
//    {
//        // ������Ʈ ���� ��� ������ ã��
//        string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab");
//        int changedCount = 0;

//        foreach (string guid in prefabPaths)
//        {
//            //string path = AssetDatabase.GUIDToAssetPath(guid);
//            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

//            // �������� ��� TMP ������Ʈ ã��
//            TextMeshProUGUI[] uiTexts = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
//            TextMeshPro[] tmpTexts = prefab.GetComponentsInChildren<TextMeshPro>(true);

//            bool prefabModified = false;

//            // UI �ؽ�Ʈ ����
//            foreach (TextMeshProUGUI text in uiTexts)
//            {
//                if (targetFont == null || text.font == targetFont)
//                {
//                    text.font = newFont;
//                    prefabModified = true;
//                    changedCount++;
//                }
//            }

//            // World �ؽ�Ʈ ����
//            foreach (TextMeshPro tmp in tmpTexts)
//            {
//                if (targetFont == null || tmp.font == targetFont)
//                {
//                    tmp.font = newFont;
//                    prefabModified = true;
//                    changedCount++;
//                }
//            }

//            // ��������� �ִ� ��쿡�� ������ ����
//            if (prefabModified)
//            {
//                PrefabUtility.SavePrefabAsset(prefab);
//            }
//        }

//        Debug.Log($"Changed {changedCount} texts in project prefabs to new font: {newFont.name}");
//    }
}
