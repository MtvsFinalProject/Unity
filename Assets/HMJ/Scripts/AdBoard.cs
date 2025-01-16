using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdBoard : MonoBehaviour
{
    public string url;
    // Update is called once per frame
    void Update()
    {
        DesignClick();
    }

    public void DesignClick()
    {
        if (Input.GetMouseButtonDown(0))  // ���콺 Ŭ���� �߻��� ���
        {
            // ���콺 ��ġ -> ���� ��ǥ
            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Pixel Perfect Camera�� �ȼ� ũ�� ����
            worldMousePosition.x = Mathf.Floor(worldMousePosition.x * 100f) / 100f;
            worldMousePosition.y = Mathf.Floor(worldMousePosition.y * 100f) / 100f;

            RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.name == gameObject.name)
                PrivateButton();
        }
    }
    public void PrivateButton()
    {
        Application.OpenURL(url);
    }
}
