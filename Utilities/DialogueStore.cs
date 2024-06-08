using QuickType.Dialogue;

namespace LastLaugh.Utilities
{
    internal class DialogueStore
    {
        internal static DialogueStore Instance { get; } = new();
        internal Dictionary<string, TextureKey> PortraitKeyMap = new();

        internal Dictionary<string, DialogueData> Dialogues;

        private DialogueStore()
        {
            Dialogues = new();
            var allFiles = Directory.GetFiles("Assets/Content/Dialogues", "*.json");
            foreach (var file in allFiles)
            {
                var dataText = File.ReadAllText(file);
                var dialogues = DialogueData.FromJson(dataText).ToDictionary(x => x.Key, x => x);
                foreach (var dialogue in dialogues)
                {
                    Dialogues.Add(dialogue.Key, dialogue.Value);
                }
            }

            // These tie to speaker name in dialogue json files
            PortraitKeyMap.Add("jane foole", TextureKey.PortraitPlayer);
            PortraitKeyMap.Add("king otto", TextureKey.PortraitKing);
            PortraitKeyMap.Add("random guard", TextureKey.PortraitGuard1);
        }

        internal static TextureKey GetPortrait(string key)
        {
            if (DialogueStore.Instance.PortraitKeyMap.TryGetValue(key.ToLower(), out var texture))
            {
                return texture;
            }
            return TextureKey.Empty;
        }

        internal static DialogueData GetDialogue(string id)
        {
            return DialogueStore.Instance.Dialogues[id];
        }

        private List<string> UnlockedKeys = new();

        internal void AddUnlockedKey(List<string> keys)
        {
            foreach (var key in keys)
                if (!UnlockedKeys.Contains(key))
                {
                    UnlockedKeys.Add(key);
                }
        }

        internal bool HasRequiredKeys(List<string> keys)
        {
            return keys
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                .All(x => UnlockedKeys.Contains(x));
        }


    }
}
