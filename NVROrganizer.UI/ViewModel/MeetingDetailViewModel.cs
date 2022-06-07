using System;
using System.Threading.Tasks;
using NvrOrganizer.UI.ViewModel;
using NvrOrganizer.UI.Data.Repositories;
using NvrOrganizer.UI.View.Services;
using NvrOrganizer.UI.Wrapper;
using Prism.Events;
using Prism.Commands;
using NvrOrganizer.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using NvrOrganizer.UI.Event;

namespace NvrOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private IMeetingRepository _meetingRepository;
        private MeetingWrapper _meeting;
        private Nvr _selectedAvailableNvr;
        private Nvr _selectedAddedNvr;
        private List<Nvr> _allNvrs;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService,
          IMeetingRepository meetingRepository) : base(eventAggregator,messageDialogService)
        {
            _meetingRepository = meetingRepository;
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

            AddedNvrs = new ObservableCollection<Nvr>();
            AvailableNvrs = new ObservableCollection<Nvr>();
            AddNvrCommand = new DelegateCommand(OnAddNvrExecute, OnAddNvrCanExecute);
            RemoveNvrCommand = new DelegateCommand(OnRemoveNvrExecute, OnRemoveNvrCanExecute);
        }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            private set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNvrCommand { get; }

        public ICommand RemoveNvrCommand { get; }

        public ObservableCollection<Nvr> AddedNvrs { get; }

        public ObservableCollection<Nvr> AvailableNvrs { get; }

        public Nvr SelectedAvailableNvr
        {
            get { return _selectedAvailableNvr; }
            set
            {
                _selectedAvailableNvr = value;
                OnPropertyChanged();
                ((DelegateCommand)AddNvrCommand).RaiseCanExecuteChanged();
            }
        }

        public Nvr SelectedAddedNvr
        {
            get { return _selectedAddedNvr; }
            set
            {
                _selectedAddedNvr = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveNvrCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int meetingId)
        {
            var meeting = meetingId > 0
              ? await _meetingRepository.GetByIdAsync(meetingId)
              : CreateNewMeeting();

            Id = meetingId;

            InitializeMeeting(meeting);
                    
            _allNvrs = await _meetingRepository.GetAllNvrsAsync();

            SetupPicklist();
        }

        protected override void OnDeleteExecute()
        {
            var result = MessageDialogService.ShowOKCancelDialog($"Do you really want to delete the appointment {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _meetingRepository.Remove(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            Id = Meeting.Id;
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        private void SetupPicklist()
        {
            var meetingNvrIds = Meeting.Model.Nvrs.Select(n => n.Id).ToList();
            var addedNvrs = _allNvrs.Where(n => meetingNvrIds.Contains(n.Id)).OrderBy(n => n.FirstName);
            var availableNvrs = _allNvrs.Except(addedNvrs).OrderBy(n => n.FirstName);

            AddedNvrs.Clear();
            AvailableNvrs.Clear();
            foreach (var addedNvr in addedNvrs)
            {
                AddedNvrs.Add(addedNvr);
            }
            foreach (var availableNvr in availableNvrs)
            {
                AvailableNvrs.Add(availableNvr);
            }
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName == nameof(Meeting.Title)) 
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();


            if (Meeting.Id == 0)
            {
                // Little trick to trigger the validation
                Meeting.Title = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = Meeting.Title;
        }

        private void OnRemoveNvrExecute()
        {
            var nvrToRemove = SelectedAddedNvr;

            Meeting.Model.Nvrs.Remove(nvrToRemove);
            AddedNvrs.Remove(nvrToRemove);
            AvailableNvrs.Add(nvrToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
      
        private bool OnRemoveNvrCanExecute()
        {
            return SelectedAddedNvr != null;
        }

        private bool OnAddNvrCanExecute()
        {
            return SelectedAvailableNvr != null;
        }

        private void OnAddNvrExecute()
        {
            var nvrToAdd = SelectedAvailableNvr;

            Meeting.Model.Nvrs.Add(nvrToAdd);
            AddedNvrs.Add(nvrToAdd);
            AvailableNvrs.Remove(nvrToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if(args.ViewModelName == nameof(NvrDetailViewModel))
            {
                await _meetingRepository.ReloadNvrAsync(args.Id);
                _allNvrs = await _meetingRepository.GetAllNvrsAsync();

                SetupPicklist();
            }
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(NvrDetailViewModel))
            {
               
                _allNvrs = await _meetingRepository.GetAllNvrsAsync();

                SetupPicklist();
            }
        }
    }
}
