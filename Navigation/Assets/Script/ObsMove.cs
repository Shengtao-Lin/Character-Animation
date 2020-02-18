using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsMove : MonoBehaviour
{

    public float speed = .2f;
	public float strength = 9f;

	private float offset;
    // Start is called before the first frame update
    void Start()
    {
        offset=Random.Range(0f,2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time*speed+offset)*strength;
        transform.position=pos;

    }
}
