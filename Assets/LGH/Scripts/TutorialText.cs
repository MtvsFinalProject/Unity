using GH;
using Photon.Pun;
using SW;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static GH.DataManager;
using static HttpManager;
[System.Serializable]
public struct MapCount
{
    public int mapId;
    public string mapType;
    public int userId;
    public int count;
}
public class TutorialText : MonoBehaviour
{
    private TMP_Text tutorialText;
    private float delayTime = 0.05f;

    private bool texting = false;

    public TutorialManager tutorialManager;

    void Start()
    {
        tutorialText = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {

    }

    public void TextChange(MapType mapType)
    {
        switch (mapType)
        {
            case MapType.MyClassroom:
                StartCoroutine(ClassText());
                break;
            case MapType.School:
                StartCoroutine(TextPrintDone("�̰��� �б���! ���� �б� ģ���鳢�� ���� �� �ִ� ��������! \r\n���� �б� ģ����� �̻� �б��� ������!"));
                break;
            case MapType.Square:
                StartCoroutine(TextPrintDone("�̰��� ������ �����̾�!\r\n��� ģ������ ���� �� �ִ� ��������!\r\n�׸��� �پ��� ��Ÿ��� �����־�!"));
                break;
            case MapType.QuizSquare:
                StartCoroutine(TextPrintDone("�̰��� ���� �����̾�!\r\nOX��� ��级�� �� �� �־�!"));
                break;

        }
    }
    IEnumerator ClassText()
    {
        yield return new WaitUntil(() => tutorialManager.lawStart);
        yield return null;

        StartCoroutine(TextPrint("���̸�Ʋ���� ������ �� ȯ����!\r\n���� �ο��!!\r\n���ο� ģ������ ��� �� �ְ� �����ְ� �־�!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        yield return null;
        StartCoroutine(TextPrint("�̰��� ������ �����̾�! \r\n�繰�Կ��� �ٸ� ����� �湮�ؼ� ���� ������ �� �� �־�!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        yield return null;
        StartCoroutine(TextPrintDone("�׷� ��� ���б��� �ٴϰ� �ִ��� �˷��� �� ������??"));
    }


    // �ؽ�Ʈ �ϳ��� ����� �ϱ�
    IEnumerator TextPrintDone(string text)
    {
        SoundManager.instance.InteractMainEftSound(SoundManager.EInteractEftType.WRITE_DATA);

        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });
        yield return new WaitForSeconds(0.3f);

        int count = 0;
        tutorialText.text = "";

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                tutorialText.text += text[count].ToString();
                count++;
                if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
                {
                    tutorialText.text = text;
                    count = text.Length;
                    break;
                }
            }
            yield return new WaitForSeconds(delayTime);
        }

        if (count == text.Length)
        {
            SoundManager.instance.StopEftSound();
            yield return new WaitUntil(() => Input.touchCount == 1 || Input.GetMouseButtonDown(0));
            tutorialText.text = "";
            gameObject.SetActive(false);
        }

    }

    // �ؽ�Ʈ �ϳ��� ����� �ϱ�
    IEnumerator TextPrint(string text)
    {
        SoundManager.instance.InteractMainEftSound(SoundManager.EInteractEftType.WRITE_DATA);

        yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });
        yield return new WaitForSeconds(0.3f);
        int count = 0;
        texting = false;
        tutorialText.text = "";

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                tutorialText.text += text[count].ToString();
                count++;
                if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
                {
                    tutorialText.text = text;
                    count = text.Length;
                    break;
                }
            }
            yield return new WaitForSeconds(delayTime);
        }

        if (count == text.Length)
        {
            texting = true;
            SoundManager.instance.StopEftSound();
        }
    }

 
}
