using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    public float timer = 1;
    private float timerProcess;
    public float speedRotate = 10;
    public Transform[] parts;
    public Vector3[] curPos;
    public Vector3[] newPos;
    public bool activate = false;

    private float angle = 0;

    public AnimationCurve animationMove;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].transform.localPosition = curPos[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activate)
        {
            timerProcess += Time.deltaTime;

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].transform.localPosition = Vector3.Lerp(curPos[i], newPos[i], animationMove.Evaluate(timerProcess / timer));
            }

            angle -= Time.deltaTime * speedRotate;
            transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(0, 180, animationMove.Evaluate(timerProcess / timer)), 0));

            if(timerProcess >= timer)
            {
                activate = false;
            }
        }
    }
}
