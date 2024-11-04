using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GH;
using Photon.Pun;


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
        QuizLastState, // ���� ��
        QuizOrderStat_End
    }
    public TMP_Text text;

    int idx = 0;
    int quizN = 5;

    float quizTime = 5.0f;
    bool quizClear = false;

    int minimumPlayer = 2;

    public List<QuizData> quizList;


    QuizState m_eNextQuizOrder = QuizState.QuizNoneState;
    QuizState m_eCurQuizOrder = QuizState.QuizNoneState;

    GameObject player;

    List<int> correctAnswers;

    private void Start()
    {
    }
    private void Update()
    {
        StartQuiz();
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

    public void StartQuiz()
    {
        if(m_eNextQuizOrder == QuizState.QuizNoneState)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount >= minimumPlayer)
                m_eNextQuizOrder = QuizState.QuizReadyState;
            else
            {
                text.text = minimumPlayer.ToString() + "�� �̻� �𿩾� ��� ���۵˴ϴ�.";
            }
        }
    }

    /// <summary>
    /// ���� ���� ��ȯ
    /// </summary>
    public void NextQuizState()
    {
        if (m_eCurQuizOrder == m_eNextQuizOrder)
            return;
        Debug.Log("���� ��ȯ: " + m_eCurQuizOrder.ToString());
        switch (m_eNextQuizOrder)
        {
            case QuizState.QuizReadyState:
                QuizReady();
                break;
            case QuizState.QuizRunState:
                QuizTimeLimit();
                break;
            case QuizState.QuizOverState:
                QuizOverLimit();
                break;
            case QuizState.QuizLastState:
                QuizEnd();
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
        text.text = quizList[idx].quizText;

        StartCoroutine(ProceedQuiz(quizTime));
    }

    public void QuizReady()
    {
        text.text = "��� �� ��� ���۵˴ϴ�.\n";
        StartCoroutine(ReadyStart(3.0f));
    }

    public void QuizOverLimit()
    {
        if (quizClear)
            text.text = "�����Դϴ�.";
        else
            text.text = "������ �ƴմϴ�.";
        StartCoroutine(QuizOver(3.0f));
    }

    public void QuizEnd()
    {
        text.text = "��� ����Ǿ����ϴ�.";
        StartCoroutine(QuizLast(3.0f));
    }

    public void QuizClearCheck()
    {
        GameObject player = DataManager.instance.player;
        Debug.Log("�÷��̾� x:" + player.transform.position.x);
        if (((player.transform.position.x > 0) && !quizList[idx].quizBoolean) || ((player.transform.position.x < 0) && quizList[idx].quizBoolean))// x
            quizClear = true;
        else
            quizClear = false;
    }
    public IEnumerator ProceedQuiz(float _quizTime)
    {
        yield return new WaitForSeconds(_quizTime);

        QuizClearCheck();
        idx++;
        idx %= quizN;

        m_eNextQuizOrder = QuizState.QuizOverState;
    }

    public IEnumerator ReadyStart(float _readyTime)
    {
        yield return new WaitForSeconds(_readyTime);
        m_eNextQuizOrder = QuizState.QuizRunState;
    }

    public IEnumerator QuizOver(float _overTime)
    {
        yield return new WaitForSeconds(_overTime);
        if(idx + 1 >= quizN)
            m_eNextQuizOrder = QuizState.QuizLastState;
        else
            m_eNextQuizOrder = QuizState.QuizRunState;

    }

    public IEnumerator QuizLast(float _lastTime)
    {
        yield return new WaitForSeconds(_lastTime);
        text.text = "";
    }
    /// <summary>
    /// �������� ����
    /// </summary>
    public void shuffleQuizList()
    {
        quizList.OrderBy(a => Guid.NewGuid()).ToList();
    }
}
