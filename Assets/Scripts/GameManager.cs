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

    [SerializeField]
    public Sprite[] numbers;
    public Sprite cross;
    public GameObject[] polygons;
    public float decreaseTime = 0.5f;
    public TextMeshProUGUI percentageText;
    public Image brain;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PercentageCounter());
        Debug.Log(percentageText.text.Substring(0, percentageText.text.Length - 1));
    }

    IEnumerator PercentageCounter()
    {
        while(true)
        {
            yield return new WaitForSeconds(decreaseTime);
            percentage--;
            UpdatePercentage();
        }
    }

    public void UpdatePercentage()
    {
        percentageText.text = percentage.ToString() + "%";
        brain.fillAmount = percentage / 100f;
        Debug.Log(brain.fillAmount);
    }
}
