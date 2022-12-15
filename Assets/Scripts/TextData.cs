using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour
{

    //npc id // 대화:표정/퀘스트
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
        //dic(npc id, text(대사 :표정 id $퀘스트))
        //0~10번까지 존재
        textDic.Add(10, new string[] { "-2시간 전-:7$0", "메일함 열기(안 읽은 메일 1개):7$0","안 읽은 메일 (2):7$0", 
        "읽음) usaKaLobo - 이번 ‘만남’에 대한 논의 이미 읽었던 메일이다.:7$0", "(20xx.xx.xx 수신) 15번가 - ★ 15% 반짝 할인쿠폰 ★ :7$0",
        "광고메일이다. 읽을 필요는 없을 것 같다.:7$0", "(읽음) otsoBat ? 다음주 ‘행사’ 일정 발표 이미 읽었던 메일이다.:7$0",
        "(20xx.xx.xx 수신) 이리 ? 급한 일입니다. 바로 확인 요망:7$0","저도 방금 들은 소식이라 수칙 무시, 독단적으로 연락하는 점 먼저 죄송합니다.:7$0",
        "최근 서울 관광 산업 증진 목적으로 모든 관광지에 정차하는 새로운 열차가 개통되었다는 소식 들으셨을 겁니다.:7$0","이 열차에 폭탄이 설치될 겁니다. 익명의 테러리스트 집단이 열차 탈취 후 객차에 폭탄을 설치, 지나가는 관광지를 승객과 함께 날려버릴 생각인 것 같습니다.:7$0",
        "시기는.:7$0","오늘 아침입니다.:7$0","많은 민간인이 희생될 것 같아 급하게 전달합니다.:7$0","-친애하는 수호신에게-:7$0",
        "(…서둘러 출발해야 할 것 같다):1$0","여긴 많이 위험해.  뒤 칸으로 가서 어른들을 찾을 수 있겠니.:1$1","저…그..그게, 부모님이 저 앞에 있어요. 부모님과 만나려면 앞으로 가야 해요.:7$1",
            "그럼 내가 저 앞에서 부모님을 찾아 뒤로 가시라고 할게. 뒤 칸으로 갈 수 있지?:1$1","하, 하지만… 앞에 부모님이 있는데요..:7$1",
        "안돼, 너무 위험해:6$1","…(아이는 금방이라도 울 것 같은 표정을 지었다):7$1","그럼 꼭 내 뒤에 붙어있어. 절대 앞으로 먼저 나가지 말고.:3$1","그래서, 부모님은 앞에 있는데 너는 왜 여기까지 온거니:8$2","…갑자기 엄청 큰 망치를 든 아저씨가 다가오길래… 의자 사이에 숨어있다가 소리가 나서 달려왔어요.:7$2",":7$2",":7$1",":7$1",":7$1",":7$1",":7$1",":7$1",":7$1"});
        
        
        portraitDic.Add(10 + 0, portraitArr[0]);
        portraitDic.Add(10 + 1, portraitArr[1]);
        portraitDic.Add(10 + 2, portraitArr[2]);
        portraitDic.Add(10 + 3, portraitArr[3]);
        portraitDic.Add(10 + 4, portraitArr[4]);
        portraitDic.Add(10 + 5, portraitArr[5]);
        portraitDic.Add(10 + 6, portraitArr[6]);
        portraitDic.Add(10 + 7, portraitArr[7]); //공백
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

    //각 npc마다 설정된 대화창 길이 반환
    public int getLength(int id)
    {
        return textDic[id].Length;
    }



}
