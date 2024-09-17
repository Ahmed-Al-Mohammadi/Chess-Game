using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman c;
        int i , j ;

        // Top Side
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for(int K = 0; K < 3; K++)
            {
                if (i >= 0 || i < 8)
                {
                    c = BoardManager.Instance.Chessmans[i, j];
                    if (c == null)
                    {
                        r[i ,j] = true;
                    }
                    else if(isWhite != c.isWhite)
                    {
                        r[i ,j] = true;
                    }
                }
                i ++;
            }
        }

        // Down Side
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int K = 0; K < 3; K++)
            {
                if (i >= 0 || i < 8)
                {
                    c = BoardManager.Instance.Chessmans[i, j];
                    if (c == null)
                    {
                        r[i, j] = true;
                    }
                    else if (isWhite != c.isWhite)
                    {
                        r[i, j] = true;
                    }
                }
                i++;
            }
        }

        // Midle Left 
        if (CurrentX != 0)
        {
            c = BoardManager.Instance.Chessmans[CurrentX-1 , CurrentY];
            if(c == null)
            {
                r[CurrentX - 1, CurrentY] = true;
            }
            else if (isWhite != c.isWhite)
            {
                r[CurrentX - 1, CurrentY] = true;
            }
        }

        // Midle Right 
        if (CurrentX != 7)
        {
            c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
            if (c == null)
            {
                r[CurrentX + 1, CurrentY] = true;
            }
            else if (isWhite != c.isWhite)
            {
                r[CurrentX + 1, CurrentY] = true;
            }
        }


        return r;
    }

    }
