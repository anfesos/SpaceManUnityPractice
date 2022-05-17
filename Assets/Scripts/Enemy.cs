using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 1.5f;
    public int enemyDamage = 10;

    Rigidbody2D RigidbodyEnemy;
    public bool facingRight = false;
    private Vector3 startPosition;

    //Sólo ocurre una vez cuando se despiertan todos los objetos de la escena
    private void Awake()
    {
        RigidbodyEnemy = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = startPosition;
    }

    // Sirve para cuando se hacen cambio a fisicas (Fuerzas , aceleraciones, velocidades), para evitar cambios en tasas de frame
    void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;

        if(facingRight){
            //mirando a la derecha
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0,180,0);
        }else{
            //mirando a la izquierda
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;
        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame){

            RigidbodyEnemy.velocity = new Vector2(currentRunningSpeed,RigidbodyEnemy.velocity.y);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Coin"){
            return;
        }
        if (collision.tag == "Player"){
            collision.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
            return;
        }

        //Si llegamos aquí, no hemos chocado ni con monedas, ni con player, por tanto se rota
        facingRight = !facingRight;
    }
}
