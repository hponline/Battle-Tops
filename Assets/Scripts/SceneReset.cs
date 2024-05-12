using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneOnFall : MonoBehaviour
{
    public Transform playerTransform; // Oyuncu nesnesinin transform bileþeni
    public float sýnýrY = -50f; // Düþme eþiði

    void Update()
    {
        if (playerTransform.position.y < sýnýrY)
        {
            ResetScene();
        }
    }

    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Scene Reset");
        SpawnManager.roundSayac = 1;
    }
}
