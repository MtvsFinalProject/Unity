using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("�� �κ��丮 ���� �г� - �� �κ��丮 ���� �г� ���� ��ư")]
    public UnityEngine.UI.Button InventoryErrorCloseButton;

    [Header("���� ��ư - ����")]
    public UnityEngine.UI.Button HeadSetOnOffButton;

    [Header("���� ��ư - ����ũ")]
    public UnityEngine.UI.Button MicroPhoneOnOffButton;

    [Header("�� ��� ���� �г�")]
    public GameObject mapSuccessRegisterPanel;

    [Header("�� ��� �г�")]
    public GameObject mapRegisterPanel;

    [Header("�� ���׽�Ʈ �г�")]
    public GameObject mapContestPanel;

    [Header("�� ��� ���� �г�")]
    public GameObject mapConfirmPanel;

    [Header("����X �г�")]
    public GameObject HeadSetXPanel;

    [Header("����ũX �г�")]
    public GameObject MirocoPhoneXPanel;

    [Header("�� �κ��丮 ���� �г� - ������ ���� ����")]
    public GameObject mapInventoryErrorPanel;

    // Start is called before the first frame update
    void Start()
    {

        SettingSoundPanel();
    }
    
    public void SettingSoundPanel()
    {
        if (!HeadSetOnOffButton || !MicroPhoneOnOffButton)
            return;

        //SoundImageSetting();

        HeadSetOnOffButton.onClick.AddListener(VoiceManager.instance.HeadSetOnOff);
        HeadSetOnOffButton.onClick.AddListener(SoundImageSetting);

        MicroPhoneOnOffButton.onClick.AddListener(VoiceManager.instance.MicrophoneOnOff);
        MicroPhoneOnOffButton.onClick.AddListener(SoundImageSetting);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HeadSetImageSetting()
    {
        VoiceManager.instance.SettingPlayerSpeaker();
        HeadSetXPanel.GetComponent<Image>().enabled = !VoiceManager.instance.GetHeadSetOnOff();
    }

    public void MirocoPhoneSetting()
    {
        MirocoPhoneXPanel.GetComponent<Image>().enabled = !VoiceManager.instance.GetMicrophoneOnOff();
    }

    public void SoundImageSetting()
    {
        HeadSetImageSetting();
        MirocoPhoneSetting();
    }
}

