using UnityEngine;

public class ParticleEnemyDetector : MonoBehaviour
{
    public ParticleSystem ps;
    public LayerMask enemyLayer;
    public float detectionRadius = 0.1f;

    void Update()
    {

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int count = ps.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 particlePos = particles[i].position;

            Collider[] hits = Physics.OverlapSphere(particlePos, detectionRadius, enemyLayer);
            foreach (var hit in hits)
            {
                Debug.Log("Particle detected enemy: " + hit.name);
                GameObject enemyObj = hit.gameObject;
                EnemyHP enemyHealth = enemyObj.GetComponent<EnemyHP>();
                enemyHealth.TakeDamage(10);

            }
        }
    }
}
