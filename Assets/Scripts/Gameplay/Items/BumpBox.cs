using UnityEngine;

public class BumpBox : MonoBehaviour, IBumpable
{
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void BumpSelf(Vector2 force) {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void BumpSelf(Vector2 force, IPlayer player) {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void BumpExplosionSelf(float explosionForce, Vector2 explosionPosition, float explosionRadius) {
        Rigidbody2DExtension.AddExplosionForce(rb, explosionForce, explosionPosition, explosionRadius);
    }
}
