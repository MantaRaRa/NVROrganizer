using NvrOrganizer.Model;
using NvrOrganizer.UI.Event;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using NvrOrganizer.UI.Wrapper;
using NvrOrganizer.UI.Data.Repositories;
using NvrOrganizer.UI.View.Services;
using NvrOrganizer.UI.Data.Lookups;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NvrOrganizer.UI.ViewModel;
using System;

namespace NvrOrganizer.UI.ViewModel
{
    public class NvrDetailViewModel : DetailViewModelBase, INvrDetailViewModel
    {
        private INvrRepository _nvrRepository;
        
        private NvrWrapper _nvr;
        private NvrPhoneNumberWrapper _selectedPhoneNumber;
        private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;
       

        public NvrDetailViewModel(INvrRepository nvrRepository, 
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
            :base(eventAggregator,messageDialogService)
        {
            _nvrRepository = nvrRepository;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            eventAggregator.GetEvent<AfterCollectionSavedEvent>()
            .Subscribe(AfterCollectionSaved);

            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);


            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<NvrPhoneNumberWrapper>();
         }

        public override async Task LoadAsync(int nvrId)
        {
            var nvr = nvrId > 0
               ? await _nvrRepository.GetByIdAsync(nvrId)
               : CreateNewNvr();

            Id = nvrId;

            InitializeNvr(nvr);

            InitializeNvrPhoneNumbers(nvr.PhoneNumbers);

            await LoadProgrammingLanguagesLookupAsync();

        }

        private void InitializeNvr(Nvr nvr)
        {
            Nvr = new NvrWrapper(nvr);
            Nvr.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _nvrRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Nvr.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(Nvr.FirstName)
                    || e.PropertyName == nameof(Nvr.LastName))
                    {
                    SetTitle();
                    }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Nvr.Id == 0)
            {
                //Little trick to trigger the validation
                Nvr.FirstName = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{Nvr.FirstName} {Nvr.LastName}";
        }

        private void InitializeNvrPhoneNumbers(ICollection<NvrPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= NvrPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var nvrPhoneNumber in phoneNumbers)
            {
                var wrapper = new NvrPhoneNumberWrapper(nvrPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += NvrPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void NvrPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges= _nvrRepository.HasChanges();
            }
            if (e.PropertyName == nameof(NvrPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = " - "});
            var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        public NvrWrapper Nvr
        {
            get { return _nvr; }
            private set
            {
                _nvr = value;
                OnPropertyChanged();
            }

        }

        public NvrPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddPhoneNumberCommand { get; }

        public ICommand RemovePhoneNumberCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        public ObservableCollection<NvrPhoneNumberWrapper> PhoneNumbers { get; }

        protected override async void OnSaveExecute()
        {
          await _nvrRepository.SaveAsync();
            HasChanges = _nvrRepository.HasChanges();
            Id = Nvr.Id;
            RaiseDetailSavedEvent(Nvr.Id, $"{Nvr.FirstName} {Nvr.LastName}");
           
        }

        protected override bool OnSaveCanExecute()
        {
            return Nvr != null 
                && !Nvr.HasErrors 
                && PhoneNumbers.All(pn => !pn.HasErrors)
                && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            if (await _nvrRepository.HasMeetingsAsync(Nvr.Id))
            {
                MessageDialogService.ShowInfoDialog($"{Nvr.FirstName} {Nvr.LastName} can't be deleted, as this NVR is part of at least one meeting");
                return;
            }

            var result = MessageDialogService.ShowOKCancelDialog($"Do you really want to delete the selected NVR {Nvr.FirstName} {Nvr.LastName}?",
               "Question");
            if(result == MessageDialogResult.OK)
            { 
            _nvrRepository.Remove(Nvr.Model);
           await _nvrRepository.SaveAsync();
                RaiseDetailDeletedEvent(Nvr.Id);
           
        }
        }

        private void OnAddPhoneNumberExecute()
        {
           var newNumber = new NvrPhoneNumberWrapper(new NvrPhoneNumber());
            newNumber.PropertyChanged += NvrPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Nvr.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; //Trigger validation
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= NvrPhoneNumberWrapper_PropertyChanged;
            _nvrRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _nvrRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }
        private Nvr CreateNewNvr()
        {
           var nvr = new Nvr();
            _nvrRepository.Add(nvr);
            return nvr;
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ProgrammingLanguageDetailViewModel))
            {
                await LoadProgrammingLanguagesLookupAsync();
            }
        }

    }
}
