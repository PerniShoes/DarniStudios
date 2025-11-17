using UnityEngine;
using System.Collections;

namespace Benjathemaker
{
    public class SimpleGemsAnim : MonoBehaviour
    {
        [HideInInspector] public bool isBeingAttracted = false;

        [Header("Idle Animation")]
        public float rotationSpeed = 50f;    // How fast the gem spins
        public float floatAmplitude = 0.2f;  // Floating height
        public float floatFrequency = 1f;    // Floating speed

        [Header("Drop Animation")]
        public float dropMoveDistance = 0.5f;  // How far it moves away after spawn
        public float dropDuration = 0.3f;      // How long the drop animation lasts

        private Vector3 startPos;
        private Vector3 dropTargetPos;
        private float randomOffset;
        private bool isDropping = true;

        void Start()
        {
            startPos = transform.position;
            randomOffset = Random.Range(0f, Mathf.PI * 2f);

            // Random direction for initial "drop"
            Vector2 random2D = Random.insideUnitCircle.normalized;
            dropTargetPos = startPos + new Vector3(random2D.x, Random.Range(0.2f, 0.6f), random2D.y) * dropMoveDistance;

            // Start simple drop animation
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

            // Rotate gem
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);

            // Floating idle animation
            float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency + randomOffset) * floatAmplitude;
            Vector3 pos = transform.position;
            pos.y = newY;
            transform.position = pos;
        }
    }
}
