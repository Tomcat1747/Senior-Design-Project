using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void onCreditsEnded(){
        SceneManager.LoadScene("0_Main Menu");
    }
}
