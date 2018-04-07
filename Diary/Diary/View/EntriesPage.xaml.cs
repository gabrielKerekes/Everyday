using System;
using Diary.Model;
using Diary.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diary.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntriesPage : ContentPage
	{
		public EntriesPage()
		{
			InitializeComponent();

		    BindingContext = new EntriesViewModel();
		}

	    private async void Button_OnClicked(object sender, EventArgs e)
	    {
	        await Navigation.PushAsync(new EntryPage());
	    }

	    private async void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
	    {
            // todo: treba asi spravit view model/naviguj sa na edit...
            await Navigation.PushAsync(new EntryPage(((EntryViewModel)e.SelectedItem).DiaryEntry));
        }
	}
}