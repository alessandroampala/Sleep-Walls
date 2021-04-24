using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject arrow;
    float currentVelocity = 0;
    float targetAngle = 90;

    // Start is called before the first frame update
    void Start()
    {
        arrow.transform.eulerAngles = new Vector3(arrow.transform.eulerAngles.x, arrow.transform.eulerAngles.y, 90);
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
            GameObject hitted = hit.transform.gameObject;
            hitted.GetComponent<Polygon>().Number--;
        }
    }
}
