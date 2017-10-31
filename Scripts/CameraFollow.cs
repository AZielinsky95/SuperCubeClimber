using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour {

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public GameObject target;


    // Update is called once per frame
    void Update()
    {
            float posY = Mathf.SmoothDamp(this.transform.position.y, target.transform.position.y, ref velocity.y, dampTime);
            if (posY > this.transform.position.y)
            {
                this.transform.position = new Vector3(this.transform.position.x, posY, this.transform.position.z);
            }
	}
}
