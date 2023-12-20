using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void LoadScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
