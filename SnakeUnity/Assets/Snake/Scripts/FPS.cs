using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class FPS : MonoBehaviour 
{
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private float fps;
	public TextMeshProUGUI _text;

	private void Awake()
	{
		frames = 0;
	}

	private void OnEnable()
	{
		lastInterval = Time.realtimeSinceStartup;
		StartCoroutine(UpdateInfo());
	}

	private void Update()
	{
		++frames;
		float timeNow = Time.realtimeSinceStartup;
		if (timeNow > lastInterval + updateInterval) 
		{
			fps = (float) (frames / (timeNow - lastInterval));
			frames = 0;
			lastInterval = timeNow;
		}
	}
	
	private IEnumerator UpdateInfo()
	{
		while (isActiveAndEnabled)
		{
			_text.text = "FPS: " + $"{fps:f2}";
			
			yield return new WaitForSeconds(updateInterval);
			yield return new WaitForEndOfFrame();
		}
	}
	
}