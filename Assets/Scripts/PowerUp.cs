using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField] // 0 = Tripple Shot 1 = Speed 2 = Shield
    private int powerupID;

    [SerializeField]
    private AudioClip _clip;
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            player player = other.transform.GetComponent<player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);
            
            if (player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        Debug.Log("ShieldsActive");
                        player.ShieldsActive();
                        break;
                    case 3:
                        Debug.Log("HealthUp");
                        player.HealthUpActive();
                        break;
                    case 4:
                        Debug.Log("AmmoUpCollected");
                        player.AmmoUpActive();
                        break;
                    case 5:
                        Debug.Log("debuff");
                        player.Nerf();
                        break;
                    default:
                        Debug.Log("default value");
                        break;

                }
                
            }

            Destroy(this.gameObject);
        }
    }
    
}
