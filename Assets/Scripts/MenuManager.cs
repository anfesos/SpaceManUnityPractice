using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;

    void Awake()
    {
        if(sharedInstance == null){
            sharedInstance = this;
        }
    }

    public void ShowMainMenu(){
        menuCanvas.enabled = true;
    }

    public void HideMainMenu(){
        menuCanvas.enabled = false;
    }


    public void ExitGame(){
        #if UNITY_EDITOR
            //Emula el stop del editor de Unity
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            //No es recomendable este forzado
            Application.Quit();
        #endif

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
