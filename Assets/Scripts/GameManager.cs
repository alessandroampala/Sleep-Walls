using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int percentage = 100;
    public static int level = 0;
    public static int points = 0;
    public static object lockObj = new object ();
    public static int crossNumber = 0;

    [SerializeField]
    public Sprite[] numbers;
    public Sprite cross;
    public GameObject[] polygons;
    public float decreaseTime = 0.5f;
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI phaseText;
    public Image brain;

    public UnityEngine.Rendering.Volume postProcessing;
    public UnityEngine.Rendering.Universal.Bloom bloom;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PercentageCounter());
        Debug.Log(percentageText.text.Substring(0, percentageText.text.Length - 1));
        postProcessing.profile.TryGet(out bloom);
        AudioManager.Play("Soundtrack");
    }

    IEnumerator PercentageCounter()
    {
        while(true)
        {
            Debug.Log(Math.Max(decreaseTime - level / 100f, 0.1f));
            yield return new WaitForSeconds(Math.Max(decreaseTime - level / 100f, 0.2f));
            percentage--;
            UpdatePercentage();
        }
    }

    public void UpdatePercentage()
    {
        percentageText.text = percentage.ToString() + "%";
        brain.fillAmount = percentage / 100f;
        Color bloomColor = Color.Lerp(Color.red, Color.white, percentage / 100f);
        bloom.tint.Override(bloomColor);

        if(percentage > 75)
        {
            phaseText.text = "Phase: Deep Sleep";
        }
        else if (percentage > 50)
        {
            phaseText.text = "Phase: Light Sleep";
        }
        else if (percentage > 15)
        {
            phaseText.text = "Phase: REM";
        }
        else
        {
            phaseText.text = "Phase: Awake";
        }
        phaseText.color = Color.Lerp(Color.red, Color.green, percentage / 100f);
    }

    public void UpdatePoints()
    {
        pointsText.text = "Points: " + points.ToString();
    }
}
