using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;

    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 7.5f)
        {

            if (transform.parent != null)
            {
                Debug.Log("T destroyed");
                Destroy(transform.parent.gameObject);
            }
            Debug.Log("laser destroyed");
            Destroy(gameObject);
        }
    }

    void MoveDown()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            if (transform.parent != null)
            {
                Debug.Log("destroyed");
                Destroy(transform.parent.gameObject);
            }
            Debug.Log("Enemy laser destroyed");
            Destroy(gameObject);    
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player" && _isEnemyLaser == true)
        {
            player player = other.GetComponent<player>();

            if (player !=null)
            {
                player.Damage();
            }
        }

        if (other.tag == "SecondaryFire")
        {
            Destroy(this.gameObject);
        }
    }
}
