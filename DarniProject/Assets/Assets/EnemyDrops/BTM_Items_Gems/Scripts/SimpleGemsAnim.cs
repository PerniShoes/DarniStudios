using UnityEngine;
using System.Collections;

namespace Benjathemaker
{
    public class SimpleGemsAnim : MonoBehaviour
    {
        [HideInInspector] public bool isBeingAttracted = false;

        [Header("Idle Animation")]
        public float rotationSpeed = 50f;    
        public float floatAmplitude = 0.2f;  
        public float floatFrequency = 1f;    

        [Header("Drop Animation")]
        public float dropMoveDistance = 0.3f;  
        public float dropDuration = 0.3f;      

        private Vector3 startPos;
        private Vector3 dropTargetPos;
        private bool isDropping = true;

        public void DropGem()
        {
            startPos = transform.position;

            Vector2 random2D = Random.insideUnitCircle.normalized;
            dropTargetPos = startPos + new Vector3(random2D.x, Random.Range(0.1f, 0.3f), random2D.y) * dropMoveDistance;

            isDropping = true;
            StartCoroutine(DropAnimation());
        }

        IEnumerator DropAnimation()
        {
            float t = 0f;
            while (t < dropDuration)
            {
                t += Time.deltaTime;
                float progress = t / dropDuration;

                // Smooth curve (ease out)
                transform.position = Vector3.Lerp(startPos, dropTargetPos, 1 - Mathf.Pow(1 - progress, 2));

                yield return null;
            }

            startPos = transform.position;
            isDropping = false;
        }

        void Update()
        {
            if (isBeingAttracted || isDropping) return;

            //// Rotate gem
            //transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);

            //// Floating idle animation
            //float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            //Vector3 pos = transform.position;
            //pos.y = newY;
            //transform.position = pos;
        }
    }
}
