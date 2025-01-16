using System.Collections;
using System.Collections.Generic;
using GH;
using MJ;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class RoomDecoratorToggle : MonoBehaviour
{
    public enum DecorationPermission
    {
        Allowed,   // �ٹ̱� ���
        NotAllowed // �ٹ̱� �����
    }

    public DecorationPermission currentPermission = DecorationPermission.NotAllowed;



    public void ToggleDecorationPermission()
    {
        currentPermission = currentPermission == DecorationPermission.Allowed
            ? DecorationPermission.NotAllowed
            : DecorationPermission.Allowed;

        Debug.Log($"���� �ٹ̱� ����: {currentPermission}");
        OnPermissionChanged(currentPermission);
    }

    // ��� ���� ����
    [PunRPC]
    public void EnableDecoration()
    {
        Debug.Log("�ٹ̱Ⱑ ���Ǿ����ϴ�.");
        SceneUIManager.GetInstance().OnInventoryUI();
    }

    // ����� ���� ����
    [PunRPC]
    public void DisableDecoration()
    {
        Debug.Log("�ٹ̱Ⱑ �����Ǿ����ϴ�.");
        SceneUIManager.GetInstance().OffInventoryUI();
    }

    public void RPC_SettingDecoration(bool bToggle)
    {
        PhotonView pv = GetComponent<PhotonView>();
        if (null == pv)
            return;

        if (bToggle)
            pv.RPC("EnableDecoration", RpcTarget.OthersBuffered);
        else
            pv.RPC("DisableDecoration", RpcTarget.OthersBuffered);
    }

    // ���� ���� �� ȣ��
    public void OnPermissionChanged(DecorationPermission newPermission)
    {
        RPC_SettingDecoration(newPermission == DecorationPermission.Allowed);
    }

}
