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

    public SpriteRenderer sr;
    Animator animator;

    private void Awake()
    {
        EventManager.TileAppear.AddListener(TileAppear);
        EventManager.TileFade.AddListener(TileFade);
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Number = Random.Range(1, 9);
        //sr.sprite = GameManager.instance.numbers[number - 1];
        animator = GetComponent<Animator>();
    }

    void TileFade()
    {
        animator.SetTrigger("fade");
    }

    void TileAppear()
    {
        animator.SetTrigger("appear");
        Number = Random.Range(1, 3);
        //int indexOppositeTile = (IndexOf(GameManager.instance.polygons, PlayerController.lastPassedTile) + 4) % 8;
        //if (gameObject == GameManager.instance.polygons[indexOppositeTile])
        //{
        //    sr.sprite = GameManager.instance.cross;
        //    Number = -1;
        //}

        lock (GameManager.lockObj)
        {
            if(GameManager.crossNumber > 0)
            {
                GameManager.crossNumber--;
                sr.sprite = GameManager.instance.cross;
                Number = -1;
            }
            else
            {
                Number = GetLevelTileNumber(GameManager.level);
            }
        }
    }

    int IndexOf(GameObject[] objects, GameObject target)
    {
        for(int i = 0; i < objects.Length; i++)
        {
            if (objects[i] == target)
                return i;
        }
        return -1;
    }

    public void Wawe()
    {
        animator.SetTrigger("wawe");
    }

    int GetLevelTileNumber(int level)
    {
        if (level <= 5)
        {
            return Random.Range(1, 4);
        }
        else if (level <= 10)
        {
            return Random.Range(1, 5);
        }
        else if (level <= 20)
        {
            return Random.Range(2, 6);
        }
        else if (level <= 30)
        {
            bool b = Random.Range(0, 1) > 0.5f;
            if (b)
            {
                return Random.Range(4, 7);
            }
            else
            {
                return Random.Range(7, 9);
            }
        }
        else
        {
            return Random.Range(8, 9);
        }
    }
}
