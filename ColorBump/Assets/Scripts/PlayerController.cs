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
        rb = GetComponent<Rigidbody>();//script hangi objeye baðlý ise o objenin rigidbody'si alýnýr
    }

     void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -bounds, bounds), transform.position.y, transform.position.z);//kenarlardan dýþarý çýkmamasý için x ekseninde sýnýr oluþturduk
        if(canMove)
           transform.position += FindObjectOfType<CameraMovement>().camVelocity;//kamera ve topun ayný hýzda ilerlemesi için


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
        if (Input.GetMouseButtonDown(0))//ekrana basýldýgýnda ilk down olaný algýlýyor fark bu
        {
            lastMousePos = Input.mousePosition;
            
        }
        if(canMove )
        {
            if (Input.GetMouseButton(0))//sol týk mobilde ekrana dokunma anlamýna geliyor. Basýlý tuttugumuzda da bu fonksiyonu algýlýyor
            {
                Vector3 vector = lastMousePos - Input.mousePosition;
                lastMousePos = Input.mousePosition;
                vector = new Vector3(vector.x, 0, vector.y);

                Vector3 moveForce = Vector3.ClampMagnitude(vector, clampDelta);
                //rb.AddForce(Vector3.forward*2); bunu yazdýðýmýz zaman sadece dümdüz ileri gidiyor
                //rb.AddForce(Vector3.forward*2+(-moveForce *sensitivity) ekrana basýlý tutup hareket ettirdiðimizdeki gitme iþini yapýyor fakat öok hýzlý
                //rb.AddForce(Vector3.forward * 2 + (-moveForce * sensitivity - rb.velocity));hýzý düþürmek için çýkarýyoruz
                rb.AddForce((-moveForce * sensitivity - rb.velocity / 5f), ForceMode.VelocityChange);//5e bölme sebebi planede saða ve sola çok yaklaþmýyordu, o açýyý kýrabilmek için bu iþlemi yaptýk

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
