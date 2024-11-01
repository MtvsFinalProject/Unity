using GH;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capture : MonoBehaviour
{
    public RenderTexture m_CaptureRenderTexture;

    public string CaptureRenderTexture()
    {
        // ���� �ؽ��� ũ�⸸ŭ 2d �ؽ��� ����
        Texture2D texture = new Texture2D(m_CaptureRenderTexture.width, m_CaptureRenderTexture.height);

        // �ش� ���� �ؽ��ĸ� Ȱ��ȭ
        RenderTexture.active = m_CaptureRenderTexture;

        texture.ReadPixels(new Rect(0, 0, m_CaptureRenderTexture.width, texture.height), 0, 0);
        texture.Apply();

        
        byte[] bytes = texture.EncodeToPNG();
        string path = Application.dataPath + "/" + DateTime.Now.ToString(("yyyy_MM_dd_HH_mm_ss_")) + DataManager.instance.playerName + ".png";
        System.IO.File.WriteAllBytes(path, bytes);

        Debug.Log(Application.dataPath);
        RenderTexture.active = null;

        return path;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


}
