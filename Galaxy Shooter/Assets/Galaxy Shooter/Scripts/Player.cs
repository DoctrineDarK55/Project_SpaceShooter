using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //variables
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _ExplosionPrefab;
    [SerializeField] private GameObject _shieldGameObject;
    [SerializeField] private float _fireRate = 0.25f;

    [SerializeField] private GameObject[] _engines;

    private float _canFire = 0.0f;
    
    [SerializeField] private float _speed = 5.0f;

    public bool canTripleShot = false;
    public bool canSpeedBoost = false;
    public bool canShield = false;

   public int playerLives = 3;

    private UIManager _UIManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    private int hitCount = 0;

    void Start()
    {
        //current pos = new position
        transform.position = new Vector3(0, 0, 0);

        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_UIManager != null)
        {
            _UIManager.UpdateLives(playerLives);
        }
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager != null)
        {
            _spawnManager.startSpawnRoutines();
        }

        _audioSource = GetComponent<AudioSource>();

        hitCount = 0;
    }


    void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        if (Time.time > _canFire)
        {
            _audioSource.Play();
            _canFire = Time.time + _fireRate;
            if (canTripleShot == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.95f, 0), Quaternion.identity);
            }
        }
    }

    public void Damage()
    {
       
        if(canShield == true)
        {
            canShield = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        hitCount++;

        if (hitCount == 1)
        {
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2)
        {
            _engines[1].SetActive(true);
        }

        playerLives--;
        _UIManager.UpdateLives(playerLives);

        if (playerLives < 1)
        {
            Instantiate(_ExplosionPrefab, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _UIManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (canSpeedBoost == true)
        {
            transform.Translate(Vector3.right * _speed * 1.5f * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * 1.5f * verticalInput * Time.deltaTime);
        }
        else
        {
            //Used for player movement up,down,left,right.
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);
        }

        //Player can move up or down. Limited on the Y Axis.
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }

        //Wraps players movement on the x Axis.
        if (transform.position.x > 9.1f)
        {
            transform.position = new Vector3(-9.1f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.1f)
        {
            transform.position = new Vector3(9.1f, transform.position.y, 0);
        }
    }

    public void TripleShotPowerupOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostPowerupOn()
    {
        canSpeedBoost = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
       
    }
    public void ShieldPowerupOn()
    {
        canShield = true;
        _shieldGameObject.SetActive(true);
    }

    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canSpeedBoost = false;
    }

    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

}
 