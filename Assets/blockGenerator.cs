using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockGenerator : MonoBehaviour
{
    public GameObject[] Tetris;

    float[] x_of_blocks = new float[] { 0.01f, 4.43f, 8.1f };
    public float y_of_blocks = -4.75f; //������ ��ϵ��� x, y ��ǥ��

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
            Choose_blocks(); //���� �����ִ� ����� ���ٸ� ���� �� ���� ����� �����.
        }
       
    }

    void Choose_blocks() //��� ������ �� 3���� �����Ͽ� �����Ѵ�.
    {
        for (int i = 0; i < 3; i++) 
        {
            GameObject block = Instantiate(Tetris[Random.Range(0, Tetris.Length)]);
            block.transform.position = new Vector2(x_of_blocks[i], y_of_blocks);
        }
        remain_block_num = 3; //ī��Ʈ�� �ٽ� 3���� �ʱ�ȭ
    }
}
