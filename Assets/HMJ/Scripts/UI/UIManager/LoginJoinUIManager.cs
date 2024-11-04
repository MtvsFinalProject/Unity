using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{
    public class UIManager : MonoBehaviour
    {
        public enum Loginstep
        {
            START,
            SERVICE,
            LOGIN,
            NAME,
            EMAIL,
            PASSWORD,
            INTEREST,
            PERSONAL
        }

        public Loginstep currentLoginstep;

        #region Button
        [Header("���� ��ư ����Ʈ")]
        public List<Button> nextButtons;

        [Header("���� ��ư ����Ʈ")]
        public List<Button> backButtons;

        [Header("4. �ߺ� ��ư")]
        public Button checkIDButton;


        [Header("4. ���� Ȯ�� ��ư")]
        public Button checkNumButton;

        #endregion

        #region Panel
        [Header("�α��� �г� ����Ʈ")]
        public List<GameObject> logins;

        #endregion

        [Header("�ε��� �����̴�")]
        private Slider indexSlider;

        [Header("4. �ߺ� ���̵� �ؽ�Ʈ")]
        public TMP_Text checkIDText;

        [Header("4. �ߺ� Ȯ�� �ؽ�Ʈ")]
        public TMP_Text checkNumText;

        [Header("5. ��й�ȣ ��ǲ�ʵ�")]
        public TMP_InputField pWInputField;

        [Header("5. ��й�ȣ Ȯ�� ��ǲ�ʵ�")]
        public TMP_InputField pWCheckInputField;

        [Header("6. ���ɻ� ����Ʈ")]
        public List<string> interests;


        private void Start()
        {
            indexSlider = GameObject.Find("Index_Slider").GetComponent<Slider>();

            // �ʱ� �г� ��Ƽ�� ����
            for (int i = 0; i < logins.Count; i++)
            {
                logins[i].SetActive(false);
            }
            logins[0].SetActive(true);

            // �������� �Ѿ�� ��ư�� �Ҵ�
            for (int i = 0; i < nextButtons.Count; i++)
            {
                nextButtons[i].onClick.AddListener(NextStep);
            }

            //  �ڷΰ��� ��ư�� �Ҵ�
            for (int i = 0; i < backButtons.Count; i++)
            {
                backButtons[i].onClick.AddListener(BackStep);
            }

            //4 ������
            checkNumButton.onClick.AddListener(CheckNum);
            checkNumText.gameObject.SetActive(false);
            checkIDButton.onClick.AddListener(CheckID);
            checkIDText.gameObject.SetActive(false);

        }

        private void Update()
        {
            Slider();
            PWCheck();
        }

        public void NextStep()
        {
            for (int i = 0; i < logins.Count; i++)
            {

                logins[i].SetActive(false);

            }
            logins[(int)currentLoginstep + 1].SetActive(true);
            currentLoginstep++;
        }
        public void BackStep()
        {
            for (int i = 0; i < logins.Count; i++)
            {

                logins[i].SetActive(false);

            }
            logins[(int)currentLoginstep - 1].SetActive(true);
            currentLoginstep--;
        }

        private void Slider()
        {
            int indexConvert = (int)currentLoginstep - 2;
            float sliderValue = indexConvert * 0.2f;
            indexSlider.value = Mathf.Lerp(indexSlider.value, sliderValue, 0.05f);
        }

        public void CheckNum()
        {
            //���***** ������ȣ Ȯ��
            checkNumText.gameObject.SetActive(true);
        }
        public void CheckID()
        {
            //���***** ���̵� �ߺ� Ȯ��
            checkIDText.gameObject.SetActive(true);
        }

        public void PWCheck()
        {
            if (pWInputField.text == pWCheckInputField.text)
            {
                nextButtons[5].GetComponent<Image>().color = new Color32(242, 136, 75, 255);
                nextButtons[5].interactable = true;
            }
            else
            {
                nextButtons[5].GetComponent<Image>().color = new Color32(242, 242, 242, 255);
                nextButtons[5].interactable = false;
            }
        }



    }
}