using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameObject arrow;
    public GameObject ground;
    float currentVelocity = 0;
    float targetAngle = 90;

    Vector2 terrainDest;
    public float groundMovement = 2f;
    public float groundSmoothTime = 1;
    Vector2 currentGroundVelocity;
    bool lastTileLock = false;

    public static GameObject lastPassedTile = null;

    public GameObject psObject;
    ParticleSystem ps;

    int currentHits = 0;

    private void Awake()
    {
        EventManager.ToGame.AddListener(() => { ground.transform.position = Vector3.zero;});
    }

    // Start is called before the first frame update
    void Start()
    {
        terrainDest = ground.transform.position;
        arrow.transform.eulerAngles = new Vector3(arrow.transform.eulerAngles.x, arrow.transform.eulerAngles.y, 90);
        ps = psObject.GetComponent<ParticleSystem>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        
        if (!(input == Vector2.zero))
        {
            targetAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        }
        float smoothedAngle = Mathf.SmoothDampAngle(arrow.transform.eulerAngles.z, targetAngle, ref currentVelocity, 0.15f);
        arrow.transform.eulerAngles = new Vector3(arrow.transform.eulerAngles.x, arrow.transform.eulerAngles.y, smoothedAngle);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit2D hit = Physics2D.Raycast(arrow.transform.position, arrow.transform.right);

            if (hit && !lastTileLock)
            {

                GameObject hitted = hit.transform.gameObject;

                //if hitted a cross
                if (hitted.GetComponent<Polygon>().sr.sprite == GameManager.instance.cross)
                {
                    GameManager.percentage -= 5;
                    GameManager.instance.UpdatePercentage();
                    AudioManager.Play("Error");
                    AddPoints(-(currentHits + 2) * 100);
                    currentHits = 0;
                }
                else
                {
                    hitted.GetComponent<Polygon>().Wawe();
                    currentHits++;
                    hitted.GetComponent<Polygon>().Number--;
                    if (hitted.GetComponent<Polygon>().Number == 0)
                    {
                        SetLevelParameters(++GameManager.level);
                        AudioManager.Play("Destroy");
                        psObject.transform.position = hitted.transform.position;
                        ps.Play();
                        Move(hitted);
                        EventManager.TileFade.Invoke();
                        StartCoroutine(AppearTiles());
                        lastPassedTile = hitted;
                        GameManager.percentage = Mathf.Clamp(GameManager.percentage + 5 + currentHits / 2, 0, 100);
                        lastPassedTile.SetActive(false);
                        lastTileLock = true;

                        AddPoints(currentHits * 100);
                        currentHits = 0;
                    }

                }
            }

        }

        TerrainMove();
    }

    void Move(GameObject tile)
    {
        Vector2 direction = (transform.position - tile.transform.position).normalized;
        terrainDest = ground.transform.position + new Vector3(direction.x, direction.y, 0) * groundMovement;
    }

    void TerrainMove()
    {
        ground.transform.position = Vector2.SmoothDamp(ground.transform.position, terrainDest, ref currentGroundVelocity, groundSmoothTime);
    }

    private IEnumerator AppearTiles()
    {
        yield return new WaitForSeconds(groundSmoothTime * 1.2f);
        lastPassedTile.SetActive(true);
        EventManager.TileAppear.Invoke();
        lastTileLock = false;
    }

    void SetLevelParameters(int level)
    {
        if(level <= 5)
        {
            GameManager.crossNumber = Random.Range(1, 2);

        }
        else if(level <= 10)
        {
            GameManager.crossNumber = Random.Range(1, 3);
        }
        else if(level <= 20)
        {
            GameManager.crossNumber = Random.Range(2, 5);
        }
        else if(level <= 30)
        {
            bool b = Random.Range(0, 1) > 0.5f;
            if(b)
            {
                GameManager.crossNumber = Random.Range(3, 7);
            }
            else
            {
                GameManager.crossNumber = Random.Range(2, 4);
            }
        }
        else
        {
            GameManager.crossNumber = Random.Range(4,5);
        }
    }

    void AddPoints(int amount)
    {
        GameManager.points += amount;
        GameManager.points = Mathf.Max(GameManager.points, 0);

        GameManager.instance.UpdatePoints();
    }
}
