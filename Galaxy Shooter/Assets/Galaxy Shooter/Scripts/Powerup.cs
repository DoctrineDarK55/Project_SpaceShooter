using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerupID; //0 = TS,1 = Speed,2 = Shield

    [SerializeField] private AudioClip _clip;

	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //access the player
            Player player = other.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
            if (player != null)
            {
               
                //enable triple shot
                if(_powerupID == 0)
                {
                    player.TripleShotPowerupOn();
                }
                else if (_powerupID == 1)
                {
                    player.SpeedBoostPowerupOn();
                }
                else if (_powerupID == 2)
                {
                    player.ShieldPowerupOn();
                }
            }
            //destroy powerup
            Destroy(this.gameObject);
        }
    }
}
