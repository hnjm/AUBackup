﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using MVVMAUbackup.Models;
using MVVMAUbackup.Commands;
namespace MVVMAUbackup.ViewModels
{
    class StatusViewModel : ViewModelBase
    {
        
        #region Constructor
        public StatusViewModel()
        {
            _statuses = new ObservableCollection<StatusModel>()
            {
                new StatusModel{
                    ElapsedTime = FolderViewModel.BackupTimer.Interval,
                    DateFinished = null,
                    BackupStatus = nameof(BackupProgress.NotStarted) 
                }
            };

            _updateProgress = new DispatcherTimer();
            _updateProgress.Interval = TimeSpan.FromSeconds(1);
            _updateProgress.Tick += UpdateElapsedTime;
        }
        #endregion

        #region Fields
        private ObservableCollection<StatusModel> _statuses;
        private DispatcherTimer _updateProgress;
        #endregion

        
        #region Properties
        public ObservableCollection<StatusModel> Statuses => _statuses;
        public DispatcherTimer UpdateProgress => _updateProgress;
        #endregion

        #region Methods
        public void AddStatus()
        {
            _statuses.Add(
                 new StatusModel
                 {
                     ElapsedTime = FolderViewModel.BackupTimer.Interval,
                     DateFinished = null,
                     BackupStatus = nameof(BackupProgress.InProgress)
                 });
        }
        private void UpdateElapsedTime(object sender, EventArgs e)
        {
            var CurrentStatus = _statuses.Last();
            if(CurrentStatus.ElapsedTime != TimeSpan.FromSeconds(0))
            {
                CurrentStatus.ElapsedTime -= TimeSpan.FromSeconds(1);
                return;
            }             
            UpdateFinishedProcess();      
        }

        public void PauseProcess()
        {
            var CurrentStatus = _statuses.Last();
            CurrentStatus.BackupStatus = nameof(BackupProgress.Paused);
            _updateProgress.Stop();
        }
        public void StartProcess()
        {
            var CurrentStatus = _statuses.Last();
            CurrentStatus.BackupStatus = nameof(BackupProgress.InProgress);
            _updateProgress.Start();
        }

        private void UpdateFinishedProcess()
        {
            var CurrentStatus = _statuses.Last();
            CurrentStatus.DateFinished = DateTime.Now;
            CurrentStatus.BackupStatus = nameof(BackupProgress.Finished);
            AddStatus();
        }
        #endregion
    }
}