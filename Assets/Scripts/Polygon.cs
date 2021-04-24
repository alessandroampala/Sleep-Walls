using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    private int _number;
    public int Number
    {
        get => _number;
        set
        {
            _number = value;
            if(_number > 0)
                sr.sprite = GameManager.instance.numbers[_number - 1];
        }
    }

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Number = Random.Range(1, 9);
        //sr.sprite = GameManager.instance.numbers[number - 1];
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
