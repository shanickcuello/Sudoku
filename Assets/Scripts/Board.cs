using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //[SerializeField] int sizeArray;

    [SerializeField] GameObject winPanel, keepTrying;

    List<NumberField> fieldList = new List<NumberField>();

    [SerializeField] int shuffleAmount;

    int[,] solvedGrid = new int[9, 9];
    int[,] riddleGrid = new int[9, 9];
    [SerializeField] int piecesToErase; // cuantos mas borrres mas dificil

    int maxHint;
    string debugText;

    [SerializeField] Transform A1, A2, A3, B1, B2, B3, C1, C2, C3;
    [SerializeField] GameObject buttonPrefab;

    [SerializeField] Dificulties difficulty;

    public Dificulties Difficulty { get => difficulty; set => difficulty = value; }

    void Start()
    {
        //VIEJOS METODOS
        //InitGrid(ref solvedGrid);
        ////DebugGrid(ref solvedGrid);
        //ShuffleGrid(ref solvedGrid, shuffleAmount);
        //CreateRiddleGrid();
    }

    public void StartGame()
    {
        FillGridBase(ref solvedGrid);
        SolveGrid(ref solvedGrid);
        CreateRiddleGrid(ref solvedGrid, ref riddleGrid);
        CreateButtons();
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

    void CreateRiddleGrid()
    {
        //Copiar la anterior solvegrid
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                riddleGrid[i, j] = solvedGrid[i, j];
            }
        }

        //Setear la dificulatad
        SetDifficulty();
        //borrar los numeros que no queremos

        for (int i = 0; i < piecesToErase; i++)
        {
            int x1 = Random.Range(0, 9);
            int y1 = Random.Range(0, 9);
            //Si hay un cero lo tiro de nuevo hasta encontrar uno sin 0
            while (riddleGrid[x1, y1] == 0)
            {
                x1 = Random.Range(0, 9);
                y1 = Random.Range(0, 9);
            }
            //una vez que lo encontamos sin 0 lo sesteamos a 0
            riddleGrid[x1, y1] = 0;
        }
        DebugGrid(ref riddleGrid);
    }

    void CreateButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject newBtn = Instantiate(buttonPrefab);
                NumberField numField = newBtn.GetComponent<NumberField>();

                //Setear los valores. 
                numField.SetValue(i, j, riddleGrid[i, j], i + "," + j, this);
                newBtn.name = i + "," + j;

                if (riddleGrid[i, j] == 0)
                    fieldList.Add(numField);

                //Parentearlos
                //A1 
                if (i < 3 && j < 3)
                    newBtn.transform.SetParent(A1, false);
                //A2
                if (i < 3 && j > 2 && j < 6)
                    newBtn.transform.SetParent(A2, false);
                //A3
                if (i < 3 && j > 5)
                    newBtn.transform.SetParent(A3, false);
                //B1
                if (i > 2 && i < 6 && j < 3)
                    newBtn.transform.SetParent(B1, false);
                //B2
                if (i > 2 && i < 6 && j > 2 && j < 6)
                    newBtn.transform.SetParent(B2, false);
                //B3
                if (i > 2 && i < 6 && j > 5)
                    newBtn.transform.SetParent(B3, false);
                //C1
                if (i > 5 && j < 3)
                    newBtn.transform.SetParent(C1, false);
                //C2
                if (i > 5 && j > 2 && j < 6)
                    newBtn.transform.SetParent(C2, false);
                //C3
                if (i > 5 && j > 5)
                    newBtn.transform.SetParent(C3, false);
            }
        }
    }

    public void SetInputInRiddle(int x, int y, int value) => riddleGrid[x, y] = value;

    public void SetDifficulty()
    {
        switch (Difficulty)
        {
            case Dificulties.DEBUG:
                piecesToErase = 3;
                maxHint = 3;
                break;
            case Dificulties.EASY:
                piecesToErase = 25;
                maxHint = 5;
                break;
            case Dificulties.MEDIUM:
                maxHint = 7;
                piecesToErase = 35;
                break;
            case Dificulties.HARD:
                piecesToErase = 45;
                maxHint = 10;
                break;
            case Dificulties.INSANE:
                piecesToErase = 65;
                maxHint = 1;
                break;
        }
    }

    public void CheckComplete()
    {
        if (CheckIfWon())
            winPanel.SetActive(true);
        else
            keepTrying.SetActive(true);
    }
    bool CheckIfWon()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (riddleGrid[i, j] != solvedGrid[i, j])
                    return false;
            }
        }
        return true;
    }

    //Called from btn 
    public void ShowHint()
    {
        if (fieldList.Count > 0 && maxHint > 0)
        {
            int randomIndex = Random.Range(0, fieldList.Count);
            maxHint--;

            riddleGrid[fieldList[randomIndex].GetX(), fieldList[randomIndex].GetY()] =
                solvedGrid[fieldList[randomIndex].GetX(), fieldList[randomIndex].GetY()];

            fieldList[randomIndex].SetHint(riddleGrid[fieldList[randomIndex].GetX(), fieldList[randomIndex].GetY()]);
            fieldList.RemoveAt(randomIndex);
        }
        else
        {
            Debug.Log("Out of hints");
        }
    }

    #region BACKTRACKING

    bool ColumnContainsNumber(int y, int value, ref int[,] grid)
    {
        for (int x = 0; x < 9; x++)
        {
            if (grid[x, y] == value)
                return true;
        }

        return false;
    }

    bool RowContainsNumber(int x, int value, ref int[,] grid)
    {
        for (int y = 0; y < 9; y++)
        {
            if (grid[x, y] == value)
                return true;
        }

        return false;
    }
    bool BlockContainsNumber(int x, int y, int value, ref int[,] grid)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[x - (x % 3) + i, y - (y % 3) + j] == value)
                    return true;
            }
        }
        return false;
    }

    bool CheckAll(int x, int y, int value, ref int[,] grid)
    {
        if (ColumnContainsNumber(y, value, ref grid)) return false;
        if (RowContainsNumber(x, value, ref grid)) return false;
        if (BlockContainsNumber(x, y, value, ref grid)) return false;

        return true;
    }

    bool IsValidGrid(ref int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void FillGridBase(ref int[,] grid)
    {
        List<int> rowValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> columnValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        int value = rowValues[Random.Range(0, rowValues.Count)];
        grid[0, 0] = value;
        rowValues.Remove(value);
        columnValues.Remove(value);

        //Rows
        for (int i = 1; i < 9; i++)
        {
            value = rowValues[Random.Range(0, rowValues.Count)];
            grid[i, 0] = value;
            rowValues.Remove(value);
        }

        //Columns
        for (int i = 1; i < 9; i++)
        {
            value = columnValues[Random.Range(0, columnValues.Count)];
            if (i < 3)
            {
                while (BlockContainsNumber(0, 0, value, ref grid))
                {
                    value = columnValues[Random.Range(0, columnValues.Count)];
                }
            }
            grid[0, i] = value;
            columnValues.Remove(value);
        }
    }

    bool SolveGrid(ref int[,] grid)
    {
        DebugGrid(ref grid);

        if (IsValidGrid(ref grid))
            return true;

        int x = 0;
        int y = 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid[i, j] == 0)
                {
                    x = i;
                    y = j;
                    break;
                }
            }
        }
        List<int> possibilities = new List<int>();
        possibilities = GetAllPossibilities(x, y, ref grid);

        for (int p = 0; p < possibilities.Count; p++)
        {
            grid[x, y] = possibilities[p]; //aca sucede la magic
            if (SolveGrid(ref grid))
            {
                return true;
            }
            grid[x, y] = 0;
        }

        return false;
    }

    List<int> GetAllPossibilities(int x, int y, ref int[,] grid)
    {
        List<int> possibilities = new List<int>();
        for (int val = 1; val <= 9; val++)
        {
            if (CheckAll(x, y, val, ref grid))
            {
                possibilities.Add(val);
            }
        }
        return possibilities;
    }
    #endregion

    void CreateRiddleGrid(ref int[,] sGrid, ref int[,] rGrid)
    {
        //Copiar la anterior solvegrid
        //for (int i = 0; i < 9; i++)
        //{
        //    for (int j = 0; j < 9; j++)
        //    {
        //        riddleGrid[i, j] = solvedGrid[i, j];
        //    }
        //}
        System.Array.Copy(sGrid, rGrid, sGrid.Length);

        //Setear la dificulatad
        SetDifficulty();

        //borrar los numeros que no queremos
        for (int i = 0; i < piecesToErase; i++)
        {
            int x1 = Random.Range(0, 9);
            int y1 = Random.Range(0, 9);
            //Si hay un cero lo tiro de nuevo hasta encontrar uno sin 0
            while (rGrid[x1, y1] == 0)
            {
                x1 = Random.Range(0, 9);
                y1 = Random.Range(0, 9);
            }
            //una vez que lo encontamos sin 0 lo sesteamos a 0
            rGrid[x1, y1] = 0;
        }
        DebugGrid(ref riddleGrid);
    }
}

public enum Dificulties
{
    DEBUG,
    EASY,
    MEDIUM,
    HARD,
    INSANE
}
