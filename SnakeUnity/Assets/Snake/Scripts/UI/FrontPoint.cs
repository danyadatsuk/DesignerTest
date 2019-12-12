using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontPoint : MonoBehaviour
{
    public Image image;
    [HideInInspector] public bool animatedShow = false;
    [HideInInspector] public bool animatedHide = false;
    private float timer = 0.4f;
    public void SetImage(bool active, bool animate)
    {
        Color col = image.color;

        if(active)
        {
            image.color = new Color(col.r, col.g, col.b, 1);
            if(animate)
            {
                animatedShow = true;
                timer = 0.4f;
            }
        }
        else
        {
            image.color = new Color(col.r, col.g, col.b, 0);
        }
    }

    public void HideImage()
    {
        animatedHide = true;
        timer = 0.15f;
    }

    private void Update() 
    {
        if(animatedShow)
        {
            Color col = image.color;

            if(timer > 0)
            {
                timer -= Time.deltaTime;

                float delta = timer / 0.4f;
                float curScale = delta * 2f + 1;
                this.transform.localScale = new Vector3(curScale, curScale, curScale);
                image.color = new Color(col.r, col.g, col.b, 1 - delta);
            }
            else
            {
                animatedShow = false;
                this.transform.localScale = Vector3.one;
                image.color = new Color(col.r, col.g, col.b, 1);
            }
        }

        if(animatedHide)
        {
            Color col = image.color;

            if(timer > 0)
            {
                timer -= Time.deltaTime;

                float delta = timer / 0.15f;
                image.color = new Color(col.r, col.g, col.b, delta);
            }
            else
            {
                animatedHide = false;
                image.color = new Color(col.r, col.g, col.b, 0);
            }
        }
    }
}
