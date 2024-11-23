using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using static GH.DataManager;

public class TutorialText : MonoBehaviour
{
    private TMP_Text tutorialText;
    private float delayTime = 0.05f;

    private bool texting = false;

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
                StartCoroutine(ClassText());
                break;
            case MapType.School:
                StartCoroutine(SchoolText());
                break;
            case MapType.Square:
                StartCoroutine(SquareText());
                break;
            case MapType.QuizSquare:
                StartCoroutine(QuizSquareText());
                break;

        }
    }
    IEnumerator ClassText()
    {
        StartCoroutine(TextPrint("���̸�Ʋ���� ������ �� ȯ����!\r\n���� �ο��!!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        StartCoroutine(TextPrint("�̰��� ������ �����̾�! \r\n�繰�Կ��� �ٸ� ����� �湮�ؼ� ���� ������ �� �� �־�!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        gameObject.SetActive(false);
    }
    IEnumerator SchoolText()
    {
        StartCoroutine(TextPrint("�̰��� �б���! ���� �б� ģ���鳢�� ���� �� �ִ� ��������! \r\n���� �б� ģ����� �̻� �б��� ������!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        gameObject.SetActive(false);
    }
    IEnumerator SquareText()
    {
        StartCoroutine(TextPrint("�̰��� ������ �����̾�!\r\n��� ģ������ ���� �� �ִ� ��������!\r\n�׸��� �پ��� ��Ÿ��� �����־�!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        gameObject.SetActive(false);
    }
    IEnumerator QuizSquareText()
    {
        StartCoroutine(TextPrint("�̰��� ���� �����̾�!\r\nOX��� ��级�� �� �� �־�!"));
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
        gameObject.SetActive(false);
    }

    // �ؽ�Ʈ �ϳ��� ����� �ϱ�
    IEnumerator TextPrint(string text)
    {
        texting = false;
        tutorialText.text = "";
        int count = 0;

        while (count != text.Length)
        {
            if (count < text.Length)
            {
                tutorialText.text += text[count].ToString();
                count++;
            }
            yield return new WaitForSeconds(delayTime);
        }

        if (count == text.Length)
        {
            texting = true;
        }
        yield return new WaitUntil(() => texting && (Input.touchCount == 1 || Input.GetMouseButtonDown(0)));
    }
}
