using Arch.Core;
using LastLaugh.Scenes.Components;
using System.Numerics;

namespace LastLaugh.Utilities.Entities
{
    internal static class PlayerUtilities
    {
        public static void BuildPlayer(World world, Vector2 pos)
        {
            var sprite = new Sprite(TextureKey.Units, 1f);
            sprite.Position = pos;
            sprite.BodyType = BodyTypes.Dynamic;
            sprite.Play("player");

            var player = new Player();
            world.Create(sprite, player, new UnitLayer());
        }
    }
}
