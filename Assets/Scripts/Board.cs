using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //[SerializeField] int sizeArray;

    [SerializeField] int shuffleAmount;

    int[,] solvedGrid = new int[9, 9];
    string debugText;


    void Start()
    {
        InitGrid(ref solvedGrid);
        DebugGrid(ref solvedGrid);
        ShuffleGrid(ref solvedGrid, shuffleAmount);
    }

    void InitGrid(ref int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                // n1 = 8 * 3 = 24
                // n2 = 8 / 3 = 2
                // result = (n1 + n2 + j ) % 9 + 1

                grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1; // formula matrices
            }
        }
    }

    void DebugGrid(ref int[,] grid)
    {
        debugText = "";
        int separator = 0;
        for (int i = 0; i < 9; i++)
        {
            debugText += "|";
            for (int j = 0; j < 9; j++)
            {
                debugText += grid[i, j].ToString();
                separator = j % 3;
                if (separator == 2)
                    debugText += "|";
            }
            debugText += "\n";
        }
        Debug.Log(debugText);
    }

    void ShuffleGrid(ref int[,] grid, int shuffleAmount)
    {
        for (int i = 0; i < shuffleAmount; i++)
        {
            int value1 = Random.Range(1, 10);
            int value2 = Random.Range(1, 10);
            //Aca tenfgo que mixear las seldas
            MixTwoGridCells(ref grid, value1, value2);
        }
        DebugGrid(ref grid);
    }

    void MixTwoGridCells(ref int[,] grid, int value1, int value2)
    {
        int x1 = 0;
        int x2 = 0;

        int y1 = 0;
        int y2 = 0;

        for (int i = 0; i < 9; i += 3)
        {
            for (int k = 0; k < 9; k += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        if (grid[i + j, k + l] == value1)
                        {
                            x1 = i + j;
                            y1 = k + l;
                        }

                        if (grid[i + j, k + l] == value2)
                        {
                            x2 = i + j;
                            y2 = k + l;
                        }
                    }
                }
                grid[x1, y1] = value2;
                grid[x2, y2] = value1;
            }
        }
    }

}
