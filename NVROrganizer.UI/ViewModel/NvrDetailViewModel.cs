using NvrOrganizer.Model;
using NvrOrganizer.UI.Data;
using NvrOrganizer.UI.Event;
using Prism.Events;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using Prism.Commands;
using NvrOrganizer.UI.Wrapper;
using NvrOrganizer.UI.Data.Repositories;
using NvrOrganizer.UI.View.Services;
using NvrOrganizer.UI.Data.Lookups;
using System.Collections.ObjectModel;

namespace NvrOrganizer.UI.ViewModel
{
    public class NvrDetailViewModel : ViewModelBase, INvrDetailViewModel
    {
        private INvrRepository _nvrRepository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;
        private NvrWrapper _nvr;
        private bool _hasChanges;

        public NvrDetailViewModel(INvrRepository nvrRepository, 
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            _nvrRepository = nvrRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;


            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
         }

        public async Task LoadAsync(int? nvrId)
        {
            var nvr = nvrId.HasValue
               ? await _nvrRepository.GetByIdAsync(nvrId.Value)
               : CreateNewNvr();

            InitializeNvr(nvr);

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
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Nvr.Id == 0)
            {
                //Little trick to trigger the validation
                Nvr.FirstName = "";
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = "-"});
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

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand SaveCommand { get; }

        public ICommand DeleteCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        private async void OnSaveExecute()
        {
          await _nvrRepository.SaveAsync();
            HasChanges = _nvrRepository.HasChanges();
            _eventAggregator.GetEvent<AfterNvrSavedEvent>().Publish(
                new AfterNvrSavedEventArgs
                {
                    Id = Nvr.Id,
                    DisplayMember = $"{Nvr.FirstName} {Nvr.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            return Nvr != null && !Nvr.HasErrors && HasChanges;
        }

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOKCancelDialog($"Do you really want to delete the selected nvr {Nvr.FirstName} {Nvr.LastName}?",
               "Question");
            if(result == MessageDialogResult.OK) { 
            _nvrRepository.Remove(Nvr.Model);
           await _nvrRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterNvrDeleteEvent>().Publish(Nvr.Id);
        }
        }
        private Nvr CreateNewNvr()
        {
           var nvr = new Nvr();
            _nvrRepository.Add(nvr);
            return nvr;
        }

    }
}
