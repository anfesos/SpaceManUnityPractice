using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables del ovimiento del personaje
    public float jumpForce = 6f;
    public float runningSpeed = 2f;

    public LayerMask groundMask;
    Rigidbody2D playerRigibody;
    Animator animator;
    Vector3 startPosition;

    //Tamaño del rayo desde el centro del personaje hacia abajo para detectar que esta en el suelo
    float rayTracing = 1.5f;

    //SCREAMING SNAKE CASE -> Cuando la variable esta en mayúscula y separada con _
    const string STATE_ALIVE = "isALive";
    const string STATE_ON_THE_GROUND = "isOnTheGround";

    [SerializeField]
    private int healthPoints, manaPoints;

    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15,
                    MAX_HEALTH = 200, MAX_MANA = 30,
                    MIN_HEALTH = 10, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;

    void Awake()
    {
        playerRigibody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

        startPosition = this.transform.position;
    }

    public void StartGame()
    {
        //Forma de inicializar los valores por defecto de las variables
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, false);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;


        Invoke("RestartPosition", 0.2f);
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.playerRigibody.velocity = Vector2.zero;


        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {

        /*Forma menos practica de realizar el manejo del contro

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            Jump();
        }
        */

        //Forma más avanzada de manejar el control
        if (Input.GetButtonDown("Jump"))
        {
            Jump(false);
        }

        if(Input.GetButtonDown("SuperJump")){
            Jump(true);
        }

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());


        //Línea que sirve para hacerle seguimiento al RayCast
        Debug.DrawRay(this.transform.position, Vector2.down * rayTracing , Color.green);
        //Debug.Log(playerRigibody.velocity);
    }

    void FixedUpdate()
    {

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (playerRigibody.velocity.x < runningSpeed)
            {
                playerRigibody.velocity = new Vector2(Input.GetAxis("Horizontal") * runningSpeed, playerRigibody.velocity.y);
            }
        }
        else
        {
            //Sino estamos dentro de la partida
            playerRigibody.velocity = new Vector2(0, playerRigibody.velocity.y);
        }


        if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    //Realiza la acción de saltar
    void Jump(bool isSuperJump)
    {
        float jumpForceFactor = jumpForce;

        if(isSuperJump && manaPoints>=SUPERJUMP_COST){
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            //El método play reproduce solo una vez
            GetComponent<AudioSource>().Play();
            if(IsTouchingTheGround()){
                playerRigibody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            }
        }

    }

    //Sirve para saber si un personaje esta o no en el suelo
    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(this.transform.position,
                            Vector2.down,
                            rayTracing,
                            groundMask))
        {

            //TODO: programar lógica de contacto con el suelo
            //animator.enabled = true;
            //GameManager.sharedInstance.currentGameState = GameState.inGame;
            return true;
        }
        else
        {
            //TODO: programar lógica de no contacto
            //animator.enabled = false;
            return false;
        }
    }

    public void Die()
    {
        float travelledDistance = GetTravelledDistance();
        float previuosMaxDistance = PlayerPrefs.GetFloat("maxscore", 0);
        if(travelledDistance > previuosMaxDistance){
            PlayerPrefs.SetFloat("maxscore",travelledDistance);
        }

        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if (this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }

        if(this.healthPoints <= 0){
            Die();
        }
    }

    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelledDistance(){
        return this.transform.position.x - startPosition.x;
    }

}
