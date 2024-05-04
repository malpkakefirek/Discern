using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFlashlight : MonoBehaviour
{
    private Vector3 vectOffset;
    private GameObject goFollow;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float x = 0.0f;
    [SerializeField] private float y = 0.0f;
    [SerializeField] private float z = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        goFollow = Camera.main.gameObject;
        vectOffset = transform.position - goFollow.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = goFollow.transform.position + vectOffset + new Vector3(x,y,z);
        transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
    }
}
