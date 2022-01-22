using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Axe : Weapon
{
    private Collider2D hitBox;
    private Quaternion originalRotation;

    private enum State {
        Normal,
        Motion
    }
    private State state;

    protected override void Start()
    {
        base.Start();
        hitBox = GetComponentInChildren<Collider2D>();
        originalRotation = transform.localRotation;
    }

    void Update() {
        switch (state) {
        case State.Motion:
            HandleMotion();
            break;
        }
    }

    public override bool Use()
    {
        if (TryMeleeAttack(hitBox)) {
            PlayUseMotion();
            base.PlayUseSound();
            return base.Use();
        } else {
            return false;
        }
    }

    protected override void PlayUseMotion()
    {
        // base.PlayUseMotion();

        transform.localRotation = originalRotation;
        transform.Rotate(0, 0, -70f);
        state = State.Motion;
    }

    private void HandleMotion() {
        // Rotate back to original rotation
        if (transform.localRotation.z >= originalRotation.z) {
            transform.localRotation = originalRotation;
            state = State.Normal;
        } else {
            transform.localRotation = 
                Quaternion.Lerp(transform.localRotation, originalRotation, 5 * Time.deltaTime);
        }
    }

    protected override void AllUsed()
    {
        // Do Something

        base.AllUsed();
    }
}
