using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 lastMousePos;
    public float sensitivity = .16f, clampDelta=42f;
    public float bounds = 5;

    [HideInInspector]
    public bool canMove,gameOver,finish;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();//script hangi objeye ba�l� ise o objenin rigidbody'si al�n�r
    }

     void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -bounds, bounds), transform.position.y, transform.position.z);//kenarlardan d��ar� ��kmamas� i�in x ekseninde s�n�r olu�turduk
        if(canMove)
           transform.position += FindObjectOfType<CameraMovement>().camVelocity;//kamera ve topun ayn� h�zda ilerlemesi i�in


        if(!canMove && !gameOver && !finish )
        {
            if(Input.GetMouseButtonDown(0))
            {           
                FindObjectOfType<GameManager>().RemoveUI();
                canMove = true;
            }
        }

        if(!canMove && gameOver)
        {
            if(Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))//ekrana bas�ld�g�nda ilk down olan� alg�l�yor fark bu
        {
            lastMousePos = Input.mousePosition;
            
        }
        if(canMove )
        {
            if (Input.GetMouseButton(0))//sol t�k mobilde ekrana dokunma anlam�na geliyor. Bas�l� tuttugumuzda da bu fonksiyonu alg�l�yor
            {
                Vector3 vector = lastMousePos - Input.mousePosition;
                lastMousePos = Input.mousePosition;
                vector = new Vector3(vector.x, 0, vector.y);

                Vector3 moveForce = Vector3.ClampMagnitude(vector, clampDelta);
                //rb.AddForce(Vector3.forward*2); bunu yazd���m�z zaman sadece d�md�z ileri gidiyor
                //rb.AddForce(Vector3.forward*2+(-moveForce *sensitivity) ekrana bas�l� tutup hareket ettirdi�imizdeki gitme i�ini yap�yor fakat �ok h�zl�
                //rb.AddForce(Vector3.forward * 2 + (-moveForce * sensitivity - rb.velocity));h�z� d���rmek i�in ��kar�yoruz
                rb.AddForce((-moveForce * sensitivity - rb.velocity / 5f), ForceMode.VelocityChange);//5e b�lme sebebi planede sa�a ve sola �ok yakla�m�yordu, o a��y� k�rabilmek i�in bu i�lemi yapt�k

            }

        }
            
        
        rb.velocity.Normalize();
    }

    private void GameOver()
    {
        canMove = false;
        gameOver = true;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator NextLevel()
    {
        finish = true;
        canMove = false;
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level"+ PlayerPrefs.GetInt("Level"));
    }

     void OnCollisionEnter(Collision target)
    {
        if(target.gameObject.tag=="Enemy")
        {
            GameOver();
        }
    }

     void OnTriggerEnter(Collider target)
    {
        if(target.gameObject.name=="Finish")
        {
            StartCoroutine(NextLevel());
        }
    }
}
