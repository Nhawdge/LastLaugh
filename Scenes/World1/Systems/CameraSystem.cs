using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;
using LastLaugh.Scenes.World1.Data;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class CameraSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var player = world.QueryFirst<Player>();
            var sprite = player.Get<Sprite>();  

            var camera = LastLaughEngine.Instance.Camera;

            var mousePos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);

            var leftEdge = Raylib.GetRenderWidth() / 2 / camera.Zoom;
            var topEdge = Raylib.GetRenderHeight() / 2 / camera.Zoom;
            
            var rightEdge = Singleton.Instance.CameraConstraints.Width - Raylib.GetScreenWidth() / 2 / camera.Zoom;
            var bottomEdge = Singleton.Instance.CameraConstraints.Height - Raylib.GetScreenHeight() / 2 / camera.Zoom;
            
            var target = sprite.Position;

            target.X = Math.Max(target.X, leftEdge);
            target.Y = Math.Max(target.Y, topEdge);
            target.X = Math.Min(target.X, rightEdge);
            target.Y = Math.Min(target.Y, bottomEdge);

            LastLaughEngine.Instance.Camera.Target = target;

            if (Raylib.IsKeyPressed(KeyboardKey.PageDown))
            {
                LastLaughEngine.Instance.Camera.Zoom = Math.Max(LastLaughEngine.Instance.Camera.Zoom - 0.2f, 0.2f);
                Console.WriteLine(LastLaughEngine.Instance.Camera.Zoom);
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.PageUp))
            {
                LastLaughEngine.Instance.Camera.Zoom = Math.Min(LastLaughEngine.Instance.Camera.Zoom + 0.2f, 10f);
                Console.WriteLine(LastLaughEngine.Instance.Camera.Zoom);
            }
            if (Raylib.IsMouseButtonPressed(MouseButton.Right))
            {
                Console.WriteLine($"Camera: {camera.Target}, Mouse: {mousePos}");
            }

        }
    }
}
