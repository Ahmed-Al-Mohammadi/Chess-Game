using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }
    public Chessman[,] Chessmans { set; get; }
    private Chessman selectedChessman;
    private const float TILE_SIZE = 1.0f ;      // هنا انا حددت حجم البلاط
    private const float TILE_OFFSET = 0.5f ;

    private int selectionX = -1;                // عشان نعرف المربع الي بيتم فية تحديده في البورد
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;    // دي ليست بكل قطع الشطرنج
    private List<GameObject> activeChessman = new List<GameObject>();       //  ليست فيها الاكتف و الحركة بتاعت كل قطع

    private Material previousMat;
    public Material selectedMat;



    private Quaternion orientation = Quaternion.Euler(0, 180, 0); // عشان اعدل عل اتجاه الحصان
    //  لما اعدل ع الاسود بيعكس الاسود !!؟


    public bool isWhiteTurn = true ;
    private void Start()        // عشان نشغل كل قطع الشطارنج 
    {
        Instance = this ;
        SpawnAllChessmans();
        
    }

    private void Update()        // عشان نحدث الاطارات و البربعات بتاعتنا
    {
        UpdateSelection();
        DrawChessboard();
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    // select the chess man 
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // move the chess man
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectChessman(int x , int y)  // بتحدد مكان القطع 
    {
        if (Chessmans[x,y] == null)
        {
            return;
        }
        if (Chessmans[x, y].isWhite != isWhiteTurn)
        {
            return;
        }

        bool hasAtleastOneMove = false;
        allowedMoves = Chessmans[x,y].PossibleMove();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtleastOneMove = true;
                }
            }
        }
        if (!hasAtleastOneMove)
        {
            return ;
        }

        selectedChessman = Chessmans[x,y];
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveChessman(int x, int y)     // بتحددد حركة القطع 
    {
        
        if (allowedMoves[x,y])
        {
            Chessman c = Chessmans[x, y];
            if (c != null && c.isWhite != isWhiteTurn)  // بيحذف القطعة الي بتتاكل
            {
                // captuer a piece

                // if it is the king
                if (c.GetType() == typeof(King))
                {
                    // end game
                    EndGame();
                    return;
                }
                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }


            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTitleCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x,y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }
        BoardHighlights.Instance.Hidehighlights();
        
        selectedChessman = null;
    }


    private void UpdateSelection()      //بيحدث المربع المتحدد باستخدام الماوس  
    {
        if (!Camera.main )
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 25.0f, LayerMask.GetMask("ChessPlane"))) // زاوية الكاميرا
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = - 1;
            selectionY = -1;
        }
    }

    private void SpawnChessman(int index , int x , int y ) //بيشغل قطعة الشطرنج 
    {
        GameObject go = Instantiate(chessmanPrefabs[index],GetTitleCenter(x,y),  Quaternion.identity) as GameObject ;
        go.transform.SetParent(transform) ;
        Chessmans[x,y] = go.GetComponent <Chessman>();
        Chessmans[x,y].SetPosition(x,y);
        activeChessman.Add(go) ;    

    }

    private void SpawnAllChessmans() //بيشغل و بيجدد مكان كل قطع الشطارنج علي البورد
    {
        activeChessman = new List<GameObject> ();       // SPAWN THE WHITE TEAM
        Chessmans = new Chessman[8, 8];
        // THE KING
        SpawnChessman(0, 3, 0);     // بنحدد مكان الملك الابيض
        // THE QUEEN
        SpawnChessman(1, 4, 0);     // بنحدد مكان الملكة البيضاء
        // ROOKS
        SpawnChessman(2, 0, 0);     // بنحدد مكان ال الطابية البيضاء
        SpawnChessman(2, 7, 0);     //  بنحدد مكان الطابية البيضاء
        // BISHOPS
        SpawnChessman(3, 2, 0);     // بنحدد مكان الفيل الاببيض 
        SpawnChessman(3, 5, 0);     // بنحدد مكان الفيل الابيض
        // KNIGHTS
        SpawnChessman(4, 1, 0);     // بنحدد مكان ال الحصان الابيض
        SpawnChessman(4, 6, 0);     // بنحدد مكان الحصان الابيض
        // PAWNS
        for (int i = 0; i < 8; i++)    // دي لوب بنحط بيها كل الجنود البيضاء
        {
            SpawnChessman(5, i, 1);
        }

        // SPAWN THE BLACK  TEAM
        // THE KING
        SpawnChessman(6, 4, 7);     // بنحدد مكان الملك الاسود
        // THE QUEEN
        SpawnChessman(7, 3, 7);     // بنحدد مكان الملكة السوداء 
        // ROOKS
        SpawnChessman(8, 0, 7);     // بنحدد مكان ال الطابية السوداء
        SpawnChessman(8, 7, 7);     //  بنحدد مكان الطابية السوداء
        // BISHOPS
        SpawnChessman(9, 2, 7);     // بنحدد مكان الفيل الاسود 
        SpawnChessman(9, 5, 7);     // بنحدد مكان الفيل الاسود
        // KNIGHTS
        SpawnChessman(10, 1, 7);     // بنحدد مكان ال الحصان الاسود
        SpawnChessman(10, 6, 7);     // بنحدد مكان الحصان الاسود

        // PAWNS
        for (int i = 0; i < 8; i++)    // دي لوب بنحط بيها كل الجنود السوداء
        {
            SpawnChessman(11, i, 6);
        }

    }

    private Vector3 GetTitleCenter(int x , int y)       // بنحدد الابعاد في المنتصف
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i <= 8; i++)            // عشان نقسم البورد ل row and col
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for(int j = 0; j <= 8; j++)
            {
                 start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
            
        }

        // draw the selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(
               Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
               Vector3.forward * selectionY  + Vector3.right * (selectionX + 1));
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn)
        {
            Debug.Log("White Team Wins");
        }
        else
        {
            Debug.Log("Black Team Wins");
        }

        foreach (GameObject go in activeChessman)
        {
            Destroy(go);
        }

        isWhiteTurn = true;
        BoardHighlights.Instance.Hidehighlights();
        SpawnAllChessmans();


    }
}
