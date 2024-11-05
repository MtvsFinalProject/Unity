using GH;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SW.PlaceManager;
namespace SW
{
    public class ClassManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadFurniture());
        }
        private IEnumerator LoadFurniture()
        {
            if (!PhotonNetwork.IsMasterClient) yield break; // ������ ����
            DataManager dataManager = DataManager.instance;
            while (dataManager.player == null)  // �÷��̾� ���� ���
            {
                yield return null;
            }
            SetTile setTile = dataManager.player.GetComponent<SetTile>();
            PlaceManager placeManager = GetInstance();
            // ��ġ �ε�
            placeManager.ReadPlace(DataManager.instance.mapId, (GetPlaceResInfo res) =>
            {
                foreach (PlaceInfo info in res.response)
                {
                    setTile.LoadData(new Vector3Int(info.x, info.y, 0), InventorySystem.GetInstance().items[info.objId].prefab, info.id);
                }
            });
        }
    }
    public class School
    {
        public int id;
        public string schoolName;
        public int backgroundColorId;
        public List<PlaceManager.ObjectInfo> furnitureList;
    }
}