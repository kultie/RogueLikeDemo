public interface ICollision
{
    void EnterCollision(Entity e);
    void ExitCollision(Entity e);
}

public interface ITriggerCollision {
    void EnterTriggerCollision(Entity e);
    void ExitTriggerCollision(Entity e);
}