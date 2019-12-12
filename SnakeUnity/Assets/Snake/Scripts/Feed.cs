using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feed : MonoBehaviour
{
    private Transform tr;
    private Vector3 rotate;
    void Start()
    {
        tr = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        rotate = tr.localRotation.eulerAngles;
        tr.localRotation = Quaternion.Euler(rotate.x, rotate.y + 100 * Time.deltaTime, rotate.z);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.collider.tag != "Player") return;

        other.transform.GetComponent<HeadCallPlayer>().snakeTail.AddPart();    

        this.gameObject.SetActive(false);  
    }
}
