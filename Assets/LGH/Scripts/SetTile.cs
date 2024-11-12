using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.InputManagerEntry;

namespace GH
{
    [System.Serializable]
    public class ObjectInfo
    {
        public GameObject obj; // ������Ʈ�� ������ ����
        public Vector3 position; // ��ġ�� ������ ����
        public int id; // DB id

        // ������
        public ObjectInfo(GameObject obj, Vector3 position)
        {
            this.obj = obj;
            this.position = position;
        }
    }

    public class SetTile : MonoBehaviour
    {
        public Tilemap tilemap;
        public Transform playerFrontTileTransform;
        private Vector3Int tilePosition;
        private Grid grid;
        public GameObject setGameObject;
        public TileBase emptyTilebase;
        public GameObject tileLine;

        public bool setMode;

        public List<ObjectInfo> objectList = new List<ObjectInfo>();

        public bool tileObjCheck;
        public int setObjectId;

        public PlayerMove playerMove;


        void Start()
        {
            setMode = false;
            tileLine.SetActive(false);
            SuchGrid();
            playerMove =GetComponent<PlayerMove>();
        }

        void Update()
        {
            tileLine.SetActive(setMode);
            //������ �ڵ� ��Ÿ�� ����
            if (DataManager.instance.setTileObj != null)
            {
                setGameObject = DataManager.instance.setTileObj;
            }
            setObjectId = DataManager.instance.setTileObjId;


            if (setMode)
            {

                if (playerMove.stingDir == -transform.up)
                {
                    playerFrontTileTransform.localPosition = new Vector3(0, -1, 0);
                }
                else if (playerMove.stingDir == transform.up)
                {
                    playerFrontTileTransform.localPosition = new Vector3(0, 2, 0);

                }
                else if (playerMove.stingDir == transform.right)
                {
                    playerFrontTileTransform.localPosition = new Vector3(1, 1, 0);

                }
                else if (playerMove.stingDir == -transform.right)
                {

                    playerFrontTileTransform.localPosition = new Vector3(-1, 1, 0);

                }

                tilePosition = grid.WorldToCell(playerFrontTileTransform.position);
                tileLine.transform.position = tilePosition;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    OnTile();
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    DeleteTile();
                }
                tileObjCheck = tilemap.HasTile(tilePosition);


            }
        }

        public void OnTile()
        {
            if (!tilemap.HasTile(tilePosition) && InventorySystem.GetInstance().CheckItem())
            {
                tilemap.SetTile(tilePosition, emptyTilebase);
                GameObject setObject = Instantiate(setGameObject, tilemap.transform);
                setObject.transform.position = tilePosition;
                AddObject(setObject);

                InventorySystem.GetInstance().UseItem();
                print(tilemap.HasTile(tilePosition));
                // ���
                ObjectInfo obj = objectList.Last();
                PlaceManager.ObjectInfo objectInfo = new PlaceManager.ObjectInfo();
                objectInfo.objId = setObjectId;
                objectInfo.x = tilePosition.x;
                objectInfo.y = tilePosition.y;
                //objectInfo.rot =
                objectInfo.mapId = DataManager.instance.mapId;
                objectInfo.mapType = DataManager.instance.mapType;
                PlaceManager.GetInstance().CreatePlace(objectInfo, (PlaceManager.PlaceInfo callBack) =>
                {
                    obj.id = callBack.id;
                });
            }
        }
        public void DeleteTile()
        {
            if (tilemap.HasTile(tilePosition))
            {
                tilemap.SetTile(tilePosition, null);
                foreach (ObjectInfo obj in objectList)
                {
                    if (obj.position == tilePosition)
                    {
                        Destroy(obj.obj.gameObject);
                        PlaceManager.GetInstance().DeletePlace(obj.id); // ���
                    }
                }
            }
        }
        public void SuchGrid()
        {
            grid = GameObject.Find("Grid").GetComponent<Grid>();
            tilemap = grid.transform.GetChild(0).GetComponent<Tilemap>();
        }

        public void AddObject(GameObject obj)
        {
            Vector3 position = obj.transform.position; // ������Ʈ�� ���� ��ġ�� ������
            ObjectInfo newObjectInfo = new ObjectInfo(obj, position); // �� ObjectInfo ��ü ����
            objectList.Add(newObjectInfo); // ����Ʈ�� �߰�
        }
        public void LoadData(Vector3Int pos, GameObject obj, int id)
        {
            tilePosition = pos;
            setGameObject = obj;
            if (!tilemap.HasTile(tilePosition))
            {
                tilemap.SetTile(tilePosition, emptyTilebase);
                GameObject setObject = Instantiate(setGameObject, tilemap.transform);
                setObject.transform.position = tilePosition;
                AddObject(setObject);
                objectList.Last().id = id;
                print(tilemap.HasTile(tilePosition));
            }
        }
    }

}