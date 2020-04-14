using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidEntity : Entity
{
    Rigidbody2D body;
    BoxCollider2D boxCollider;
    public Vector2 velocity {
        get;
        private set;
    }

    public Vector2 acceleration {
        get;
        private set;
    }

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float maxVelocity;

    [SerializeField]
    float friction;

    public void Setup(float moveSpeed, float maxVelocity, float friction) {
        this.moveSpeed = moveSpeed;
        this.maxVelocity = maxVelocity;
        this.friction = friction;
    }

    protected override void Awake()
    {
        base.Awake();
        body = gameObject.AddComponent<Rigidbody2D>();
        SetupBodyInfo();
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
    }

    private void SetupBodyInfo() {
        body.gravityScale = 0;
        body.mass = 0;
        body.angularDrag = 0;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void AddForce(Vector2 force) {
        acceleration += force * moveSpeed;
    }

    public override void ManualFixedUpdate(float dt)
    {
        velocity += acceleration;
        velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
        body.MovePosition(body.position + velocity * dt);
        acceleration = Vector2.zero;
        velocity *= friction;
    }

    public void SetMoveSpeed(float value) {
        moveSpeed = value;
    }
}
