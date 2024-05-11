using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // FocalPoint'i odak noktasý alýp onun etrafýnda dönüyoruz
    private GameObject focalPoint;
    private Rigidbody playerRb;

    // Ýtme gücü
    private float powerUpStrangth = 70.0f;
    public float speed = 5.0f;
    public bool hasPowerUp = false;

    public GameObject powerupIndicator;
    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;

    // PowerUp sayacý
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
        MoveController();
        Indicator();
        ResetScene();
    }

    // Yön hareketleri
    public void MoveController()
    {
        // Yön hareketleri
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    // Player nesnemizin altýndaki çember
    public void Indicator()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.4f, 0);

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }
    }

    // PowerUp döngümüz (Coroutine) 
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
    // PowerUp döngümüz (Coroutine) 
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
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();

            // Düþman nesnesi konumundan player konumunu çýkartýr ve deðiþkene atar.
            // Hedef takip sistemi
            Vector3 awayFromPlayer = enemyRigidBody.transform.position - transform.position;
            enemyRigidBody.AddForce(awayFromPlayer * powerUpStrangth, ForceMode.Impulse);
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

    // Sahne Resetleme
    public void ResetScene()
    {
        float x = -340.0f;

        if (playerRb.position.y < x )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            print("Scene restarted");

            SpawnManager.roundSayac = 0;
            
        }                
    }
}
