using UnityEngine;

namespace Benjathemaker
{
    public class SimpleGemsAnim : MonoBehaviour
    {
        [HideInInspector] public bool isBeingAttracted = false;

        [Header("Animation Settings")]
        public float rotationSpeed = 50f;   // degrees per second
        public float floatAmplitude = 0.2f; // how high it floats
        public float floatFrequency = 1f;   // how fast it floats

        private Vector3 startPosition;
        private float randomOffset; // random phase so gems don't float identically

        void Start()
        {
            startPosition = transform.position;
            randomOffset = Random.Range(0f, Mathf.PI * 2f); // random wave phase
        }

        void Update()
        {
            // No animation if going to player
            if (isBeingAttracted) return;

            // Rotate
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);

            // Sinusoida :3
            float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency + randomOffset) * floatAmplitude;
            Vector3 pos = transform.position;
            pos.y = newY;
            transform.position = pos;
        }
    }
}
