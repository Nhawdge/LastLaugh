using LastLaugh.Scenes.Components;
using Raylib_cs;

namespace LastLaugh.Scenes.World1.Data
{
    internal class Singleton
    {
        private Singleton() { }
        internal static Singleton Instance = new();
        internal Dictionary<Rectangle, CollisionType> CollisionGrid = new();
        internal Rectangle CameraConstraints;
    }
}
