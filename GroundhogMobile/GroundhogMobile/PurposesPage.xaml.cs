using Core;
using Core.Models;
using GroundhogMobile.Models;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurposesPage : ContentPage
    {
        private PurposeGroup group;

        public PurposesPage(PurposeGroup group)
        {
            InitializeComponent();

            this.group = group;
            lblGroupName.Text = group.Name;

            LoadData();
        }

        private void LoadData()
        {
            List<PurposeViewModel> purposes =
                GroundhogContext.PurposeLogic
                .Read(group.Id)
                .OrderBy(req => req.Text)
                .Select(req => new PurposeViewModel(req))
                .ToList();

            purposesList.ItemsSource = null;
            purposesList.ItemsSource = purposes;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            PurposePage page = new PurposePage(new PurposeViewModel { GroupId = group.Id });
            page.Disappearing += (sender2, e2) =>
            {
                if (page.IsSuccess)
                {
                    Purpose purpose = page.Model.Convert();
                    GroundhogContext.PurposeLogic.Create(purpose);
                    LoadData();
                }
            };

            await Navigation.PushAsync(page);
        }
        public ICommand MenuItemClone =>
           new Command<PurposeViewModel>((purposeModel) =>
           {
               Purpose clone = new Purpose
               {
                   Id = null,
                   GroupId = purposeModel.GroupId,
                   Text = purposeModel.Text,
                   Completed = false
               };

               GroundhogContext.PurposeLogic.Create(clone);
               LoadData();
           });

        public ICommand MenuItemUpdate =>
            new Command<PurposeViewModel>(async (purposeModel) =>
            {
                PurposePage page = new PurposePage(purposeModel);

                page.Disappearing += (sender2, e2) =>
                {
                    if (page.IsSuccess)
                    {
                        Purpose purpose = page.Model.Convert();
                        GroundhogContext.PurposeLogic.Update(purpose);
                        LoadData();
                    }
                };

                await Navigation.PushAsync(page);
            });

        public ICommand MenuItemDelete =>
            new Command<PurposeViewModel>((purposeModel) =>
            {
                GroundhogContext.PurposeLogic.Delete(purposeModel.Id);
                LoadData();
            });

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            PurposeViewModel viewModel = (PurposeViewModel)((CheckBox)sender).BindingContext;
            Purpose model = viewModel.Convert();

            GroundhogContext.PurposeLogic.Update(model);
        }

        private async void purposesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Dictionary<string, ICommand> commands =
                new Dictionary<string, ICommand>
                {
                    { "Изменить", MenuItemUpdate },
                    { "Удалить", MenuItemDelete },
                    { "Дублировать", MenuItemClone }
                };

            CommandPage page = new CommandPage((e.Item as PurposeViewModel).Text, commands.Keys);
            Device.BeginInvokeOnMainThread(async () => await PopupNavigation.Instance.PushAsync(page));

            object obj = await page.Result;
            if (obj != null)
                commands[obj as string].Execute(e.Item as PurposeViewModel);
        }
    }
}