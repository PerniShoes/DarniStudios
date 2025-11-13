using UnityEngine;

public class ParticleEnemyDetector : MonoBehaviour
{
    public ParticleSystem damageParticleSystem;
    public LayerMask enemyLayer;
    public float detectionRadius = 0.1f;

    void Start()
    {
        detectionRadius = damageParticleSystem.main.startSize.constant;
    }

    void Update()
    {

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[damageParticleSystem.particleCount];
        int count = damageParticleSystem.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 particlePos = particles[i].position;

            Collider[] hits = Physics.OverlapSphere(particlePos, detectionRadius, enemyLayer);
            foreach (var hit in hits)
            {
                GameObject enemyObj = hit.gameObject;
                EnemyHP enemyHealth = enemyObj.GetComponent<EnemyHP>();
                enemyHealth.TakeDamage(10);

            }
        }
    }
}
