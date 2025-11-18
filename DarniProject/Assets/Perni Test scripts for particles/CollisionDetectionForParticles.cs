using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleEnemyDetector : MonoBehaviour
{
    public ParticleSystem damageParticleSystem;
    public LayerMask enemyLayer;
    public float detectionRadius = 0.1f;

    private ParticleSystem.Particle[] particles;
    private Collider[] hitBuffer = new Collider[100]; // adjust size based on max expected collisions

    void Start()
    {
        detectionRadius = damageParticleSystem.main.startSize.constant;
        particles = new ParticleSystem.Particle[damageParticleSystem.main.maxParticles];
    }

    void Update()
    {

        int count = damageParticleSystem.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            Vector3 particlePos = particles[i].position;

            int hitsCount = Physics.OverlapSphereNonAlloc(particlePos, detectionRadius, hitBuffer, enemyLayer);
            for (int j = 0; j < hitsCount; j++)
            {
                GameObject enemyObj = hitBuffer[j].gameObject;
                EnemyHP enemyHealth = enemyObj.GetComponent<EnemyHP>();
                enemyHealth.TakeDamage(1);
            }
        }
    }
}
