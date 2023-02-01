using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; //����̹��� ���� �ٲ��ֱ� ���� ���

public class block_rule : MonoBehaviour//, IPointerDownHandler//, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private bool isDragging;
    private bool lockDown;
    public bool crushBlock; //�������� ���� ��ư�� ���� �� �ְ� �ǰ�, ��ư�� ������ ����� �ν�����.
    
    private float elapsedTime = 0.0f; //��Ŭ���� ���� ����
    public int gage_of_director = 0;
    public int score_of_director = 0; //���� ����
    private float longClickTime = 2.0f;

    float startX;
    float startY; //����� ó�� ������ ������ ��ǥ��
    private static Transform[, ]grid = new Transform[9, 9];

    GameObject block_gener; //��� ���ʷ����͸� ���� ���� ������Ʈ
    GameObject gameDirector; //score ���� UI
     
    public void CrushBlock()
    {
        crushBlock = true;
        Debug.Log(crushBlock);
    }

    public void OnMouseDown() //����Ͽ����� ������ �ȵȴٰ� �Ѵ�
    {  
        isDragging = true;
        Debug.Log("Ŭ����");
    }
    

    public void OnMouseUp()
    {
        isDragging = false;
    
        if(ValidMove()) //������ ���� ��
        {
            AddToGrid(); //grid�� �߰��Ѵ�.
            this.gameDirector.GetComponent<GameDirector>().elapsedTime = 0.0f;
         
            //Debug.Log("������ �� ���� ��");
            float x = Mathf.RoundToInt(transform.position.x);
            float y = Mathf.RoundToInt(transform.position.y);
            
            transform.position = new Vector2(x, y);
            //Debug.Log(transform.position.x);
            //Debug.Log(transform.position.y);
            lockDown = true; //������ ���ǵ帮�� ��ٴ�.
            CheckLine(); //������ ���� �ѹ� �� ��ĵ�Ѵ�.
            this.block_gener.GetComponent<blockGenerator>().remain_block_num -= 1;//�����Ҽ� �ִ� ��� ���� 1 ����.
            Debug.Log($"remain_block_num : {this.block_gener.GetComponent<blockGenerator>().remain_block_num}");
            //DebugGrid(); //���ִ� ��� ��ġ ���

        }
        else//�������� ���� ��
        {
            Debug.Log(transform.position.x);
            Debug.Log(transform.position.y);
            Debug.Log('\n');

            transform.position = new Vector2(startX, startY);
            //transform.Translate()
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("����");
        startX = transform.position.x;
        startY = transform.position.y;
        lockDown = false;
        crushBlock = false;
        this.block_gener = GameObject.Find("Generator");
        this.gameDirector = GameObject.Find("GameDirector");

       
        gage_of_director = this.gameDirector.GetComponent<GameDirector>().gage;
        //score_of_director = this.gameDirector.GetComponent<GameDirector>().gage;

        for (int i = 0; i < 4; i++)//�ڽ� ������Ʈ�� BoxCollider ��Ȱ��ȭ
        {
            transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging & !lockDown)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
            elapsedTime += Time.deltaTime;
            //Debug.Log(mousePosition[0]);

            if (mousePosition.sqrMagnitude < 1 & (longClickTime < elapsedTime) & (gage_of_director == 1)) //crushBlock�� true�̰� ��Ŭ�����϶�
            //if (mousePosition.sqrMagnitude < 1 & (longClickTime < elapsedTime)) //���߿� ����� �ٷ� �� �ڵ�� �����Ұ�
            {
                Debug.Log("��Ŭ����");
                //Debug.Log("crushBlock ��ġ");

                for (int i = 0; i< 4; i++)//�ڽ� ������Ʈ�� BoxCollider Ȱ��ȭ
                {
                    transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
                transform.DetachChildren();
                crushBlock = false; //�ٽ� �����·�
                this.gameDirector.GetComponent<GameDirector>().gage = 0;//�������� �ʱ�ȭ�Ѵ�. 
                
                this.block_gener.GetComponent<blockGenerator>().remain_block_num += 3; //��� ���� �������� �ʰ� ����
                Destroy(gameObject);//�θ� Ŭ���� ����
            }
        }
        


    }
    /*
    bool ValidLocation(float x, float y)
    {
        if ((x >= 0.0 & x <= 8.0) & (y >= 0 & y <= 8.0))
        {
            return true;
        }
        else
        {
            return false;
        }

    }*/
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            if (roundX <0 || roundX > 9|| roundY < 0 || roundY > 9)
            {
                return false;

            }
            if (grid[roundX, roundY] != null) //���� �ٸ� ���� ��ģ�ٸ�
            {
                return false;
            }
                
        }
        return true;
    }

    void AddToGrid() //�ٸ� ��ϰ� ��ġ�� �ʵ���
    {
        if(this.transform.childCount == 0) //�������� ä�� �� �μ� ����� ��
        {
            int roundX = Mathf.RoundToInt(this.transform.position.x);
            int roundY = Mathf.RoundToInt(this.transform.position.y);

            grid[roundX, roundY] = this.transform;
            //Debug.Log($"grid[{roundX} ,{roundY}]�� add�Ϸ�\n");
        }
        foreach(Transform children in transform) //�Ϲ����� ����Ͻ�
        {   
            
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundX, roundY] = children;
            //Debug.Log($"grid[{roundX} ,{roundY}]�� add�Ϸ�\n");
        }
    }

    void CheckLine()
    {
        
        for (int i = 8; i>=0; i--)
        {
            //Debug.Log($"{i}��°�� {HashLine(i)}");//
            if (HashLine_ver(i))
            {
                DeleteLine(i, 0);//code�� 0�� �ѱ�� ���η� �� �������.
            }
            if (HashLine_hor(i))
            {
                DeleteLine(i, 1); //code�� 1�� �ѱ�� ���η� �� �������.
            }
            
        }
        for (int j = 0; j < 3; j++) // 3 * 3 ���ִ��� Ȯ��
        {
            for (int k = 0; k < 3; k++)
            {
                if (HashLine_threeBythree(j * 3, k * 3)) //���� ���� (0,0) (0,3) (0,6) (3,0) (3,3) (3,6) (6,0) (6,3) (6.6)
                    {
                        DeleteLine_threeBythree(j * 3, k * 3); //���ִ� ��� ����
                    }
            }
        }
        
    }
    bool HashLine_ver(int i) //(����) ��°�ٿ� ����� �󸶳� ���ִ��� ����Ѵ�.
    {
        //Debug.Log("HashLine ȣ��");

        for (int j = 0; j < 9; j++)
        {
            if (grid[i, j] == null)
            {                 
                //Debug.Log($"{i} ,{j}");
                //Debug.Log($"{i} ,{j}");
                return false;
            }
            
        }
        return true;
    }

     bool HashLine_hor(int i) //(����) ��°�ٿ� ����� �󸶳� ���ִ��� ����Ѵ�.
    {
        //Debug.Log("HashLine ȣ��");

        for (int j = 0; j < 9; j++)
        {
            if (grid[j, i] == null)
            {                 
                return false;
            }
            
        }
   
        return true;
    }
    bool HashLine_threeBythree(int row, int col)
    {
        // 3*3 ĭ�� �� ���ִ��� �˻�

        int set_row = (row / 3) * 3;    //  3x3�� ���� ù ��ġ
        int set_col = (col / 3) * 3;    //  3x3�� ���� ù ��ġ
        int num_of_notNull = 0;

        for (int i = set_row; i < set_row + 3; i++)
        {

            for (int j = set_col; j < set_col + 3; j++)
            {
                if (grid[i, j] != null)
                {
                    num_of_notNull += 1;
                }

            }
        }
        if (num_of_notNull == 9)
        {
            return true;
            
        }
        else
        {
            return false;
        }
    }


    void DeleteLine(int i, int code) //�Է¹��� ���� �����ϰ�, �� �ڸ��� null�� ä���.
    {
        //Debug.Log("Delete ȣ��");
        if (code == 0)
        {
            for (int k = 0; k < 9; k++)
            {
                Destroy(grid[i, k].gameObject);
                grid[i, k] = null;
            }
        }
        if (code == 1)
        {
            for (int k = 0; k < 9; k++)
            {
                Destroy(grid[k, i].gameObject);
                grid[k, i] = null;
            }
        }
        this.gameDirector.GetComponent<GameDirector>().score += 9; //score�� 9���� �����ش�.
        //this.gameDirector.GetComponent<GameDirector>().gage = 1; //gage�� ��ȭ���ش�.
        //������ ��ȭ���ִ� �޼��� �߰��Ұ�
        Calculate_gage();
    }
    void DeleteLine_threeBythree(int row, int col)
    {
        int set_row = (row / 3) * 3;    //  3x3�� ���� ù ��ġ
        int set_col = (col / 3) * 3;    //  3x3�� ���� ù ��ġ

        for (int i = set_row; i < set_row + 3; i++)
        {

            for (int j = set_col; j < set_col + 3; j++)
            {
                Debug.Log($"grid[{i}, {j}] ���� �õ���");
                Destroy(grid[i, j].gameObject);
                grid[i, j] = null;

            }
        }
        this.gameDirector.GetComponent<GameDirector>().score += 9; //score�� 9���� �����ش�.
        //this.gameDirector.GetComponent<GameDirector>().gage = 1; //gage�� ��ȭ���ش�.
        Calculate_gage();
    }

    void DebugGrid() //gird�� �󸶳� ����� ���ִ��� Ȯ���ϴ� �ڵ�
    {
      

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] != null)
                {
                    Debug.Log($"grid[{i} ,{j}] ������");
                }
            }

        }
    }
    
    void Calculate_gage()
    {
        score_of_director = this.gameDirector.GetComponent<GameDirector>().score;
        if (score_of_director == 9) //�� �ϳ� ������ ��
        {
            this.gameDirector.GetComponent<GameDirector>().gage = 1;
        }
        else if(score_of_director == 27) //�� 3�� ������ ��
        {
            this.gameDirector.GetComponent<GameDirector>().gage = 1;
        }
        else if (score_of_director == 900) //�� 10�� ������ ��
        {
            this.gameDirector.GetComponent<GameDirector>().gage = 1;
        }
    }
}
