using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TKJP.Test
{
    public class CubeController : MonoBehaviour
    {
        private Rigidbody rb;
        private Vector3 targetPos;

        // Start is called before the first frame update
        void Start()
        {
            rb = this.GetComponent<Rigidbody>();
        }
        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
#elif UNITY_ANDROID
            if(Input.touchCount > 0){
                targetPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
#endif
            targetPos.z = 0;
        }
        void FixedUpdate()
        {
            this.transform.LookAt(targetPos);

            Vector3 diff = targetPos - this.transform.position;
            rb.velocity = diff.normalized * Mathf.Min(5f, diff.magnitude);
        }
    }
}