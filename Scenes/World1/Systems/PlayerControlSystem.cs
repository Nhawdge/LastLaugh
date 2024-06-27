using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh.Extensions;
using LastLaugh.Scenes.Components;
using LastLaugh.Scenes.World1.Data;
using System.Numerics;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class PlayerControlSystem : GameSystem
    {
        private Direction LastDirection = Direction.Down;
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
                LastDirection = Direction.Right;
            }
            else if (force.X < 0)
            {
                sprite.Play("walk-l");
                LastDirection = Direction.Left;
            }
            else if (force.Y > 0)
            {   
                sprite.Play("walk-d");
                LastDirection = Direction.Down;
            }
            else if (force.Y < 0)
            {
                sprite.Play("walk-u");
                LastDirection = Direction.Up;
            }
            else
            {
                var idleName = LastDirection switch
                {
                    Direction.Up => "idle-u",
                    Direction.Down => "idle-d",
                    Direction.Left => "idle-l",
                    Direction.Right => "idle-r",
                    _ => "idle"
                };
                sprite.Play(idleName);
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
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

}
