using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // FocalPoint'i odak noktasý alýp onun etrafýnda dönüyoruz
    private GameObject focalPoint;
    private Rigidbody playerRb;
    private float powerUpStrangth = 30.0f;
    public float speed = 5.0f;
    public bool hasPowerUp = false;
    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        // Player nesnemizin altýndaki çember
        powerupIndicator.transform.position = transform.position + new Vector3(0, - 0.4f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            powerupIndicator.gameObject.SetActive(true);
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    // Güçlendiricimizi aldýktan sonra 7sn bekler ve baþa döner.
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            // Düþman nesnenin collision deðerini alýr ve deðiþkene atar.
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();

            // Düþman nesnesi konumundan player konumunu çýkartýr ve deðiþkene atar.
            Vector3 awayFromPlayer = enemyRigidBody.transform.position - transform.position;

            // AddForce methodu ile itme/dürtme gerçekleþtirilir.
            enemyRigidBody.AddForce(awayFromPlayer * powerUpStrangth, ForceMode.Impulse);


            Debug.Log("Player güçlendirme ile : " + collision.gameObject.name + " nesnesine çarptý " + hasPowerUp);
        }
    }
}
