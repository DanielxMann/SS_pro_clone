using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    private player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;


    private SpriteRenderer mySpriteRenderer;


    private bool _isEnemyDead = false;
    void Start()
    {

        _player = GameObject.Find("Player").GetComponent<player>();
        _audioSource = GetComponent<AudioSource>();
        //null check player
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
    }
    void Update()
    {
        CalculateMovement();


        // if the variable isn't empty (we have a reference to our SpriteRenderer
        if (mySpriteRenderer != null)
        {
            // flip the sprite
            mySpriteRenderer.flipX = true;
        }
        if (Time.time > _canFire && _isEnemyDead == false)
        {

            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Debug.Log("instatiate lenemy laser");
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.localRotation = Quaternion.Euler(180, 0, 0);
        transform.Translate(Vector3.up * _speed * Time.deltaTime);// lazy way of getting sprite flip to work

        if (transform.position.y < -10f)
        {
            float randomX = Random.Range(-17.5f, 17.5f);
            transform.position = new Vector3(randomX, 10, 0);
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
            
            _speed = 0;
            _isEnemyDead = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
            _audioSource.Play();
        }

        if (other.tag == "Laser")
        {
            
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _speed = 0;
            _isEnemyDead = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
            Destroy(other.gameObject);
            _audioSource.Play();
        }

        if (other.tag == "SecondaryFire")
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }

            _speed = 0;
            _isEnemyDead = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
            _audioSource.Play();
        }
        SpawnManager controller = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent("SpawnManager") as SpawnManager;
        controller.KilledEnemy();
    }
}