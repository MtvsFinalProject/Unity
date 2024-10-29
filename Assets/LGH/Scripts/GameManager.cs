using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using SW;

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

        public GameObject interacterPrefab;

        public TMP_Text currtRoomPlayerCnt;

        public List<GameObject> emojiList = new List<GameObject>();

        public GameObject interracBut;

        public bool interMode = false;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
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
            interracBut.SetActive(false);
            CoSpwamPlayer();
            // OnPhotonSerializeView ���� ������ ���� �� �� �����ϱ�(per seconds)
            PhotonNetwork.SerializationRate = 60;
            // ��κ� ������ ���� �� �� �����ϱ�
            PhotonNetwork.SendRate = 60;
        }
        void Update()
        {
            if(PhotonNetwork.CurrentRoom != null)
            currtRoomPlayerCnt.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            InteractionButton();
        }

        public void CoSpwamPlayer()
        {
            StartCoroutine(SpawnPlayer());

        }
        IEnumerator SpawnPlayer()
        {
            //�뿡 ������ �Ϸ�� ������ ��ٸ���.
            yield return new WaitUntil(() => { return PhotonNetwork.InRoom; });

            Vector2 randomPos = Random.insideUnitCircle * 2.0f;
            Vector3 initPosition = new Vector3(randomPos.x, randomPos.y, 0);

            GameObject p = PhotonNetwork.Instantiate("Player", initPosition, Quaternion.identity);
            Instantiate(interacterPrefab, p.transform);

            if (p.GetComponent<PhotonView>().IsMine)
            {
                DataManager.instance.player = p;
            }

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

        public void InteractionButton()
        {
            if(DataManager.instance.player != null)
            {

                interracBut.SetActive(interMode);
            }
        }

        public void ClickInterractionButton()
        {
            //��ư ������ �� ��ȣ�ۿ�
            DataManager.instance.player.GetComponentInChildren<PlayerInteracter>().InteractBut();
        }

        public void OnTile()
        {
            SetTile setTile = DataManager.instance.player.GetComponent<SetTile>();
            if (setTile.tileObjCheck)
            {
                setTile.DeleteTile();
            }
            else
            {
                setTile.OnTile();
            }
        }

       
    }
}