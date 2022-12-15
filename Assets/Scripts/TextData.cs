using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour
{

    //npc id // ��ȭ:ǥ��/����Ʈ
    Dictionary<int, string[]> textDic;
    Dictionary<int, Sprite> portraitDic;
    int textCnt = 0;

    public Sprite[] portraitArr;

    void Awake()
    {
        textDic = new Dictionary<int, string[]>();
        portraitDic = new Dictionary<int, Sprite>();
        generateData();


    }

    void generateData()
    {
        //dic(npc id, text(��� :ǥ�� id $����Ʈ))
        //0~10������ ����
        textDic.Add(10, new string[] { "-2�ð� ��-:7$0", "������ ����(�� ���� ���� 1��):7$0","�� ���� ���� (2):7$0", 
        "����) usaKaLobo - �̹� ���������� ���� ���� �̹� �о��� �����̴�.:7$0", "(20xx.xx.xx ����) 15���� - �� 15% ��¦ �������� �� :7$0",
        "��������̴�. ���� �ʿ�� ���� �� ����.:7$0", "(����) otsoBat ? ������ ����硯 ���� ��ǥ �̹� �о��� �����̴�.:7$0",
        "(20xx.xx.xx ����) �̸� ? ���� ���Դϴ�. �ٷ� Ȯ�� ���:7$0","���� ��� ���� �ҽ��̶� ��Ģ ����, ���������� �����ϴ� �� ���� �˼��մϴ�.:7$0",
        "�ֱ� ���� ���� ��� ���� �������� ��� �������� �����ϴ� ���ο� ������ ����Ǿ��ٴ� �ҽ� �������� �̴ϴ�.:7$0","�� ������ ��ź�� ��ġ�� �̴ϴ�. �͸��� �׷�����Ʈ ������ ���� Ż�� �� ������ ��ź�� ��ġ, �������� �������� �°��� �Բ� �������� ������ �� �����ϴ�.:7$0",
        "�ñ��.:7$0","���� ��ħ�Դϴ�.:7$0","���� �ΰ����� ����� �� ���� ���ϰ� �����մϴ�.:7$0","-ģ���ϴ� ��ȣ�ſ���-:7$0",
        "(�����ѷ� ����ؾ� �� �� ����):1$0","���� ���� ������.  �� ĭ���� ���� ����� ã�� �� �ְڴ�.:1$1","������..�װ�, �θ���� �� �տ� �־��. �θ�԰� �������� ������ ���� �ؿ�.:7$1",
            "�׷� ���� �� �տ��� �θ���� ã�� �ڷ� ���ö�� �Ұ�. �� ĭ���� �� �� ����?:1$1","��, �������� �տ� �θ���� �ִµ���..:7$1",
        "�ȵ�, �ʹ� ������:6$1","��(���̴� �ݹ��̶� �� �� ���� ǥ���� ������):7$1","�׷� �� �� �ڿ� �پ��־�. ���� ������ ���� ������ ����.:3$1","�׷���, �θ���� �տ� �ִµ� �ʴ� �� ������� �°Ŵ�:8$2","�����ڱ� ��û ū ��ġ�� �� �������� �ٰ����淡�� ���� ���̿� �����ִٰ� �Ҹ��� ���� �޷��Ծ��.:7$2",":7$2",":7$1",":7$1",":7$1",":7$1",":7$1",":7$1",":7$1"});
        
        
        portraitDic.Add(10 + 0, portraitArr[0]);
        portraitDic.Add(10 + 1, portraitArr[1]);
        portraitDic.Add(10 + 2, portraitArr[2]);
        portraitDic.Add(10 + 3, portraitArr[3]);
        portraitDic.Add(10 + 4, portraitArr[4]);
        portraitDic.Add(10 + 5, portraitArr[5]);
        portraitDic.Add(10 + 6, portraitArr[6]);
        portraitDic.Add(10 + 7, portraitArr[7]); //����
        portraitDic.Add(10 + 8, portraitArr[8]);
        portraitDic.Add(10 + 9, portraitArr[9]);
        portraitDic.Add(10 + 10, portraitArr[10]);
    }

    public string getText(int id, int textCnt)
    {
        return textDic[id][textCnt];
    }

    public Sprite getPortrait(int id, int portraitIdx)
    {
        if (portraitDic.ContainsKey(id))
        {
            return portraitDic[id + portraitIdx];
        }
        return null;

    }

    //�� npc���� ������ ��ȭâ ���� ��ȯ
    public int getLength(int id)
    {
        return textDic[id].Length;
    }



}
