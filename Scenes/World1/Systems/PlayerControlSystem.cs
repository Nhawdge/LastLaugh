using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;
using LastLaugh.Scenes.World1.Data;
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

            //Raylib.DrawRectangleLinesEx(sprite.GetCollider(CollisionDetectors.Bottom), 5, Color.Red);
            //Raylib.DrawRectangleLinesEx(sprite.GetCollider(CollisionDetectors.Top), 5, Color.Red);
            //Raylib.DrawRectangleLinesEx(sprite.GetCollider(CollisionDetectors.Left), 5, Color.Red);
            //Raylib.DrawRectangleLinesEx(sprite.GetCollider(CollisionDetectors.Right), 5, Color.Red);

            if (Singleton.Instance.ActiveDialogue != null)
            {
                return;
            }

            var force = Vector2.Zero;

            var speed = 100f;

            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                force.Y = -1;
            }
            if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                force.Y = 1;
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

            if (force.X > 0)
            {
                sprite.Play("walk-r");
            }
            else if (force.X < 0)
            {
                sprite.Play("walk-l");
            }
            else if (force.Y > 0)
            {
                sprite.Play("walk-d");
            }
            else if (force.Y < 0)
            {
                sprite.Play("walk-u");
            }
            else
            {
                sprite.Play("idle");
            }


            var futurepos = sprite.Position + sprite.Force * Raylib.GetFrameTime();

            if (!Singleton.Instance.CollisionGrid
                .Where(x => x.Value == CollisionType.Solid)
                .Any(grid => Raylib.CheckCollisionCircleRec(futurepos, 1, grid.Key))
                )
            {
                sprite.Position = futurepos;
            }

        }
    }
}
