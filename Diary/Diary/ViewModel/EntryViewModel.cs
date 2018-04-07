using System;
using System.Windows.Input;
using Diary.Model;
using Xamarin.Forms;

namespace Diary.ViewModel
{
    public class EntryViewModel : BaseViewModel
    {
        private DiaryEntry diaryEntry;
        public DiaryEntry DiaryEntry
        {
            get => diaryEntry;
            set
            {
                diaryEntry = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(DateString));
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Text));
                OnPropertyChanged(nameof(Thoughts));
                OnPropertyChanged(nameof(Tags));
            }
        }

        public DateTime Date
        {
            get => DiaryEntry.Date;
            set
            {
                DiaryEntry.Date = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DateString));
            }
        }
        
        public string DateString => $"Date: {DiaryEntry.Date.ToShortDateString()}";

        public string Title
        {
            get => DiaryEntry.Title;
            set
            {
                DiaryEntry.Title = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get => DiaryEntry.Text;
            set
            {
                DiaryEntry.Text = value;
                OnPropertyChanged();
            }
        }

        public string Thoughts
        {
            get => DiaryEntry.Thoughts;
            set
            {
                DiaryEntry.Thoughts = value;
                OnPropertyChanged();
            }
        }

        public string Tags
        {
            get => DiaryEntry.TagsToString();
            set
            {
                DiaryEntry.SetTags(value);
                OnPropertyChanged();
            }
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand SubmitCommand { get; set; }

        private INavigation navigation;
        private bool isAdding;

        public EntryViewModel(INavigation navigation, DiaryEntry diaryEntry)
        {
            this.navigation = navigation;
            DiaryEntry = diaryEntry;

            if (DiaryEntry == null)
            {
                isAdding = true;
                DiaryEntry = DiaryEntry.NewDefault();
            }

            DeleteCommand = new Command(Delete);
            SubmitCommand = new Command(Submit);
        }

        public void Delete()
        {
            if (isAdding)
            {
                navigation.PopAsync();
            }
            else
            {
                MessagingCenter.Send(this, "DeleteItem", this);
                navigation.PopAsync();
            }
        }

        public void Submit()
        {
            if (isAdding)
            {
                MessagingCenter.Send(this, "AddItem", this);
                navigation.PopAsync();
            }
            else
            {
                MessagingCenter.Send(this, "EditItem", this);
                navigation.PopAsync();
            }
        }
    }
}
