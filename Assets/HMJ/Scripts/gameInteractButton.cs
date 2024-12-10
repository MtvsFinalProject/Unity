using GH;
using SW;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class gameInteractButton : MonoBehaviour
{
    private static gameInteractButton instance;

    private bool bButton;

    public GameObject onOffObject;

    public static gameInteractButton GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        InitButton();
    }

    public void InitButton()
    {
        bButton = false;
    }

    IEnumerator interactButtonDown()
    {
        Debug.Log("Button Pressed");
        bButton = true;
        yield return null;
        bButton = false;
    }

    public void ButtonDown()
    {
        if(!bButton)
            StartCoroutine(interactButtonDown());
    }

    public bool GetButtonDown()
    {
        return bButton;
    }

    public bool InTriggerZone()
    {
        if (!DataManager.instance.player || PhotonNetMgr.instance.roomName != "������ ����")
            return false;

        Vector2[] OmocPosition = { new Vector2(-19.3f, -22.7f), new Vector2(-7.6f, -22.6f) };
        foreach (Vector2 pos in OmocPosition)
        {
            if (Vector2.Distance(DataManager.instance.player.transform.position, pos) <= 8.0f)
                return true;
        }
        return false;
    }

    private void OnEnable()
    {
        StartCoroutine(reEnableAfterCondition(false));
    }

    private IEnumerator reEnableAfterCondition(bool bValue)
    {
        while (!InTriggerZone() == bValue) // ������ ������ ������ ���
        {
            yield return null; // �����Ӹ��� üũ
        }
        onOffObject.SetActive(bValue); // �ٽ� Ȱ��ȭ

        StartCoroutine(reEnableAfterCondition(!bValue));
    }


}
