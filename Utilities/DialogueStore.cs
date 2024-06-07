using System.Text.Json;
using System.Text.Json.Nodes;
using QuickType.Dialogue;

namespace LastLaugh.Utilities
{
    internal class DialogueStore
    {
        internal static DialogueStore Instance { get; } = new();

        internal Dictionary<string, DialogueData> Dialogues;

        internal DialogueData GetDialogue(string id)
        {
            return Dialogues[id];
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

        }
    }
}
