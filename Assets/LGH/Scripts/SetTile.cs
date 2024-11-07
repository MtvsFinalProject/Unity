using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GH
{
    [System.Serializable]
    public class ObjectInfo
    {
        public GameObject obj; // ������Ʈ�� ������ ����
        public Vector3 position; // ��ġ�� ������ ����

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

        void Start()
        {
            setMode = false;
            tileLine.SetActive(false);
            SuchGrid();

        }

        void Update()
        {
            tileLine.SetActive(setMode);
            //���� ����

            if (DataManager.instance.setTileObj != null)
            {
                setGameObject = DataManager.instance.setTileObj;
            }
            if (setMode)
            {
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
    }

}