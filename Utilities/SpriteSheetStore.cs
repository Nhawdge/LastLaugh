using Raylib_cs;
using System.Xml.Linq;

namespace LastLaugh.Utilities
{
    internal class SpriteSheetStore
    {
        internal static SpriteSheetStore Instance { get; } = new();
        internal Dictionary<string, Rectangle> SpriteStore { get; set; } = new();
        internal Dictionary<string, Texture2D> TextureCache { get; set; } = new();

        private SpriteSheetStore()
        {
            LoadSprites();
        }

        private void LoadSprites()
        {
        }

        internal Rectangle GetSpriteSheetSource(string key)
        {
            if (SpriteStore.Count <= 0)
            {
                LoadSprites();
            }
            return SpriteStore[key.ToString()];
        }
    }

    internal enum SpriteKey
    {
        None,
    }
}
