using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumInteractObj : Interact, IInteract
{
    public string interacterName = "��ȣ�ۿ����";
    public string GetInfo()
    {
        return interacterName;
    }


    public void Interact()
    {
        print(interacterName);
    }
    public void HighlightOff()
    {
        material.SetInt("_On", 0);
        print("����");
    }

    public void HighlightOn()
    {
        material.SetInt("_On", 1);
        print("����");
    }
}
