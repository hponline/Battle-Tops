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

    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

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

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerUp = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());

            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown); 
            }
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
        }
    }

    // Güçlendiricimizi aldýktan sonra 7sn bekler ve baþa döner.
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            // Düþman nesnenin collision deðerini alýr ve deðiþkene atar.
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();

            // Düþman nesnesi konumundan player konumunu çýkartýr ve deðiþkene atar.
            Vector3 awayFromPlayer = enemyRigidBody.transform.position - transform.position;

            // AddForce methodu ile itme/dürtme gerçekleþtirilir.
            enemyRigidBody.AddForce(awayFromPlayer * powerUpStrangth, ForceMode.Impulse);


            Debug.Log("Player güçlendirme ile : " + collision.gameObject.name + " nesnesine çarptý " + currentPowerUp.ToString());
        }
    }

    // Roket
    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehavior>().Fire(enemy.transform);
        }
    }
}
