namespace NiceControls
{
    public class Item
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public Item(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
