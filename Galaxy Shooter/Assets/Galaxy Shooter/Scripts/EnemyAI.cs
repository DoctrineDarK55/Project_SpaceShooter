using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Variable 
    private float _Speed = 5.0f;
    [SerializeField] private GameObject _EnemyExplosionPrefab;

    private UIManager _UIManager;
    [SerializeField] private AudioClip _clip;

    // Use this for initialization
    void Start()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    
    }

    // Update is called once per frame
    void Update()
    {
        //move down
        transform.Translate(Vector3.down * _Speed * Time.deltaTime);

        //when offscreen at bottom, respawn on top with a new x position between the bounds of the screen.
        if (transform.position.y < -6.5f)
        {
            transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 6.5f, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Laser")
        {
            if(other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            Destroy(other.gameObject);
            Instantiate(_EnemyExplosionPrefab, transform.position, Quaternion.identity);
            _UIManager.UpdateScore();
            AudioSource.PlayClipAtPoint(_clip,Camera.main.transform.position, 1f);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
            }
            Instantiate(_EnemyExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position);
            Destroy(this.gameObject);
        }

    }
}
