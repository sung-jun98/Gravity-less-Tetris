using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI�� ����ϴµ� �ʿ��ϴ�.
using System;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    GameObject display_score;
    GameObject remain_time; //���� UI 
    GameObject blockRule; //��� �μ� ��
    GameObject goImage; //����̹���

    Color color; //����̹����� �����ϰ� �ٲ� ��

    public int score = 0; //�� ���� ����� �μ��������� ����
    public int gage = 0; //����� �μ� �������� ��Ÿ�� ����

    public float elapsedTime = 0.0f; //���������� ����� ��ġ�ѵ� ���ʳ� ���������� ��� ����
    public float game_over_time = 10.0f; //10�� �̻� ������ �� ���� ����� ���� �ʴ´ٸ� ���ӿ���

    // Start is called before the first frame update
    void Start()
    {
        this.display_score = GameObject.Find("Score");
        this.remain_time = GameObject.Find("RemainTime");
        this.blockRule = GameObject.Find("block_rule");

        goImage = GameObject.Find("backgrd_img"); //��� �̹��� �ٲٱ� //////
        color = goImage.GetComponent<SpriteRenderer>().color; //��� �̹����� �� ///////
    }

    // Update is called once per frame
    void Update()
    {
        this.display_score.GetComponent<Text>().text = "Score : " + score.ToString();
        this.remain_time.GetComponent<Text>().text = "�����ð� : " + (15.0f - Math.Truncate(elapsedTime)).ToString();
        //Math.Truncate(d)


        elapsedTime += Time.deltaTime;
        //Debug.Log(elapsedTime);
        if (elapsedTime > game_over_time)
        {
            //Debug.Log("���ӿ���");
        }
        //Debug.Log(this.blockRule.GetComponent<block_rule>().crushBlock);
        if (gage >= 1) //���� �������� 1�̻��̶�� �������� á���� �� �� �ֵ��� ������ �ٲپ��ش�.
        {
            color.a = 0.0f;
            goImage.GetComponent<SpriteRenderer>().color = color;
            Debug.Log("��� �̹��� �ٲٷ��� �õ���");
        }
        else //�� �̿ܿ��� ��� ������
        {
            color.a = 1.0f;
            goImage.GetComponent<SpriteRenderer>().color = color;
        }

        if (15.0f - Math.Truncate(elapsedTime) == 0)
        {
            SceneManager.LoadScene("SampleScene"); //�ð��� 0�� �Ǹ� ���ӿ���
        }
        //Debug.Log("score = " + score.ToString());

    }
}
