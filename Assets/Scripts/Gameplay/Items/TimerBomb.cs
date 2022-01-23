using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TimerBomb : MonoBehaviourPunCallbacks
{
    private float power;
    private float explosionRadius;
    private float delayTime;
    private float igniteTime;
    private IPlayer thrower;
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    public LayerMask isLayer;

    private void Start() {
        
    }

    public void InitBomb(float power, float explosionRadius, float delayTimeInSec, IPlayer thrower, Vector2 direction) {
        this.power = power;
        this.explosionRadius = explosionRadius;
        this.delayTime = delayTimeInSec;
        this.thrower = thrower;
        igniteTime = Time.time;
        Invoke("DetonateBomb", delayTime);
        // ** Fatal ** Physics2D seems not workin in thread...
        /*int delayTime = Mathf.FloorToInt(delayTimeInSec * 1000);
        Task.Run( async () => {
            await Task.Delay(delayTime);
            DetonateBomb();
        });*/

        GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
    }

    private void DetonateBomb() {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D target in hitTargets) {
            if (target.TryGetComponent<IBumpable>(out IBumpable bumpTarget)) {
                // Power decreases along the distance
                bumpTarget.BumpSelf(ExplosionPower(
                    power, 
                    Vector2.Distance(transform.position, target.transform.position),
                    target.transform.position - transform.position), thrower);
                
                // Rigidbody2DExtension.AddExplosionForce(targetRb, power, transform.position, explosionRadius);
            }
        }

        Destroy(this.gameObject);
        // PhotonNetwork.Destroy(gameObject);
        // if(photonView.IsMine) { 
		// 	PhotonNetwork.Destroy(gameObject);
		// }
    }

    private Vector2 ExplosionPower(float power, float distance, Vector2 vector) {
        float expPower = power * Sigmoid(distance);
        return vector.normalized * expPower;
    }

    private float Sigmoid(float x) {
        // range: 1 ~ 0
        return 1.5f - Mathf.Exp(x) / (Mathf.Exp(x) + 1);
    }
}
