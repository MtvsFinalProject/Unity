using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GH.DataManager;

public class TutorialText : MonoBehaviour
{
    private TMP_Text tutorialText;
    private string mapTutorialText;
    private float delayTime = 0.075f;

    void Start()
    {
        tutorialText = GetComponentInChildren<TMP_Text>();
        TextChange(MapType.MyClassroom);
    }

    void Update()
    {

    }

    public void TextChange(MapType mapType)
    {
        switch (mapType)
        {
            case MapType.MyClassroom:
                
                break;
            case MapType.School:
                break;
            case MapType.Square:
                break;
            case MapType.QuizSquare:
                break;

        }
    }
    IEnumerator ClassText()
    {
        mapTutorialText = "���̸�Ʋ���� ������ �� ȯ����!\r\n���� �ο��!!";
        StartCoroutine(TextPrint(mapTutorialText));
        yield return new WaitUntil(() => Input.touchCount == 1 || Input.GetMouseButtonDown(0));
        mapTutorialText = "�̰��� ������ �����̾�! \r\n�繰�Կ��� �ٸ� ����� �湮�ؼ� ���� ������ �� �� �־�!";
        StartCoroutine(TextPrint(mapTutorialText));
    }




    // �ؽ�Ʈ �ϳ��� ����� �ϱ�
    IEnumerator TextPrint(string text)
    {
        tutorialText.text = "";
        int count = 0;

        while(count != text.Length)
        {
            if(count < text.Length)
            {
                tutorialText.text += text[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(delayTime);
        }
    }
}
