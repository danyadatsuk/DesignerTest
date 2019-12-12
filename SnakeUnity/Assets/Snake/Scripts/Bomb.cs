using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float radius = 1f;
    public float power = 300f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.collider.tag != "Player") return;

        var player = other.transform.GetComponent<HeadCallPlayer>().snakeTail;
        player.RememberPartNumber();
        player.playerController.StopControlDestroy(); 
        player.DestroyAll();  

        player.playerController.mainCamera.StopCameraDestroy();

        Manager.sendEventBomb = true;

        // Взрыв  
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }

        transform.gameObject.SetActive(false);  
    }
}
