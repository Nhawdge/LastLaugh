using System.Numerics;

namespace LastLaugh.Scenes.Components
{
    internal class Doorway
    {
        internal Vector2 Destination;
        internal string LevelId = string.Empty;
        internal Guid TargetEntityId;
        private Vector2 doorAt;

        public Doorway(Vector2 doorAt)
        {
            this.doorAt = doorAt;
        }
    }
}
