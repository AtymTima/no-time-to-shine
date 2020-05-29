using UnityEngine;

public class JumpParticles : MonoBehaviour
{
    [SerializeField] private float timeBeforeNextRunParticles = 0.25f;

    PoolLifetime objectFromPool;
    ObjectPool<PoolLifetime> objectPool;

    public delegate void OnParticlesBegin(float waitForSeconds, bool stopNow);
    public static event OnParticlesBegin onParticlesBegin = delegate { };

    private bool startedRunning;

    private void Awake()
    {
        JumpSimple.OnGroundLanding += SummonParticles;
        Player.OnRunning += ParticlesOnRunning;
        UpdateManager.onJumpParticlesEnd += SummonRunningParticles;
    }

    private void Start()
    {
        objectPool = ObjectPool<PoolLifetime>.Instance;
    }

    private void OnDestroy()
    {
        JumpSimple.OnGroundLanding -= SummonParticles;
        Player.OnRunning -= ParticlesOnRunning;
        UpdateManager.onJumpParticlesEnd -= SummonRunningParticles;
    }

    private void SummonParticles(bool isGrounded)
    {
        objectFromPool = objectPool.Get();
        if (gameObject == null) { return; }
        objectFromPool.transform.position = transform.position;
        objectFromPool.gameObject.SetActive(true);
    }

    private void ParticlesOnRunning(bool isRunning)
    {
        if (isRunning)
        {
            SummonRunningParticles();
        }
        else if (!isRunning)
        {
            onParticlesBegin?.Invoke(0, true);
        }
    }

    private void SummonRunningParticles()
    {
        SummonParticles(true);
        onParticlesBegin?.Invoke(timeBeforeNextRunParticles, false);
    }
}
