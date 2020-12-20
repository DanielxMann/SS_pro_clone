using System.Collections;
using UnityEngine;

public class player : MonoBehaviour
{
    private bool cameraShake;
    
    [SerializeField]
    private float _speed = 5.5f;
    private float _speedMultiplier = 2;

    [SerializeField]
    private GameObject _SecondaryFirePrefab;
    [SerializeField]
    private GameObject _LazerPrefab;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    private float _fireRate = 0f;
    [SerializeField]
    private float _nextFire = 0f;
    private float _secondaryFire = 50f;
    private float _secondaryDuration = 15f;

    public int maxAmmo = 20;
    [SerializeField]
    private int currentAmmo;

    private UIManager _uIManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private GameObject _HealthUpPrefab;
    private float _immunityStart = 0f;
    public float immunityDuration = 1f;

    private bool _isAmmoUpActive = false;
    private bool _isHealthUpActive = false;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private bool _nerfActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _shieldLives = 3;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;
    [SerializeField]
    private int _ammoText;

    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioSource _audioSource;


    void Start()
    {
        currentAmmo = maxAmmo;

        transform.position = new Vector3(0, -1, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uIManager == null)
        {
            Debug.Log("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source onPlayer is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }


    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && currentAmmo > 0)
        {
            FireLazer();
        }

        if (Input.GetKeyDown(KeyCode.Q) && Time.time > _nextFire)
        {
            MegaFire();
        }
    }

    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= -1)
        {
            transform.position = new Vector3(transform.position.x, -1, 0);
        }

        else if (transform.position.y <= -9f)
        {
            transform.position = new Vector3(transform.position.x, -9f, 0);
        }

        if (transform.position.x >= 18)
        {
            transform.position = new Vector3(-18, transform.position.y, 0);
        }

        else if (transform.position.x <= -18)
        {
            transform.position = new Vector3(18, transform.position.y, 0);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }

    }

    void MegaFire()
    { 
      Instantiate(_SecondaryFirePrefab, transform.position, Quaternion.identity, transform);      
    }

    

    void FireLazer()
    {
        currentAmmo--;
        _uIManager.UpdateAmmo(currentAmmo);

        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
        }

        _nextFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
 
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_LazerPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }
    public void Damage()
    {
        CameraShake.instance.StartShake(.5f, .25f);

        if (_isShieldsActive == true && _immunityStart <= Time.time)
        {
            _shieldLives--;
            _immunityStart = Time.time + immunityDuration;
            

            if (_shieldLives == 2)
            {
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
            }
            else if (_shieldLives == 1)
            {
                _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            }
            else if (_shieldLives == 0)
            {
                _isShieldsActive = false;
                _shieldVisualizer.SetActive(false);
            }
            return;
        }

        if (_isShieldsActive == false && _immunityStart <= Time.time)
        {
            _playerLives--;
            _immunityStart = Time.time + immunityDuration;

            if (_playerLives == 2)
            {
                _leftEngine.SetActive(true);
            }
            else if (_playerLives == 1)
            {
                _rightEngine.SetActive(true);
            }
        }
        if (_playerLives < 1)
        {
            _playerLives = 0;
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

        _uIManager.UpdateLives(_playerLives);
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        currentAmmo = maxAmmo;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
    public void ShieldsActive()
    {
        Debug.Log("test3");
        _shieldLives = 3;
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldVisualizer.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uIManager.UpdateScore(_score);
    }

/*    private void AmmoCount(int currentAmmo)
    {
        _ammoText += currentAmmo;
        _uIManager.UpdateAmmo(_ammoText);
    }*/

    public void HealthUpActive() 
    {
        _isHealthUpActive = true;
        _playerLives++;
        if (_playerLives > 3)
        {
            _playerLives = 3;
        }
        _uIManager.UpdateLives(_playerLives);
    }

    public void AmmoUpActive()
    {
        _isAmmoUpActive = true;
        currentAmmo = maxAmmo;
    }
    
    public void Nerf()
    {
        _nerfActive = true;
        currentAmmo = 0;
        _shieldLives = 0;
        _isShieldsActive = false;
        _shieldVisualizer.SetActive(false);

        CalculateMovement();
        if (_nerfActive == true)
        {
            Vector3 direction = new Vector3(0, 0, 0);
        }
    }
}

