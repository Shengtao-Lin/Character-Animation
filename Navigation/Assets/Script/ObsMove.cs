using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsMove : MonoBehaviour
{

    public float speed = .2f;
	public float strength = 9f;

    public int flag;

	private float offset;
    // Start is called before the first frame update
    void Start()
    {
        offset=Random.Range(0f,2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(flag == 0)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Sin(Time.time*speed+offset)*strength;
            transform.position=pos;
        }
        if(flag == 1)
        {
            Vector3 pos = transform.position;
            pos.z = Mathf.Sin(Time.time*speed+offset)*strength;
            transform.position=pos;
        }

    }
}
