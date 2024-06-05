using Arch.Core;
using Arch.Core.Extensions;
using LastLaugh;
using LastLaugh.Scenes.Components;
using LastLaugh.Scenes.World1.Data;
using LastLaugh.Utilities;
using System.Numerics;

namespace LastLaugh.Scenes.World1.Systems
{
    internal class MovementForceSystem : GameSystem
    {
        internal override void Update(World world)
        {
            var query = new QueryDescription().WithAll<Sprite>();
            var grid = Singleton.Instance.CollisionGrid;

            world.Query(in query, (entity) =>
            {
                var sprite = entity.Get<Sprite>();
                if (sprite.BodyType == BodyTypes.Static)
                {
                    return;
                }

                var damper = 0.9f;
                sprite.Acceleration *= damper;
                sprite.Acceleration += sprite.Force;

                var futurePos = sprite.Position + sprite.Acceleration * Raylib.GetFrameTime();

                sprite.IsGrounded = grid.Any(gridItem => Raylib.CheckCollisionRecs(sprite.GetCollider(CollisionDetectors.Bottom), gridItem.Key));

                if (!sprite.IsGrounded)
                {
                    sprite.Acceleration.Y += 100;
                }
                else
                {
                    sprite.Acceleration.Y = Math.Min(sprite.Acceleration.Y, 0);
                    futurePos.Y = Math.Min(sprite.Position.Y, futurePos.Y);
                }

                var hitCeiling = grid.Any(gridItem => Raylib.CheckCollisionRecs(sprite.GetCollider(CollisionDetectors.Top), gridItem.Key));

                if (hitCeiling)
                {
                    sprite.Acceleration.Y = Math.Max(sprite.Acceleration.Y, 0);
                    futurePos.Y = Math.Max(sprite.Position.Y, futurePos.Y);
                }
                var hitLeft = grid.Any(gridItem => Raylib.CheckCollisionRecs(sprite.GetCollider(CollisionDetectors.Left), gridItem.Key));
                if (hitLeft)
                {
                    sprite.Acceleration.X = Math.Max(sprite.Acceleration.X, 0);
                    futurePos.X = Math.Max(sprite.Position.X, futurePos.X);
                }
                var hitright = grid.Any(gridItem => Raylib.CheckCollisionRecs(sprite.GetCollider(CollisionDetectors.Right), gridItem.Key));

                if (hitright)
                {
                    sprite.Acceleration.X = Math.Min(sprite.Acceleration.X, 0);
                    futurePos.X = Math.Min(sprite.Position.X, futurePos.X);
                }

                if (hitright && hitLeft)
                {
                    //futurePos.Y -= 1;
                }
                sprite.Position = futurePos;

            });
        }
    }
}
