using GH;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;
using static SW.PlaceManager;
namespace MJ
{
    public class MapContestFuritureLoader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadFurniture());
        }
        private IEnumerator LoadFurniture()
        {
            while (!PhotonNetwork.InRoom)  // ��������
            {
                yield return null;
            }
            if (!PhotonNetwork.IsMasterClient) yield break; // ������ ����
            DataManager dataManager = DataManager.instance;
            while (dataManager.player == null)  // �÷��̾� ���� ���
            {
                print(dataManager.player == null);
                yield return null;
            }
            InventorySystem inventorySystem = InventorySystem.GetInstance();

            SetTile setTile = dataManager.player.GetComponent<SetTile>();
            MapContestLoader mapContestLoader = MapContestLoader.GetInstance();
            // ��ġ �ε�

            //while (inventorySystem.items.Count == 0)  // ������
            //{
            //    yield return null;
            //}

            //foreach (ObjectContestInfo info in mapContestLoader.loadfurnitureList)
            //{
            //    if (inventorySystem.items[info.objId] != null)
            //        setTile.LoadData(new Vector3Int(info.x, info.y, 0), inventorySystem.items[info.objId].prefab, info.id);
            //}
        }
    }

}