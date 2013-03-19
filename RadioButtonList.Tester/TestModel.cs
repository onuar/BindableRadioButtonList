namespace RadioButtonList.Tester
{
    public class TestModel
    {
        public int Id { get; set; }

        public TestEnum TestEnum { get; set; }
        public string TestText { get; set; }

        public override string ToString()
        {
            return string.Format("Text: {0} Value:{1}", TestText, TestEnum.ToString());
        }
    }

    public enum TestEnum
    {
        FirstItem = 1,
        SecondItem = 2
    }
}
