using System.ComponentModel;

namespace NiceControls.Tester
{
    public class TestModel : INotifyPropertyChanged
    {
        private TestEnum _testEnum;
        private string _testText;
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public TestEnum TestEnum
        {
            get { return _testEnum; }
            set
            {
                _testEnum = value;
                OnPropertyChanged("TestEnum");
            }
        }

        public string TestText
        {
            get { return _testText; }
            set
            {
                _testText = value;
                OnPropertyChanged("TestText");
            }
        }

        public override string ToString()
        {
            return string.Format("Text: {0} Value:{1}", TestText, TestEnum.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum TestEnum
    {
        FirstItem = 666,
        SecondItem = 777
    }
}
