using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour {

    public SpriteAlpha lastHit;
    [Range(0,1)]
    public float fadeDuration;
    [Range(0, 1)]
    public float opacity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        Vector3 direction = (Camera.main.transform.position - transform.position);


        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity)){

            if(hit.transform.GetComponent<SpriteAlpha>() != null){
                if (lastHit != hit.transform.GetComponent<SpriteAlpha>())
                {
                    lastHit = hit.transform.GetComponent<SpriteAlpha>();
                    lastHit.Invisible(opacity, fadeDuration);
                }
            }
        }else if(lastHit != null){
            lastHit.Invisible(1,fadeDuration);
            lastHit = null;
        }
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = (Camera.main.transform.position - transform.position);
        Gizmos.DrawRay(transform.position, direction);
    }
}
