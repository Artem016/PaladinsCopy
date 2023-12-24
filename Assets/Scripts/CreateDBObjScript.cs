using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class CreateDBObjScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI inputLoginUp, inputPasswordUp, inputPasswordRipeatUp, InputLoginIn, InputPasswordIn;
    [SerializeField] GameObject MainPage, RegistrationPage;

    public void SignUp()
    {
        if(inputPasswordUp.text == inputPasswordRipeatUp.text) 
        {
            var ds = new DataService("tempObjDatabase.db");

            ds.CreateDBObj(inputLoginUp.text, inputPasswordUp.text);

            var obj = ds.GetObj();
            ToConsole(obj);
            MainPage.SetActive(true);
            RegistrationPage.SetActive(false);
        }
        else
        {
            Debug.Log("Пароли не совпадают");
        }
    }

    public void SignIn()
    {
        var ds = new DataService("tempObjDatabase.db");
        IEnumerable<Obj> objects = ds.GetObj();

        // Значение, с которым вы хотите сравнить объекты

        // Цикл для сравнения каждого объекта с заданным значением
        foreach (var obj in objects)
        {
            if (obj.Login == InputLoginIn.text && obj.Password == InputPasswordIn.text)
            {
                // Выполнение действий в случае соответствия условию
                // Например, вывод объекта или выполнение другой логики
                Debug.Log("Found a matching object:");
                Debug.Log(obj.ToString());
                SceneManager.LoadScene("Menu");
            }
            else
            {
                Debug.Log("Не прокнуло");
            }
        }
    }

    private void ToConsole(IEnumerable<Obj> obj)
    {
        foreach (var o in obj)
        {
            Debug.Log(o.ToString());
        }
    }
}