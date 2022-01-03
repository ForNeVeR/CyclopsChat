using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using Cyclops.Core;
using Cyclops.Core.Helpers;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace Cyclops.MainApplication.ViewModel
{
    public class VcardViewModel : ViewModelBaseEx
    {
        private readonly ILogger logger;
        private readonly Action closeAction;
        private IUserSession session;
        private VCard sourceVCard = null;

        public VcardViewModel(ILogger logger, Jid target, Action closeAction, bool isEditMode = false)
        {
            this.logger = logger;
            this.closeAction = closeAction;
            IsEditMode = isEditMode;
            session = ChatObjectFactory.GetSession();
            GetVCardInfo(target).NoAwait(logger);
            IsBusy = true;
            Save = new RelayCommand(SaveAction, SaveCanExecute);
            Cancel = new RelayCommand(closeAction);
            LoadPicture = new RelayCommand(LoadPictureAction, () => IsEditMode);
            ClearPicture = new RelayCommand(ClearPictureAction, () => IsEditMode && Photo != null);
        }

        private void ClearPictureAction()
        {
            avatarsChanged = true;
            Photo = null;
        }

        private void LoadPictureAction()
        {
            // Create OpenFileDialog
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif";
            dlg.Multiselect = false;

            // Display OpenFileDialog by calling ShowDialog method
            bool? result = dlg.ShowDialog();
            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                Image img;
                try
                {
                    img = Image.FromFile(dlg.FileName);
                    if (img.Height > 100 || img.Width > 100)
                    {
                        img = img.ResizeImageInProportionIfLarge(100);
                    }
                    Photo = img;
                    avatarsChanged = true;
                }
                catch
                {
                    MessageBox.Show("Invalid image file.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void SaveAction()
        {
            IsBusy = true;
            var vCard = new VCard
            {
                Birthday = Birthday,
                Comments = Comments,
                Email = Email,
                FullName = FullName,
                Photo = Photo?.Clone() as Image,
            };

            DoAction().NoAwait(logger);
            async Task DoAction()
            {
                try
                {
                    await session.UpdateVCard(vCard);
                }
                finally
                {
                    IsBusy = false;
                    closeAction();
                }
            }
        }

        private bool avatarsChanged = false;
        private bool SaveCanExecute()
        {
            return IsEditMode && session.IsAuthenticated && sourceVCard != null &&
                (Birthday != sourceVCard.Birthday ||
                 FullName != sourceVCard.FullName ||
                 avatarsChanged ||
                 Email != sourceVCard.Email ||
                 Comments != sourceVCard.Comments);
        }

        public RelayCommand Save { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand LoadPicture { get; set; }
        public RelayCommand ClearPicture { get; set; }

        private async Task GetVCardInfo(Jid target)
        {
            var obj = await session.GetVCard(target);

            sourceVCard = obj;
            Photo = obj.Photo;
            Nick = obj.Nick;
            FullName = obj.FullName;
            Birthday = obj.Birthday ?? DateTime.MinValue;
            Comments = obj.Comments;
            Email = obj.Email;
            IsBusy = false;
        }

        private Image? photo;
        public Image? Photo
        {
            get { return photo; }
            set
            {
                photo = value;
                RaisePropertyChanged("Photo");
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        private string nick;
        public string Nick
        {
            get { return nick; }
            set
            {
                nick = value;
                RaisePropertyChanged("Nick");
            }
        }

        private bool isEditMode;
        public bool IsEditMode
        {
            get { return isEditMode; }
            set
            {
                isEditMode = value;
                RaisePropertyChanged("IsEditMode");
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        private DateTime birthday;
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                RaisePropertyChanged("Birthday");
            }
        }

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                RaisePropertyChanged("FullName");
            }
        }

        private string comments;
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                RaisePropertyChanged("Comments");
            }
        }
    }
}
