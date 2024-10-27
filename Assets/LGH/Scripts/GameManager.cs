using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace GH
{
    public class GameManager : MonoBehaviourPun
    {
        public static GameManager instance;

        public GameObject activateButtonPannel;
        public GameObject emojiButtonPannel;
        public Button stingButton;
        // �г� �� ����
        bool onActivate = true;

        public VariableJoystick Joystick;
        public Transform emojiTransform;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            activateButtonPannel.SetActive(false);
            emojiButtonPannel.SetActive(true);

            StartCoroutine(SpawnPlayer());

            // OnPhotonSerializeView ���� ������ ���� �� �� �����ϱ�(per seconds)
            PhotonNetwork.SerializationRate = 30;
            // ��κ� ������ ���� �� �� �����ϱ�
            PhotonNetwork.SendRate = 30;
        }
        void Update()
        {
                
            
        }
        IEnumerator SpawnPlayer()
        {
            //�뿡 ������ �Ϸ�� ������ ��ٸ���.
            yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });

            Vector2 randomPos = Random.insideUnitCircle * 5.0f;
            Vector3 initPosition = new Vector3(randomPos.x, randomPos.y, 0);

            PhotonNetwork.Instantiate("Player", initPosition, Quaternion.identity);
        }
        public void ConversionPanel()
        {
            if (onActivate)
            {
                activateButtonPannel.SetActive(true);
                emojiButtonPannel.gameObject.SetActive(false);
                onActivate = false;
            }
            else
            {
                activateButtonPannel.SetActive(false);
                emojiButtonPannel.gameObject.SetActive(true);
                onActivate = true;

            }
        }
    }
}