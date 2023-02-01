using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI를 사용하는데 필요하다.
using System;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    GameObject display_score;
    GameObject remain_time; //게임 UI 
    GameObject blockRule; //블록 부술 때
    GameObject goImage; //배경이미지

    Color color; //배경이미지를 투명하게 바꿀 색

    public int score = 0; //몇 개의 블록을 부수었는지의 정보
    public int gage = 0; //블록을 부순 게이지를 나타낼 정보

    public float elapsedTime = 0.0f; //마지막으로 블록을 배치한뒤 몇초나 지났는지를 담는 변수
    public float game_over_time = 10.0f; //10초 이상 스도쿠 판 위에 블록을 놓지 않는다면 게임오버

    // Start is called before the first frame update
    void Start()
    {
        this.display_score = GameObject.Find("Score");
        this.remain_time = GameObject.Find("RemainTime");
        this.blockRule = GameObject.Find("block_rule");

        goImage = GameObject.Find("backgrd_img"); //배경 이미지 바꾸기 //////
        color = goImage.GetComponent<SpriteRenderer>().color; //배경 이미지의 색 ///////
    }

    // Update is called once per frame
    void Update()
    {
        this.display_score.GetComponent<Text>().text = "Score : " + score.ToString();
        this.remain_time.GetComponent<Text>().text = "남은시간 : " + (15.0f - Math.Truncate(elapsedTime)).ToString();
        //Math.Truncate(d)


        elapsedTime += Time.deltaTime;
        //Debug.Log(elapsedTime);
        if (elapsedTime > game_over_time)
        {
            //Debug.Log("게임오버");
        }
        //Debug.Log(this.blockRule.GetComponent<block_rule>().crushBlock);
        if (gage >= 1) //만약 게이지가 1이상이라면 게이지가 찼음을 알 수 있도록 배경색을 바꾸어준다.
        {
            color.a = 0.0f;
            goImage.GetComponent<SpriteRenderer>().color = color;
            Debug.Log("배경 이미지 바꾸려고 시도함");
        }
        else //그 이외에는 모두 검정색
        {
            color.a = 1.0f;
            goImage.GetComponent<SpriteRenderer>().color = color;
        }

        if (15.0f - Math.Truncate(elapsedTime) == 0)
        {
            SceneManager.LoadScene("SampleScene"); //시간이 0이 되면 게임오버
        }
        //Debug.Log("score = " + score.ToString());

    }
}
