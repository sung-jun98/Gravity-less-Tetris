using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; //배경이미지 투명도 바꿔주기 위해 사용

public class block_rule : MonoBehaviour//, IPointerDownHandler//, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private bool isDragging;
    private bool lockDown;
    public bool crushBlock; //게이지가 차면 버튼을 누를 수 있게 되고, 버튼을 누르면 블록이 부숴진다.
    
    private float elapsedTime = 0.0f; //롱클릭을 위한 변수
    public int gage_of_director = 0;
    public int score_of_director = 0; //현재 점수
    private float longClickTime = 2.0f;

    float startX;
    float startY; //블록이 처음 생성된 지점의 좌표값
    private static Transform[, ]grid = new Transform[9, 9];

    GameObject block_gener; //블록 제너레이터를 담을 게임 오브젝트
    GameObject gameDirector; //score 관련 UI
     
    public void CrushBlock()
    {
        crushBlock = true;
        Debug.Log(crushBlock);
    }

    public void OnMouseDown() //모바일에서는 지원이 안된다고 한다
    {  
        isDragging = true;
        Debug.Log("클릭됨");
    }
    

    public void OnMouseUp()
    {
        isDragging = false;
    
        if(ValidMove()) //스도쿠 범위 안
        {
            AddToGrid(); //grid에 추가한다.
            this.gameDirector.GetComponent<GameDirector>().elapsedTime = 0.0f;
         
            //Debug.Log("스도쿠 판 범위 안");
            float x = Mathf.RoundToInt(transform.position.x);
            float y = Mathf.RoundToInt(transform.position.y);
            
            transform.position = new Vector2(x, y);
            //Debug.Log(transform.position.x);
            //Debug.Log(transform.position.y);
            lockDown = true; //앞으로 못건드리게 잠근다.
            CheckLine(); //스도쿠 판을 한번 쭉 스캔한다.
            this.block_gener.GetComponent<blockGenerator>().remain_block_num -= 1;//선택할수 있는 블록 수를 1 뺀다.
            Debug.Log($"remain_block_num : {this.block_gener.GetComponent<blockGenerator>().remain_block_num}");
            //DebugGrid(); //차있는 블록 위치 출력

        }
        else//스도쿠판 밖일 시
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
        //Debug.Log("시작");
        startX = transform.position.x;
        startY = transform.position.y;
        lockDown = false;
        crushBlock = false;
        this.block_gener = GameObject.Find("Generator");
        this.gameDirector = GameObject.Find("GameDirector");

       
        gage_of_director = this.gameDirector.GetComponent<GameDirector>().gage;
        //score_of_director = this.gameDirector.GetComponent<GameDirector>().gage;

        for (int i = 0; i < 4; i++)//자식 오브젝트의 BoxCollider 비활성화
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

            if (mousePosition.sqrMagnitude < 1 & (longClickTime < elapsedTime) & (gage_of_director == 1)) //crushBlock이 true이고 롱클릭중일때
            //if (mousePosition.sqrMagnitude < 1 & (longClickTime < elapsedTime)) //나중에 지우고 바로 위 코드로 수정할것
            {
                Debug.Log("롱클릭중");
                //Debug.Log("crushBlock 터치");

                for (int i = 0; i< 4; i++)//자식 오브젝트의 BoxCollider 활성화
                {
                    transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
                transform.DetachChildren();
                crushBlock = false; //다시 원상태로
                this.gameDirector.GetComponent<GameDirector>().gage = 0;//게이지를 초기화한다. 
                
                this.block_gener.GetComponent<blockGenerator>().remain_block_num += 3; //계속 블럭이 생성되지 않게 조절
                Destroy(gameObject);//부모 클래스 삭제
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
            if (grid[roundX, roundY] != null) //만약 다른 블럭과 겹친다면
            {
                return false;
            }
                
        }
        return true;
    }

    void AddToGrid() //다른 블록과 겹치지 않도록
    {
        if(this.transform.childCount == 0) //게이지를 채운 후 부순 블록일 시
        {
            int roundX = Mathf.RoundToInt(this.transform.position.x);
            int roundY = Mathf.RoundToInt(this.transform.position.y);

            grid[roundX, roundY] = this.transform;
            //Debug.Log($"grid[{roundX} ,{roundY}]에 add완료\n");
        }
        foreach(Transform children in transform) //일반적인 경우일시
        {   
            
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundX, roundY] = children;
            //Debug.Log($"grid[{roundX} ,{roundY}]에 add완료\n");
        }
    }

    void CheckLine()
    {
        
        for (int i = 8; i>=0; i--)
        {
            //Debug.Log($"{i}번째줄 {HashLine(i)}");//
            if (HashLine_ver(i))
            {
                DeleteLine(i, 0);//code로 0을 넘기면 세로로 줄 사라진다.
            }
            if (HashLine_hor(i))
            {
                DeleteLine(i, 1); //code로 1을 넘기면 가로로 줄 사라진다.
            }
            
        }
        for (int j = 0; j < 3; j++) // 3 * 3 차있는지 확인
        {
            for (int k = 0; k < 3; k++)
            {
                if (HashLine_threeBythree(j * 3, k * 3)) //들어가는 값은 (0,0) (0,3) (0,6) (3,0) (3,3) (3,6) (6,0) (6,3) (6.6)
                    {
                        DeleteLine_threeBythree(j * 3, k * 3); //차있는 블록 삭제
                    }
            }
        }
        
    }
    bool HashLine_ver(int i) //(세로) 몇째줄에 블록이 얼마나 차있는지 계산한다.
    {
        //Debug.Log("HashLine 호출");

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

     bool HashLine_hor(int i) //(가로) 몇째줄에 블록이 얼마나 차있는지 계산한다.
    {
        //Debug.Log("HashLine 호출");

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
        // 3*3 칸이 꽉 차있는지 검사

        int set_row = (row / 3) * 3;    //  3x3의 행의 첫 위치
        int set_col = (col / 3) * 3;    //  3x3의 열의 첫 위치
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


    void DeleteLine(int i, int code) //입력받은 줄을 삭제하고, 그 자리를 null로 채운다.
    {
        //Debug.Log("Delete 호출");
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
        this.gameDirector.GetComponent<GameDirector>().score += 9; //score에 9점씩 더해준다.
        //this.gameDirector.GetComponent<GameDirector>().gage = 1; //gage를 변화해준다.
        //게이지 변화해주는 메서드 추가할것
        Calculate_gage();
    }
    void DeleteLine_threeBythree(int row, int col)
    {
        int set_row = (row / 3) * 3;    //  3x3의 행의 첫 위치
        int set_col = (col / 3) * 3;    //  3x3의 열의 첫 위치

        for (int i = set_row; i < set_row + 3; i++)
        {

            for (int j = set_col; j < set_col + 3; j++)
            {
                Debug.Log($"grid[{i}, {j}] 제거 시도중");
                Destroy(grid[i, j].gameObject);
                grid[i, j] = null;

            }
        }
        this.gameDirector.GetComponent<GameDirector>().score += 9; //score에 9점씩 더해준다.
        //this.gameDirector.GetComponent<GameDirector>().gage = 1; //gage를 변화해준다.
        Calculate_gage();
    }

    void DebugGrid() //gird에 얼마나 블록이 차있는지 확인하는 코드
    {
      

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] != null)
                {
                    Debug.Log($"grid[{i} ,{j}] 차있음");
                }
            }

        }
    }
    
    void Calculate_gage()
    {
        score_of_director = this.gameDirector.GetComponent<GameDirector>().score;
        if (score_of_director == 9) //줄 하나 없앴을 때
        {
            this.gameDirector.GetComponent<GameDirector>().gage = 1;
        }
        else if(score_of_director == 27) //줄 3개 없앴을 떄
        {
            this.gameDirector.GetComponent<GameDirector>().gage = 1;
        }
        else if (score_of_director == 900) //줄 10개 없앴을 떄
        {
            this.gameDirector.GetComponent<GameDirector>().gage = 1;
        }
    }
}
