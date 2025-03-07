using Photon.Chat;
using SW;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GH
{

    public class ChatItem : MonoBehaviour
    {
        TMP_Text chatText;
        private void Awake()
        {
            chatText = GetComponent<TMP_Text>();

        }
        void Start()
        {

        }

        void Update()
        {

        }
        public void SetText(string msg, Color chatColor)
        {
            //컬러 안나오는거 해결하기
            string[] text = msg.Split(" ", 2);
            if (text[0] == DataManager.instance.playerName)
            {
                print("색상변경11");
                text[0] = "<color=#" + ColorUtility.ToHtmlStringRGB(chatColor) + ">" + text[0] + "</color>";
            }

            if (msg[0] == '@')
            {
                print("색상변경22");
                print("text : "+text[0]);

                text[0] = "<color=#" + ColorUtility.ToHtmlStringRGB(chatColor) + ">" + text[0] + "</color>";
                text[1] = "<color=#F26A1B>" + text[1] + "</color>";
            }

            text[0] = "<b>" + text[0] + "</b>";

            chatText.text = text[0] + "  " + text[1];
        }
    }
}