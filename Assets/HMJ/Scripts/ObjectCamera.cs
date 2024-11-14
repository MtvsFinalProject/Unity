using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // ���� ȭ�� ���� (16:9)
    public float orthographicSize = 5f; // ���� orthographic size

    public Camera cam;

    void Start()
    {
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        // ���� ȭ�� ����
        float currentAspect = (float)Screen.width / Screen.height;

        // ȭ�� ������ ���� orthographic size ����
        if (currentAspect >= targetAspect)
        {
            cam.orthographicSize = orthographicSize;
        }
        else
        {
            float scale = targetAspect / currentAspect;
            cam.orthographicSize = orthographicSize * scale;
        }
    }

    void Update()
    {
        // ȭ���� ����Ǹ� �ٽ� ����
        if (Screen.width != cam.pixelWidth || Screen.height != cam.pixelHeight)
        {
            AdjustCameraSize();
        }
    }
}
