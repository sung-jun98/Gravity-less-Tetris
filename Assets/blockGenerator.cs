using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockGenerator : MonoBehaviour
{
    public GameObject[] Tetris;

    float[] x_of_blocks = new float[] { 0.01f, 4.43f, 8.1f };
    public float y_of_blocks = -4.75f; //생성될 블록들의 x, y 좌표값

    public int remain_block_num = 3;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject block = Instantiate(block1);
        //block.transform.position = new Vector2(4, -4);
        Choose_blocks();
    }

    // Update is called once per frame
    void Update()  
    {
        if (remain_block_num == 0)
        {
            Choose_blocks(); //만약 남아있는 블록이 없다면 새로 세 개의 블록을 만든다.
        }
       
    }

    void Choose_blocks() //블록 프리펩 중 3개를 선택하여 생성한다.
    {
        for (int i = 0; i < 3; i++) 
        {
            GameObject block = Instantiate(Tetris[Random.Range(0, Tetris.Length)]);
            block.transform.position = new Vector2(x_of_blocks[i], y_of_blocks);
        }
        remain_block_num = 3; //카운트값 다시 3으로 초기화
    }
}
