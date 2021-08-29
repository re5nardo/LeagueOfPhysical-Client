using UnityEngine;

namespace DebugCommandHandler
{
    public class MyCharacterPositionHandler
    {
        public static void Handle(float x, float y, float z)
        {
            if (Entities.MyCharacter != null)
            {
                Entities.MyCharacter.Position = new Vector3(x, y, z);
            }
        }
    }
}
