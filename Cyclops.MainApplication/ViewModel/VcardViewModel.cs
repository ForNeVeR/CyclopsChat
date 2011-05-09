using System;
using System.Drawing;
using System.Windows;
using Cyclops.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace Cyclops.MainApplication.ViewModel
{
    public class VcardViewModel : ViewModelBase
    {
        private readonly Action closeAction;
        private IUserSession session;
        private Vcard sourceVcard = null;
        
        public VcardViewModel(IEntityIdentifier target, Action closeAction, bool isEditMode = false)
        {
            this.closeAction = closeAction;
            IsEditMode = isEditMode;
            session = ChatObjectFactory.GetSession();
            session.RequestVcard(target, OnReceived);
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
            session.UpdateVcard(new Vcard
                                    {
                                        Birthday = Birthday,
                                        Comments = Comments,
                                        Email = Email,
                                        FullName = FullName,
                                        Photo = Photo == null ? null : (Photo.Clone() as Image),
                                    }, SaveCompleted);
        }

        private void SaveCompleted(bool obj)
        {
            IsBusy = false;
            closeAction();
        }

        private bool avatarsChanged = false;
        private bool SaveCanExecute()
        {
            return IsEditMode && session.IsAuthenticated && sourceVcard != null &&
                (Birthday != sourceVcard.Birthday ||
                 FullName != sourceVcard.FullName ||
                 avatarsChanged ||
                 Email != sourceVcard.Email ||
                 Comments != sourceVcard.Comments);
        }

        public RelayCommand Save { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand LoadPicture { get; set; }
        public RelayCommand ClearPicture { get; set; }

        private void OnReceived(Vcard obj)
        {
            sourceVcard = obj;
            Photo = obj.Photo;
            Nick = obj.Nick;
            FullName = obj.FullName;
            Birthday = obj.Birthday;
            Comments = obj.Comments;
            Email = obj.Email;
            IsBusy = false;
        }

        private Image photo;
        public Image Photo
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
