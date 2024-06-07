namespace LastLaugh.Utilities
{
    public class Dialogue
    {
        public string SelectionText { get; set; }
        public List<string> ConvoText { get; set; }
        public Dialogue[] Options { get; set; }
        public List<string> RequiredKeys { get; set; }
        public List<string> CreatedKeys { get; set; }
    }
}
