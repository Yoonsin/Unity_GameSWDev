using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static bool onTM = false;

    public GameObject textfield; //��ȭâ
    public GameObject TextData;
    public Text text; //��ȭ �ؽ�Ʈ


    public Image bgImg;
    public Image portraitImg;

    TextData textdata;
    string[] textSplit;
    int textCnt = 0; //0~15 : 1�ܰ�
    bool textFlag = true; //â Ȱ��ȭ ����
    bool textAniFlag = false; //�ؽ�Ʈ �ִ� Ȱ��ȭ ����
    string writerText;


    int questCnt = 0; //����Ʈ id
    int oldQuestCnt = 0; //����Ʈ 
    int id = 10; //ȣ���� id
    int length = 0; //ȣ���� ��� ����


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        onTM = false;
        textCnt = 14;
        textFlag = true;
        textAniFlag = false;
        textdata = TextData.GetComponent<TextData>();
        length = textdata.getLength(id);
        //Time.timeScale = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (onTM)
        {
            //��ȭâ Ȱ��ȭ & ��簡 ���� �ƴ϶��
            if (textFlag == true && textCnt < length)
            {
                //altŰ ������ �ؽ�Ʈ �ִϰ� �����ٸ� (�ؽ�Ʈ �ִϰ� ���� �߿� altŰ ������ ��ȭ ���ڿ��� ����)
                if (Input.GetKeyDown(KeyCode.LeftAlt) && textAniFlag == false)
                {
                    showText();
                }

            }
        }
        
    }

    public void showText()
    {
        textSplit = textdata.getText(id, textCnt).Split(':'); //�����͸� ���� ǥ��+����Ʈ ���̵�� �и�

        //Debug.Log(textSplit[1]);
        //Debug.Log(textSplit[1].Split('$')[1]);
        questCnt = int.Parse(textSplit[1].Split('$')[1]); // ����Ʈ ���̵� ����
        if (questCnt != oldQuestCnt) //���� �ٸ� ����Ʈ��
        {

            //��ȭâ �ߴ�
            textFlag = false;
            textfield.SetActive(textFlag);
            bgImg.color = new Color(0, 0, 0, 0);
            portraitImg.color = new Color(1, 1, 1, 0);
            oldQuestCnt = questCnt;
            Time.timeScale = 1;
        }
        else
        {
            oldQuestCnt = questCnt;

            if (questCnt == 0) //ó�� Ʃ�丮�� �� ���� ��� ������
                bgImg.color = new Color(0, 0, 0, 1);
            else
                bgImg.color = new Color(0, 0, 0, 0);
            
            portraitImg.sprite = textdata.getPortrait(id, int.Parse((textSplit[1].Split('$'))[0]));
            portraitImg.color = new Color(1, 1, 1, 1); //ǥ�� �̹��� ����
            StartCoroutine(NormalText(textSplit[0])); //�ؽ�Ʈ �ִ� ��� + ��� ����
            textfield.SetActive(textFlag); //�ؽ�Ʈ â Ȱ��ȭ
           //Debug.Log(textCnt);
            textCnt++; //���� ���� �ű�� 
        }
    }

    public bool getFlag()
    {
        return textFlag;
    }

    public void setFlag(bool flag)
    {
        textFlag = flag;
    }


    //�ѱ��ھ� �ԷµǴ� ������ �ִϸ��̼� �ڷ�ƾ
    IEnumerator NormalText(string narration)
    {
        textAniFlag = true; //�ؽ�Ʈ �ִ� ������..
        writerText = "";
        text.text = writerText;

        for (int i = 0; i < narration.Length; i++)
        {
            writerText += narration[i];
            text.text = writerText;
            if (writerText == narration) //������ ��µƴٸ�
            {
                textAniFlag = false; //�ؽ�Ʈ �ִ� ��
            }
            yield return null; //for�� ���� update���� ��ٸ���
        }
    }

}
