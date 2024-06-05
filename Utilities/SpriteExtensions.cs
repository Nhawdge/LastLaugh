using LastLaugh.Scenes.Components;
using System.Numerics;

namespace LastLaugh.Utilities
{
    internal static class SpriteExtensions
    {
        public static Rectangle GetCollider(this Sprite sprite, CollisionDetectors side)
        {
            if (sprite == null)
                return new Rectangle();
            var result = new Rectangle(0, 0, 5, 5);
            var padding = 5;

            var hitBox = sprite.CollisionDestination;


            switch (side)
            {
                case CollisionDetectors.Top:
                    result.X = hitBox.X;
                    result.Y = hitBox.Y;
                    result.Width = hitBox.Width;
                    result.Height = padding;
                    break;
                case CollisionDetectors.Bottom:
                    result.X = hitBox.X;
                    result.Y = hitBox.Y + hitBox.Height - padding;
                    result.Width = hitBox.Width;
                    result.Height = padding;
                    break;
                case CollisionDetectors.Left:
                    result.X = hitBox.X - padding;
                    result.Y = hitBox.Y;
                    result.Width = padding;
                    result.Height = hitBox.Height - padding;
                    break;
                case CollisionDetectors.Right:
                    result.X = hitBox.X + hitBox.Width;
                    result.Y = hitBox.Y;
                    result.Width = padding;
                    result.Height = hitBox.Height - padding;
                    break;
                case CollisionDetectors.Center:
                    result = hitBox;
                    break;
            }

            //switch (offset)
            //{
            //    case CollisionDetectors.Top:
            //        result.Y = hitBox.Y + hitBox.Height / 4;
            //        break;
            //    case CollisionDetectors.Bottom:
            //        result.Y = hitBox.Y + hitBox.Height / 4 * 3;
            //        break;
            //    case CollisionDetectors.Left:
            //        result.X = hitBox.X + hitBox.Width / 4;
            //        break;
            //    case CollisionDetectors.Right:
            //        result.X = hitBox.X + hitBox.Width / 4 * 3;
            //        break;
            //    case CollisionDetectors.Center:
            //        if (side is CollisionDetectors.Top or CollisionDetectors.Bottom)
            //            result.X = hitBox.X + hitBox.Width / 2;
            //        else
            //            result.Y = hitBox.Y + hitBox.Height / 2;
            //        break;
            //}

            return result;
        }

    }
    public enum CollisionDetectors
    {
        Top,
        Bottom,
        Left,
        Right,
        Center,
    }
    public enum BodyTypes
    {
        Static,
        Dynamic
    }
}
