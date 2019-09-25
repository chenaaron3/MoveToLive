using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class HeroController : MonoBehaviour
{
    public static HeroController instance;

    public float speed;
    bool blank = true;
    float shakeAmt = .05f;
    float shakeTime = .5f;
    Vector3 cameraInitPos;

    GameObject iceShield;
    int iceCombo;
    public int IceCombo
    {
        get
        {
            return iceCombo;
        }
        set
        {
            Debug.Log(iceCombo);
            iceCombo = value;
            // UPGRADE ICE 2 ICE SHIELD
            if (EnhancementManager.instance.upgrades["Ice"].upgrade[2])
            {
                if (iceCombo % 6 == 0)
                {
                    StartCoroutine(UseIceShield());
                }
            }
            // UPGRADE ICE 3 SPEED UP
            if (EnhancementManager.instance.upgrades["Ice"].upgrade[3])
            {
                if (iceCombo % 3 == 0)
                {
                    StartCoroutine(SpeedBoost());
                }
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        iceShield = transform.Find("IceShield").gameObject;
        cameraInitPos = Camera.main.transform.position;
    }

    private void Update()
    {

        // if not going towards wall
        Vector3 direction = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical")).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, .1f, 1 << 9);
        if (!hit)
        {
            direction = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxisRaw("Vertical")).normalized;
            if (direction.magnitude != 0)
            {
                transform.position += direction * speed * Time.deltaTime;
                transform.right = direction;
            }
            else
            {
                transform.position += transform.right * Time.deltaTime;
            }
        }



        if (Input.GetKey(KeyCode.Space) && blank)
        {
            blank = false;
            foreach (Enemy e in FindObjectsOfType<Enemy>())
            {
                e.Die("Fire");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.GameOver();
        }
    }

    public IEnumerator CameraShake()
    {
        float time = 0;
        while (time < shakeTime)
        {
            float shakeMultiplyer = (1 - time / shakeTime);
            Vector3 pp = Camera.main.transform.position;
            float quakeAmt = shakeMultiplyer * shakeAmt * (Random.value * 2 - 1);
            pp.y += quakeAmt;
            quakeAmt = shakeAmt * (Random.value * 2 - 1);
            pp.x += quakeAmt;
            Camera.main.transform.position = pp;
            yield return new WaitForEndOfFrame();
            time += Time.fixedDeltaTime;
            Camera.main.transform.position = cameraInitPos;
            yield return new WaitForEndOfFrame();
        }
        Camera.main.transform.position = cameraInitPos;
    }

    IEnumerator UseIceShield()
    {
        iceShield.SetActive(true);
        yield return new WaitForSeconds(3);
        iceShield.SetActive(false);
    }

    IEnumerator SpeedBoost()
    {
        float initSpeed = speed;
        speed += 1;

        while (speed > initSpeed)
        {
            speed -= 1 / .5f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        speed = initSpeed;
    }
}
