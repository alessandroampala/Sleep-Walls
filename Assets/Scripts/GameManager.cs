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
    static bool inGame = false;
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
    public TextMeshProUGUI deathText;
    public Image brain;

    public UnityEngine.Rendering.Volume postProcessing;
    public UnityEngine.Rendering.Universal.Bloom bloom;

    public GameObject[] menuObjects;
    public GameObject[] gameObjects;
    public GameObject[] deathObjects;

    public Image tentacleLeft;
    public Image tentacleRight;


    private void Awake()
    {
        instance = this;
        EventManager.ToMenu.AddListener(ToMenu);
        EventManager.ToGame.AddListener(ToGame);
        EventManager.Death.AddListener(Death);
    }

    // Start is called before the first frame update
    void Start()
    {
        postProcessing.profile.TryGet(out bloom);
        EventManager.ToMenu.Invoke();
    }

    IEnumerator PercentageCounter()
    {
        while(true)
        {
            Debug.Log(Math.Max(decreaseTime - level / 100f, 0.1f));
            yield return new WaitForSeconds(Math.Max(decreaseTime - level / 100f, 0.2f));
            percentage--;
            UpdatePercentage();
            if(percentage <= 0)
            {
                EventManager.Death.Invoke();
            }
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
            phaseText.text = "Stage: Deep Sleep";
        }
        else if (percentage > 50)
        {
            phaseText.text = "Stage: Light Sleep";
        }
        else if (percentage > 15)
        {
            phaseText.text = "Stage: REM";
        }
        else
        {
            phaseText.text = "Stage: Awake";
        }
        phaseText.color = Color.Lerp(Color.red, Color.green, percentage / 100f);
        tentacleLeft.color = tentacleRight.color = Color.Lerp(Color.red, Color.white, percentage / 100f);
    }

    public void UpdatePoints()
    {
        pointsText.text = "Points: " + points.ToString();
    }

    public void ToMenu()
    {
        foreach(GameObject o in menuObjects)
        {
            o.SetActive(true);
        }
        foreach (GameObject o in gameObjects)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in deathObjects)
        {
            o.SetActive(false);
        }
        inGame = false;
        StopCoroutine(PercentageCounter());
    }

    public void ToGame()
    {
        foreach (GameObject o in menuObjects)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in gameObjects)
        {
            o.SetActive(true);
        }
        foreach (GameObject o in deathObjects)
        {
            o.SetActive(false);
        }
        inGame = true;
        level = 0;
        percentage = 100;
        StartCoroutine(PercentageCounter());
        AudioManager.Play("Soundtrack");
    }

    public void Death()
    {
        deathText.color = Color.red;
        deathText.text = "You woke up!\nPoints: " + points;
        foreach (GameObject o in menuObjects)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in gameObjects)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in deathObjects)
        {
            o.SetActive(true);
        }
        StopCoroutine(PercentageCounter());
        AudioManager.Stop("Soundtrack");
    }
}
