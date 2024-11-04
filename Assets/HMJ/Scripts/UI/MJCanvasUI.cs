using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJCanvasUI : MonoBehaviour
{
    [Header("�ݱ� ��ư")]
    public UnityEngine.UI.Button MapRegisterCloseButton;
    public UnityEngine.UI.Button MapContestCloseButton;

    [Header("�� ��� �г� - �� ��� ���� �г� ��ư")]
    public UnityEngine.UI.Button MapConfirmYesButton;
    public UnityEngine.UI.Button MapConfirmNoButton;

    [Header("�� ��� �г� - �� ��� �г� ��ư")]
    public UnityEngine.UI.Button mapRegisterButton;

    [Header("�� ��� �г� - �� ���׽�Ʈ �г� ��ư")]
    public UnityEngine.UI.Button mapContestButton;

    [Header("�� ��� ���� �г� ����")]
    public UnityEngine.UI.Button MapRegisterSuccessCloseButton;

    [Header("�� �κ��丮 �г� - �� �κ��丮 �г� ��ư")]
    public UnityEngine.UI.Button InventoryButton;

    [Header("�� �κ��丮 �г� - �� �κ��丮 �г� ���� ��ư")]
    public UnityEngine.UI.Button InventoryCloseButton;


    [Header("�� ��� ���� �г�")]
    public GameObject mapSuccessRegisterPanel;

    [Header("�� ��� �г�")]
    public GameObject mapRegisterPanel;

    [Header("�� ���׽�Ʈ �г�")]
    public GameObject mapContestPanel;

    [Header("�� ��� ���� �г�")]
    public GameObject mapConfirmPanel;

    // Start is called before the first frame update
    void Start()
    {
        SceneUIManager.GetInstance().RestartSetting(MapContestCloseButton, MapRegisterCloseButton, mapRegisterButton, InventoryButton, InventoryCloseButton, mapContestButton, MapConfirmYesButton, MapConfirmNoButton, MapRegisterSuccessCloseButton, mapContestPanel, mapRegisterPanel, mapConfirmPanel, mapSuccessRegisterPanel);
        SceneUIManager.GetInstance().OffAllMapPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
