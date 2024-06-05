using Raylib_cs;

namespace LastLaugh.Utilities
{
    internal class TextureManager
    {
        internal static TextureManager Instance { get; } = new();

        internal Dictionary<TextureKey, Texture2D> TextureStore { get; set; } = new();
        internal Dictionary<TextureKey, string> FilePathMapping { get; set; } = new();
        internal Dictionary<TextureKey, NPatchInfo> NPatchInfos { get; set; } = new();


        internal Dictionary<string, Texture2D> TextureCache { get; set; } = new();

        private TextureManager()
        {
            MapKeys();
            LoadTextures();
        }

        private void MapKeys()
        {
            FilePathMapping.Add(TextureKey.Empty, "");
            FilePathMapping.Add(TextureKey.RaylibLogo, "Assets/raylib_logo_animation");
            FilePathMapping.Add(TextureKey.Player, "Assets/Art/Player");
            FilePathMapping.Add(TextureKey.Tiles, "Assets/Art/Tiles");
            FilePathMapping.Add(TextureKey.ScifiTiles, "Assets/Art/sci-fi-tileset");
        }

        private void LoadTextures()
        {
            foreach (var mapping in FilePathMapping)
            {
                LoadKey(mapping.Key);
            }

            var mostPatchInfos = new NPatchInfo
            {
                Left = 10,
                Top = 10,
                Right = 10,
                Bottom = 10,
                Layout = NPatchLayout.NinePatch
            };

            //            NPatchInfos.Add(TextureKey.BlueBox, mostPatchInfos);

        }

        private void LoadKey(TextureKey key)
        {
            TextureStore.Add(key, Raylib.LoadTexture($"{FilePathMapping[key]}.png"));
        }

        internal Texture2D GetTexture(TextureKey key)
        {
            if (TextureStore.Count <= 0)
            {
                LoadTextures();
            }
            return TextureStore[key];
        }
        internal Texture2D? GetCachedTexture(string key)
        {
            if (TextureCache.TryGetValue(key, out var texture))
                return texture;
            return null;
        }
    }

    internal enum TextureKey
    {
        Empty,
        RaylibLogo,
        Player,
        Tiles,
        ScifiTiles,
    }
}
