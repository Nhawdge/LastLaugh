using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;
using LastLaugh.Utilities;
using System.Numerics;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class PlayerControlSystem : GameSystem
    {
        private float jumpClock = 0;
        internal override void Update(World world)
        {
            var playerEntity = world.QueryFirst<Player>();

            var player = playerEntity.Get<Player>();
            var sprite = playerEntity.Get<Sprite>();

            Raylib.DrawRectangleLinesEx(sprite.GetCollider(CollisionDetectors.Bottom), 5, Color.Red);
            Raylib.DrawRectangleLinesEx(sprite.GetCollider(CollisionDetectors.Top), 5, Color.Red);
            Raylib.DrawRectangleLinesEx(sprite.GetCollider(CollisionDetectors.Left), 5, Color.Red);
            Raylib.DrawRectangleLinesEx(sprite.GetCollider(CollisionDetectors.Right), 5, Color.Red);


            var force = Vector2.Zero;

            var speed = 50f;
            var jumpStrength = 200;
            if (sprite.IsGrounded)
            {
                jumpClock = 0;
            }
            else
            {
                jumpClock += Raylib.GetFrameTime();
            }
            var canJump = sprite.IsGrounded || jumpClock < 0.25f;

            if (canJump && Raylib.IsKeyDown(KeyboardKey.Space))
            {
                force.Y = -jumpStrength * Raylib.GetFrameTime();
            }
            if (Raylib.IsKeyDown(KeyboardKey.S))
            {
            }
            if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                force.X = -1;
            }
            if (Raylib.IsKeyDown(KeyboardKey.D))
            {
                force.X = 1;
            }
            sprite.Force = force * speed;
        }
    }
}
