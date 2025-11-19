using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;


public class PlayerController : MonoBehaviour, IDamageable
{
    public PlayerData playerData;
    //References
    [SerializeField] private InputActionReference moveInput, jumpInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private BoxCollider2D boxCollider;
    public float playerSpeed;
    public float jumpForce;
    public int playerHealth = 1;
    public float groundDetectionRange;
    public LayerMask groundLayer;
    PhotonView view;
    [SerializeField] private GameObject gameOverText;

    [SerializeField] private GameObject bloodEffect;

    //player state
    [SerializeField] private PlayerState playerState = PlayerState.roaming;


    public GameObject[] limbs; //limbs are like the head and stuff



    private enum PlayerState
    {
        roaming,
        dead,
        gameOver
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //check what player you are, first or second

        playerState = PlayerState.roaming;
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerState)
        {
            case (PlayerState.roaming):
                //check if game started or not
                if (GameManager.gameState != GameManager.GameState.WaitingForPlayers)
                {
                    Movement();

                }

                break;
            case (PlayerState.dead):
                view.RPC("Dead", RpcTarget.All);
                playerState = PlayerState.gameOver;
                break;
            case (PlayerState.gameOver):
                break;
        }


    }

    [PunRPC]
    public void Dead()
    {
        boxCollider.enabled = false;
        //disable all renderers
        PhotonNetwork.Instantiate(bloodEffect.name, transform.position, transform.rotation);
        for (int i = 0; i < limbs.Length; i++)
        {
            limbs[i].SetActive(false);
        }
        PhotonNetwork.LoadLevel("Lobby");



    }






    private void Movement()
    {
        if (view.IsMine) // code only runs if this is my player character
        {
            rb.linearVelocity = new Vector2(moveInput.action.ReadValue<Vector2>().x * playerSpeed, rb.linearVelocityY);
            if (isGrounded() && jumpInput.action.IsPressed())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
                rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            }
        }
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(groundCheck.position, -Vector2.up, groundDetectionRange, groundLayer);
    }

    public void TakeDamage(int Damage)
    {
        playerHealth -= Damage;
        if (playerHealth <= 0)
        {
            playerState = PlayerState.dead;
        }
    }







}
