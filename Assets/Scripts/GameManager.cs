using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] Board board;

    //Called from Btn
    public void SetDificulty(int dificultIndex)
    {
        board.Difficulty = (Dificulties)dificultIndex;
        board.StartGame();
        menuPanel.SetActive(false);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
