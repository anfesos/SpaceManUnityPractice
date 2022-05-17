using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;

    //Distancia para seguir el personaje, x por delante, 0 en Y, -10 en z
    public Vector3 offset = new Vector3(-5f,0.0f,-10f);

    //0.3f son segundos
    public float dampingTime = 0.3f; 

    //Velocidad de la camara
    public Vector3 velocity = Vector3.zero;

    //Awake recordar que es la que permite que se corra frame a frame algo
    void Awake()
    {
        //Esta linea sirve para tratar que el programa vaya a esta tasa de frame rate, sino se pone, iria a la velocidad que le permita la RAM
        Application.targetFrameRate = 60;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(true);
    }

    public void ResetCameraPosition()
    {
        MoveCamera(false);
    }

    //Sirve para hacer un movimiento suave
    void MoveCamera(bool smooth)
    {
        Vector3 destination = new Vector3(
                    target.position.x - offset.x,
                    offset.y,
                    offset.z
                );
        
        if(smooth){
            //SmoothDamp -> Da un efecto de cinematica para mover la camara, el metodo por referencia permite que se autocalcule la variable
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                destination,
                ref velocity,
                dampingTime
            );
        }else{
            this.transform.position = destination;
        }

        
    }
}
