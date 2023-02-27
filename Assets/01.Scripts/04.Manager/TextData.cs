using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour
{

    //npc id // 대화:표정/퀘스트
    string[] textLog;
    Dictionary<int, Sprite> portraitDic;
    int textCnt = 0;

    public Sprite[] portraitArr;

    void Awake()
    {
        
        portraitDic = new Dictionary<int, Sprite>();
        generateData(); //대화창 데이터 생성


    }

    void generateData()
    {
        //text(대사 :표정 id $퀘스트#캐릭터 id)
        //0~10번까지 존재
        //사실 메인스토리만 있는 터라 굳이 textDic에는 id가 있을 필요가 없음. 추후 수정 예정 
        textLog = new string[] { "-2시간 전-:7$0#10", "메일함 열기(안 읽은 메일 1개):7$0#10","안 읽은 메일 (2):7$0#10", 
        "읽음) usaKaLobo - 이번 ‘만남’에 대한 논의 이미 읽었던 메일이다.:7$0#10", "(20xx.xx.xx 수신) 15번가 - ★ 15% 반짝 할인쿠폰 ★ :7$0#10",
        "광고메일이다. 읽을 필요는 없을 것 같다.:7$0#10", "(읽음) otsoBat ? 다음주 ‘행사’ 일정 발표 이미 읽었던 메일이다.:7$0#10",
        "(20xx.xx.xx 수신) 이리 ? 급한 일입니다. 바로 확인 요망:7$0#10","저도 방금 들은 소식이라 수칙 무시, 독단적으로 연락하는 점 먼저 죄송합니다.:7$0#10",
        "최근 서울 관광 산업 증진 목적으로 모든 관광지에 정차하는 새로운 열차가 개통되었다는 소식 들으셨을 겁니다.:7$0#10","이 열차에 폭탄이 설치될 겁니다. 익명의 테러리스트 집단이 열차 탈취 후 객차에 폭탄을 설치, 지나가는 관광지를 승객과 함께 날려버릴 생각인 것 같습니다.:7$0#10",
        "시기는.:7$0#10","오늘 아침입니다.:7$0#10","많은 민간인이 희생될 것 같아 급하게 전달합니다.:7$0#10","-친애하는 수호신에게-:7$0#10",
        "(…서둘러 출발해야 할 것 같다):1$0#10","여긴 많이 위험해.  뒤 칸으로 가서 어른들을 찾을 수 있겠니.:1$1#10","저…그..그게, 부모님이 저 앞에 있어요. 부모님과 만나려면 앞으로 가야 해요.:4$1#20",
            "그럼 내가 저 앞에서 부모님을 찾아 뒤로 가시라고 할게. 뒤 칸으로 갈 수 있지?:1$1#10","하, 하지만… 앞에 부모님이 있는데요..:3$1#20",
        "안돼, 너무 위험해:6$1#10","…(아이는 금방이라도 울 것 같은 표정을 지었다):4$1#10","그럼 꼭 내 뒤에 붙어있어. 절대 앞으로 먼저 나가지 말고.:3$1#10",
         "그래서, 부모님은 앞에 있는데 너는 왜 여기까지 온거니:8$2#10","…갑자기 엄청 큰 망치를 든 아저씨가 다가오길래… 의자 사이에 숨어있다가 소리가 나서 달려왔어요.:3$2#20",
            "아! 오셨구만!:0$3#30","늘 우리 팀 계획에 찬물을 끼얹는 놈이 누군가 했는데, 드디어 보는구만.:0$3#30","날 아나보지?:9$3#10","당연히!:0$3#30","모르지! 그렇게 목격자가 없는데 어떻게 뒤를 캐겠어?:1$3#30",
            "그래서 이번에 ‘깜짝’ 이벤트를 준비했지. 널 찾는 것 보다 네 정보책의 꼬리를 밟는게 더 쉬울 것 같더라고:0$3#30","그리고 꼬마야! 심부름 하느라 수고했다! 부모님을 생각하는 효심이 아주 갸륵하구나! 하하하!:0$3#30",
            "…저, 그:4$3#20","꼬마야, 절대 앞으로 먼저 나가지 말랬지? 이번에도 계속 뒤에 있어줘.:5$3#10","…네:1$3#20", "...:7$4#10","곧 종착역에 도착해. 내리면 아마 ‘정보책’이라던 사람이 네 부모님과 같이 있을거야.:3$4#10",
         "…원래대로면 목격자는 없어야겠지만:3$4#10","괜찮겠지?:4$4#10","…네:2$4#20","...그리고..고마워요.:1$4#20","...그래.고마워.:5$4#10",".:7$5#10"};


        
        //호랑이
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

        //꼬마
        portraitDic.Add(20 + 1, portraitArr[11]);
        portraitDic.Add(20 + 2, portraitArr[12]);
        portraitDic.Add(20 + 3, portraitArr[13]);
        portraitDic.Add(20 + 4, portraitArr[14]);

        //보스
        portraitDic.Add(30 + 0, portraitArr[15]);
        portraitDic.Add(30 + 1, portraitArr[16]);


    }

    public string getText(int textCnt)
    {
        return textLog[textCnt];
    }

    public Sprite getPortrait(int id, int portraitIdx)
    {
        if (portraitDic.ContainsKey(id))
        {
            return portraitDic[id + portraitIdx];
        }
        return null;

    }

    //설정된 대화창 길이 반환
    public int getLength()
    {
        return textLog.Length;
    }



}
