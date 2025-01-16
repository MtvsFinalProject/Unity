using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HttpManager;
using UnityEngine.Networking;
namespace SW
{
    public class Report : MonoBehaviour
    {
        public static Report instance;
        private void Start()
        {
            instance = this;
        }
        public ReportPanel reportPanel;
        private ReportInfo reportInfo;
        [Serializable]
        private struct ReportInfo
        {
            public int reporterId;
            public int reportedUserId;
            public string contentType;
            public int contentId;
            public string reason;
            public ReportInfo(int reportedUserId, string contentType, int contentId)
            {
                reporterId = AuthManager.GetInstance().userAuthData.userInfo.id;
                this.reportedUserId = reportedUserId;
                this.contentType = contentType;
                this.contentId = contentId;
                reason = "";
            }
        }
        public enum ContentType
        {
            User, Chat, Board, Comment, Guestbook, Note, MapContest, Gallery
        }
        // ����O, ä��, �Խ���O, ���O, ����O, ����O, �����׽�ƮO, ��ī�̺�������O
        public void CreateReportInfo(string targetText, ContentType contentType, int reportedUserId = -1, int contentId = -1)
        {
            reportInfo = new ReportInfo(reportedUserId, contentType.ToString(), contentId);
            reportPanel.SetActivePanel(true);
            reportPanel.SetInfo(targetText);
        }
        public void ConfirmReport(string reason)
        {
            reportInfo.reason = reason;
            HttpInfo info = new HttpInfo();
            info.url = HttpManager.GetInstance().SERVER_ADRESS + "/report";
            info.body = JsonUtility.ToJson(reportInfo);
            info.contentType = "application/json";
            info.onComplete = (DownloadHandler downloadHandler) =>
            {
                ToastMessage.OnMessage("�Ű� �����Ǿ����ϴ�.");
                reportPanel.SetActivePanel(false);
            };
            StartCoroutine(HttpManager.GetInstance().Post(info));
        }
    }
}