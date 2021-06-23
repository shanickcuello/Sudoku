using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputField : MonoBehaviour
{
    public static UserInputField instance;

    NumberField lastField;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ActiveInputField(NumberField field)
    {
        gameObject.SetActive(true);
        lastField = field;
    }

    public void ClickedInput(int number)
    {
        lastField.ReceiveInput(number);
        gameObject.SetActive(false);
    }
}
