
namespace GameMessage
{
    public struct EntityRegister
    {
        public int entityId;

        public EntityRegister(int entityId)
        {
            this.entityId = entityId;
        }
    }

    public struct EntityUnregister
    {
        public int entityId;

        public EntityUnregister(int entityId)
        {
            this.entityId = entityId;
        }
    }
}
