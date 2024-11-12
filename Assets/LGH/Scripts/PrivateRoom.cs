using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrivateRoom : MonoBehaviour
{
    public List<GameObject> playersList = new List<GameObject>();
    public GameObject passWordPanel;

    //�� �н�����
    public string roomPassword;
    //�� �н����� ��ǲ �ʵ�
    public TMP_InputField passWordInputField;

    //�� �н����� �ȳ� �ؽ�Ʈ
    public TMP_Text passWordText;

    // �н����� Ʋ���� �� �ؽ�Ʈ
    public TMP_Text passWordWrongText;

    //�н����� ���� ��ư
    public Button passWordSubmitButton;

    private BoxCollider2D boxCollider;

    void Start()
    {
        passWordPanel.SetActive(false);
        passWordSubmitButton.onClick.AddListener(PassWordCheck);
        boxCollider = GetComponent<BoxCollider2D>();
        passWordWrongText.enabled = false;

    }

    void Update()
    {

    }

    private void OnUI(GameObject player)
    {
        passWordPanel.SetActive(true);

        if (playersList.Count < 1)
        {
            passWordText.text = "��й�ȣ ����";
            playersList.Add(player);
        }
        else
        {
            passWordText.text = "��й�ȣ";
        }
    }

    public void PassWordCheck()
    {
        if (playersList.Count < 1)
        {
            roomPassword = passWordInputField.text;
            boxCollider.isTrigger = true;
            passWordPanel.SetActive(false);
        }
        else
        {
            if (passWordInputField.text == roomPassword)
            {
                boxCollider.isTrigger = true;
                passWordPanel.SetActive(false);
            }
            else
            {
                passWordWrongText.enabled = true;
            }

        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnUI(collision.gameObject);

        }
    }
}
