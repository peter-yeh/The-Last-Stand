using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem ps;
    private ObjectPool pool;

    public void Start()
    {
        ps = GetComponent<ParticleSystem>();
        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
    }

    public void Update()
    {
        if (ps && !ps.IsAlive())
        {
            //Destroy(gameObject);
            pool.PutBackInPool(gameObject);

        }

    }
}
