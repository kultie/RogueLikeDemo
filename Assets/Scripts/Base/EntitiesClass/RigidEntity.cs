using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidEntity : Entity
{
    public Rigidbody2D body { private set; get; }
    BoxCollider2D boxCollider;
    public Vector2 velocity
    {
        get;
        private set;
    }

    public Vector2 acceleration
    {
        get;
        private set;
    }

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float maxVelocity;

    [SerializeField]
    float friction;

    PhysicInteractionDelegate onTriggerEnter;
    PhysicInteractionDelegate onTriggerExit;

    public void SetupRigidInfo(float moveSpeed, float maxVelocity, float friction)
    {
        this.moveSpeed = moveSpeed;
        this.maxVelocity = maxVelocity;
        this.friction = friction;
    }

    public void SetupColliderInfo(float colliderSize)
    {
        SetScaleForCollider(colliderSize);
    }

    protected override void Awake()
    {
        base.Awake();
        body = gameObject.AddComponent<Rigidbody2D>();
        SetupBodyInfo();
        boxCollider = gameObject.AddComponent<BoxCollider2D>();
    }

    protected virtual void SetupBodyInfo()
    {
        body.gravityScale = 0;
        body.mass = 0;
        body.angularDrag = 0;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void AddForce(Vector2 force)
    {
        acceleration += force * moveSpeed;
    }

    public void ResetAcceleration()
    {
        acceleration = Vector2.zero;
    }

    public void MoveBody(float dt)
    {
        velocity += acceleration * dt;
        velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
        body.MovePosition(body.position + velocity * dt);
        velocity *= friction;
    }

    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }

    public void SetScaleForCollider(float value)
    {
        boxCollider.size = Vector2.one * value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter?.Invoke(collision.GetComponent<RigidEntity>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onTriggerExit?.Invoke(collision.GetComponent<RigidEntity>());
    }

    public void RegisterPhysicEvent(CharacterControllerBase characterController) {
        onTriggerEnter = characterController.OnTriggerEnter;
        onTriggerExit = characterController.OnTriggerExit;
    }
}
