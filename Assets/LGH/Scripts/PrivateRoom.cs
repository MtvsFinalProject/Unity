using Photon.Realtime;
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
    public string roomPassword = "99999";
    //�� �н����� ��ǲ �ʵ�
    public TMP_InputField passWordInputField;

    //�� �н����� �ȳ� �ؽ�Ʈ
    public TMP_Text passWordText;

    // �н����� Ʋ���� �� �ؽ�Ʈ
    public TMP_Text passWordWrongText;

    //�н����� ���� ��ư
    public Button passWordSubmitButton;
    public Button passWordExitButton;

    private BoxCollider2D boxCollider;

    private bool activeRoom = false;

    void Start()
    {
        passWordPanel.SetActive(false);
        passWordSubmitButton.onClick.AddListener(PassWordCheck);
        passWordExitButton.onClick.AddListener(PassWordExit);
        boxCollider = GetComponent<BoxCollider2D>();
        passWordWrongText.enabled = false;

    }

    void Update()
    {

    }

    private void OnUI(GameObject player)
    {
        passWordInputField.text = "";
        passWordPanel.SetActive(true);

        if (activeRoom == false)
        {
            passWordText.text = "��й�ȣ �������ּ���";
        }
        else
        {
            passWordText.text = "��й�ȣ�� �Է����ּ���";
        }
    }

    public void PassWordCheck()
    {
        if (!activeRoom)
        {
            roomPassword = passWordInputField.text;
            boxCollider.isTrigger = true;
            passWordPanel.SetActive(false);
            activeRoom = true;
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
                passWordWrongText.gameObject.SetActive(true);
            }

        }

    }

    public void PassWordExit()
    {
        passWordPanel.SetActive(false);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnUI(collision.gameObject);
            playersList.Add(collision.gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
          //  playersList.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for(int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i] == collision.gameObject)
                {
                    playersList.RemoveAt(i);
                    boxCollider.isTrigger = false;
                    break;
                }
            }
            if(playersList.Count == 0)
            {
                activeRoom = false;
            }

        }
    }
}
