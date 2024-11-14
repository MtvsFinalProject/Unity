using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace MJ
{
    public class FileManager : MonoBehaviour
    {
        private static FileManager instance;
        // Start is called before the first frame update

        private void Awake()
        {
            if (null == instance)
            {
                instance = this;

                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public static FileManager Instance
        {
            get
            {
                if (null == instance)
                    return null;
                return instance;
            }
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // Ư�� ���, ������ ���� �̸��� ���ϴ� �Լ�
        //public FileInfo[] GetFileInfo(string filePath, string form = "")
        //{
        //    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);

        //    return directoryInfo.GetFiles("*." + form);
        //}

        // Resources ���� ���� Ư�� ��ο��� ��� ��������Ʈ ������ �ҷ����� �Լ�
        public Sprite[] LoadSpritesFromResources(string folderPath)
        {
            // Resources ���� ������ �ش� ����� ��� ��������Ʈ �ε�
            return Resources.LoadAll<Sprite>(folderPath);
        }
    }
}
    