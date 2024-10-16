using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GH
{
    public class InventoryUI : MonoBehaviour
    {
        private GraphicRaycaster inventory_GraphicRay;
        private PointerEventData inventory_PointerEventData;
        private List<RaycastResult> inventory_RayResultList;

        // ���� �巡�׸� ������ ����
        private ItemSlotUI beginDragSlot;
        // �ش� ������ ������ Ʈ������
        private Transform beginDragIcon_Transform;

        //�巡�� ���� �� ������ ��ġ
        private Vector3 beginDragIconPoint;
        //�巡�� ���� �� Ŀ���� ��ġ
        private Vector3 beginDragCursorPoint;
        private int beginDragSlotSiblingIndex;

        
        private void Update()
        {
            //��ǻ�� ����
            inventory_PointerEventData.position = Input.mousePosition;

            OnPointDown();
            OnPointDrage();
            OnPointUp();
        }

        private T RayCastGetFirstComponent<T>() where T : Component
        {
            inventory_RayResultList.Clear();

            inventory_GraphicRay.Raycast(inventory_PointerEventData, inventory_RayResultList);

            if(inventory_RayResultList.Count == 0)
            {
                return null;
            }
            return inventory_RayResultList[0].gameObject.GetComponent<T>();
       
        }

        private void OnPointDown()
        {

        }
        
        private void OnPointDrage()
        {

        }
         
        private void OnPointUp()
        {

        }
    }

}