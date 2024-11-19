using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [Header("�ۼ� �г�")]
    public GameObject createPanel;
    public Button saveBoardButton;
    public TMP_InputField titleInputField;
    public TMP_InputField contentInputField;
    [Header("���� �г�")]
    public GameObject contentPanel;
    public TMP_InputField comentInputField;
    public Button saveComentButton;
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void SetCreatePanel()
    {
        if (createPanel.activeSelf)
        {
            createPanel.SetActive(false);
            titleInputField.text = "";
            contentInputField.text = "";
            saveBoardButton.interactable = false;
        }
        else
        {
            createPanel.SetActive(true);
        }
    }
    public void CreatePanelContentsChanged()
    {
        print(titleInputField.text.Length + "/" + contentInputField.text.Length);
        if (titleInputField.text.Length == 0 || contentInputField.text.Length == 0)
            saveBoardButton.interactable = false;
        else
            saveBoardButton.interactable = true;
    }
    public void SaveBoard()
    {
        // ���� ��û ���

        SetCreatePanel();
    }
    public void SetContentPanel()
    {
        if (contentPanel.activeSelf)
        {
            contentPanel.SetActive(false);
            comentInputField.text = "";
            saveComentButton.interactable = false;
        }
        else
        {
            contentPanel.SetActive(true);
        }
    }
    public void ComentInputFieldChanged()
    {
        if (comentInputField.text.Length == 0)
            saveComentButton.interactable = false;
        else
            saveComentButton.interactable = true;
    }
    public void SaveComent()
    {
        // ��� ���� ��û ���

        comentInputField.text = "";
    }

    public void LoadBoardData()
    {
        
    }
    public void LoadComentsData()
    {

    }
}
