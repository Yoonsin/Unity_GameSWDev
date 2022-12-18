using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static bool onTM = false;

    public GameObject textfield; //대화창
    public GameObject TextData;
    public Text text; //대화 텍스트


    public Image bgImg;
    public Image portraitImg;

    TextData textdata;
    string[] textSplit;
    int textCnt = 0; //0~15 : 1단계
    bool textFlag = true; //창 활성화 여부
    bool textAniFlag = false; //텍스트 애니 활성화 여부
    string writerText;


    int questCnt = 0; //퀘스트 id
    int oldQuestCnt = 0; //퀘스트 
    int id = 10; //호랑이 id
    int length = 0; //호랑이 대사 길이


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
            //대화창 활성화 & 대사가 끝이 아니라면
            if (textFlag == true && textCnt < length)
            {
                //alt키 누르고 텍스트 애니가 끝났다면 (텍스트 애니가 진행 중에 alt키 누르면 대화 문자열이 꼬임)
                if (Input.GetKeyDown(KeyCode.LeftAlt) && textAniFlag == false)
                {
                    showText();
                }

            }
        }
        
    }

    public void showText()
    {
        textSplit = textdata.getText(id, textCnt).Split(':'); //데이터를 대사와 표정+퀘스트 아이디로 분리

        //Debug.Log(textSplit[1]);
        //Debug.Log(textSplit[1].Split('$')[1]);
        questCnt = int.Parse(textSplit[1].Split('$')[1]); // 퀘스트 아이디 추출
        if (questCnt != oldQuestCnt) //만약 다른 퀘스트면
        {

            //대화창 중단
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

            if (questCnt == 0) //처음 튜토리얼 할 때만 배경 검정색
                bgImg.color = new Color(0, 0, 0, 1);
            else
                bgImg.color = new Color(0, 0, 0, 0);
            
            portraitImg.sprite = textdata.getPortrait(id, int.Parse((textSplit[1].Split('$'))[0]));
            portraitImg.color = new Color(1, 1, 1, 1); //표정 이미지 갱신
            StartCoroutine(NormalText(textSplit[0])); //텍스트 애니 재생 + 대사 갱신
            textfield.SetActive(textFlag); //텍스트 창 활성화
           //Debug.Log(textCnt);
            textCnt++; //다음 대사로 옮기기 
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


    //한글자씩 입력되는 느낌의 애니메이션 코루틴
    IEnumerator NormalText(string narration)
    {
        textAniFlag = true; //텍스트 애니 진행중..
        writerText = "";
        text.text = writerText;

        for (int i = 0; i < narration.Length; i++)
        {
            writerText += narration[i];
            text.text = writerText;
            if (writerText == narration) //끝까지 출력됐다면
            {
                textAniFlag = false; //텍스트 애니 끝
            }
            yield return null; //for문 다음 update까지 기다리기
        }
    }

}
