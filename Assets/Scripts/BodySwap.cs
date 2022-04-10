using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySwap : MonoBehaviour
{
    public GameObject player;
    RougeController rougeController;
    GameObject _camera;
    
    private void Start()
    {
        player = GameObject.Find("Player 2D");
        _camera = Camera.main.gameObject;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            player.GetComponent<RougeController>().enabled = false;
            collision.gameObject.GetComponent<RougeController>().enabled = true;
            _camera.transform.SetParent(collision.transform, false);
<<<<<<< HEAD
            Destroy(gameObject);
=======
            //Destroy(gameObject);
            gameObject.SetActive(false);
>>>>>>> d304b0d0d893b133da15b939214a8313c9f9996f
            GameManager.instance.currentPlayer.Add(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.currentPlayer[0].GetComponent<RougeController>().enabled = false;
            player.GetComponent<RougeController>().enabled = true;
            _camera.transform.SetParent(collision.transform, false);
<<<<<<< HEAD
            Destroy(gameObject);
=======
            //Destroy(gameObject);
            gameObject.SetActive(false);
>>>>>>> d304b0d0d893b133da15b939214a8313c9f9996f
        }
    }
}