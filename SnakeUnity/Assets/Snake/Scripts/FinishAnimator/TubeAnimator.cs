using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeAnimator : MonoBehaviour
{
    public bool startAnimation = false;
    // Start is called before the first frame update
    private float timer = 1;
    private float timerProcess;

    // Update is called once per frame
    void Update()
    {
        if(startAnimation)
        {
            timerProcess += Time.deltaTime;

            transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(-90f, 0f, 180f), new Vector3(-34f, 0f, 180f), timerProcess / timer));

            if(timerProcess >= timer)
            {
                startAnimation = false;
            }
        }
    }

    public void PlayAnimation()
    {
        startAnimation = true;
    }
}
