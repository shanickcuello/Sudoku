using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberField : MonoBehaviour
{
    Board board;
    int x1, y1;
    int value;

    string identifier;

    public TextMeshProUGUI number;

    public void SetValue(int x1, int y1, int value, string identifier, Board board)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.value = value;
        this.identifier = identifier;
        this.board = board;

        number.text = (value != 0) ? value.ToString() : "";

        if(value != 0)
            GetComponentInParent<Button>().interactable = false;
        else
        {
            number.color = Color.white;
        }
    }

    public void ButtonClick() => UserInputField.instance.ActiveInputField(this);

    public void ReceiveInput(int newValue)
    {
        value = newValue;
        number.text = (value != 0) ? value.ToString() : "";
        number.color = Color.black; //Este es un naranjita que me gusto jeje. #F06A00
        board.SetInputInRiddle(x1, y1, value);
    }

    public int GetX() => x1;

    public int GetY() => y1;

    public void SetHint(int value)
    {
        this.value = value;
        number.text = value.ToString();
        number.color = Color.green;
        GetComponentInParent<Button>().interactable = false;
    }
}
