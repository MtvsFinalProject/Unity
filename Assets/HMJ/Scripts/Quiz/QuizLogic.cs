using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuizLogic : MonoBehaviour
{

    [Serializable]
    public struct QuizData
    {
        public string quizText;
        public bool quizBoolean;
    }

    public enum QuizState
    {
        QuizNoneState, // 
        QuizReadyState, // ���� �غ���
        QuizRunState, // ���� ������
        QuizOverState, // ���� ��
        QuizOrderStat_End
    }
    public TMP_Text text;

    int idx = 0;
    int quizN = 5;

    float quizTime = 10.0f;
    bool quizClear = false;

    public List<QuizData> quizList;


    QuizState m_eNextQuizOrder = QuizState.QuizNoneState;
    QuizState m_eCurQuizOrder = QuizState.QuizReadyState;


    private void Update()
    {
        NextQuizState();
    }
    /// <summary>
    /// ���� �߰��ϱ� - �ڵ�� �߰��� ���
    /// </summary>
    /// <param name="quizData"></param>
    public void AddQuizData(QuizData quizData)
    {
        quizList.Add(quizData);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void RandomQuiz()
    {
        shuffleQuizList();
    }

    /// <summary>
    /// ���� ���� ��ȯ
    /// </summary>
    public void NextQuizState()
    {
        if (m_eCurQuizOrder == m_eNextQuizOrder)
            return;
        Debug.Log("���� ��ȯ: " + m_eCurQuizOrder.ToString());
        switch (m_eCurQuizOrder)
        {
            case QuizState.QuizReadyState:
                QuizReady();
                break;
            case QuizState.QuizRunState:
                QuizTimeLimit();
                break;
            case QuizState.QuizOverState:
                break;
            case QuizState.QuizOrderStat_End:
                break;
        }
        m_eCurQuizOrder = m_eNextQuizOrder;
    }

    /// <summary>
    /// ���� ���� �ð����� �ڷ�ƾ �� ���� ������Ʈ�� �Ѿ��
    /// </summary>
    public void QuizTimeLimit()
    {
        idx %= quizN;
        text.text = quizList[idx].quizText;
        StartCoroutine(ProceedQuiz(quizTime));
        quizClear = true;
    }

    public void QuizReady()
    {
        text.text = "��� �� ��� ���۵˴ϴ�.\n";
        StartCoroutine(ReadyStart(3.0f));
        m_eNextQuizOrder = QuizState.QuizRunState;
    }

    public IEnumerator ProceedQuiz(float _quizTime)
    {
        yield return new WaitForSeconds(_quizTime);
        m_eNextQuizOrder = QuizState.QuizOverState;
    }

    public IEnumerator ReadyStart(float _readyTime)
    {
        yield return new WaitForSeconds(_readyTime);
    }

    /// <summary>
    /// �������� ����
    /// </summary>
    public void shuffleQuizList()
    {
        quizList.OrderBy(a => Guid.NewGuid()).ToList();
    }
}
