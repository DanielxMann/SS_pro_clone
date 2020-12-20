using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Lvl2 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private player _player;

    
    private bool _isEnemyDead = false;
    private bool _isShieldsActive = true;
    [SerializeField]
    private GameObject _shieldVisualizer;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<player>();

        //null check player
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
/*        if (_isShieldsActive == true)
        {
            _isShieldsActive = true;
            _shieldVisualizer.SetActive(true);
        }
        else if (_isShieldsActive == false)
        {
            {
                _isShieldsActive = false;
                _shieldVisualizer.SetActive(false);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.localRotation = Quaternion.Euler(180, 0, 0);
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            float randomX = Random.Range(-17.5f, 17.5f);
            transform.position = new Vector3(randomX, 9, 0);
        }

        if (transform.position.x >= 17.5f)
        {
            transform.position = new Vector3(-17.5f, transform.position.y, 0);
        }

        else if (transform.position.x <= -17.5f)
        {
            transform.position = new Vector3(17.5f, transform.position.y, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            player player = other.transform.GetComponent<player>();

            if (player != null)
            {
                player.Damage();
            }
            if (_shieldVisualizer == true)
            {
                _shieldVisualizer.SetActive(false);
            }
            else
            {
                _speed = 0;
                _isEnemyDead = true;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.5f);
            }
        }

        if (other.tag == "Laser")
        {

            if (_player != null)
            {
                _player.AddScore(30);

                if (_shieldVisualizer == true)
                {
                    _shieldVisualizer.SetActive(false);
                }
                else if (_shieldVisualizer == false)
                {
                    _speed = 0;
                    _isEnemyDead = true;
                    Destroy(GetComponent<Collider2D>());
                    Destroy(other.gameObject);
                    Destroy(this.gameObject, 2.5f);
                }
            }
        }

        if (other.tag == "SecondaryFire")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }
            if (_shieldVisualizer == true)
            {
                _shieldVisualizer.SetActive(false);
            }
            else
            {
                _speed = 0;
                _isEnemyDead = true;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.5f);
            }
        }
    }
}
