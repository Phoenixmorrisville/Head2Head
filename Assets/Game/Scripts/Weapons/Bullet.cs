using System.Numerics;
using Photon.Pun;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float amountOfCollisionsTillDeath = 1f;

    [SerializeField] private LayerMask playerLayer;
    private GameManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = FindFirstObjectByType<GameManager>();

        rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
    }

    void Update()
    {
        Vector2 direction = rb.linearVelocity.normalized;


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (amountOfCollisionsTillDeath <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        amountOfCollisionsTillDeath -= 1;

        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();
        if (dmg != null)
        {

            Debug.Log("Damage component is not null");
            dmg.TakeDamage(1);
        }






    }



}
