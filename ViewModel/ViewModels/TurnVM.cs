using BusinessLayout.Business;
using Data.Context;
using Data.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class TurnVM : ObservableObject
    {
        public TurnVM()
        {
            TurnId = Turns.FirstOrDefault()?.turn_id;
        }

        #region ItemsSource
        public List<Colors> Colors => ColorB.GetColors();

        public ObservableCollection<Turns> Turns { get; set; } = new ObservableCollection<Turns>(TurnB.SelectTurns());
        #endregion

        #region Properties   
        private int? _turnId;
        public int? TurnId
        {
            get => _turnId;
            set
            {
                Set(ref _turnId, value);
                VerifyIsValidTurnId();
            }
        }

        private TimeSpan? _morning_d;
        public TimeSpan? Morning_d
        {
            get => _morning_d;
            set => Set(ref _morning_d, value);
        }

        private TimeSpan? _morning_h;
        public TimeSpan? Morning_h
        {
            get => _morning_h;
            set => Set(ref _morning_h, value);
        }

        private TimeSpan? _late_d;
        public TimeSpan? Late_d
        {
            get => _late_d;
            set => Set(ref _late_d, value);
        }

        private TimeSpan? _late_h;
        public TimeSpan? Late_h
        {
            get => _late_h;
            set => Set(ref _late_h, value);
        }

        private string _color;
        public string Color
        {
            get { return _color; }
            set { Set(ref _color, value); }
        }
        #endregion

        #region Methods
        private void ClearProperties()
        {
            Morning_d = null;
            Morning_h = null;
            Late_d = null;
            Late_h = null;
            Color = null;
        }

        private Turns GetTurn()
        {
            return new Turns
            {
                turn_id = _turnId ?? 0,
                morning_d = _morning_d,
                morning_h = _morning_h,
                late_d = _late_d,
                late_h = _late_h,
                color = Color
            };
        }

        private void MoveSelection(int index, int n)
        {
            TurnId = Turns[(index + n) == Turns.Count ? 0 : (index + n) == -1 ? (Turns.Count - 1) : index + n].turn_id;
        }

        private void VerifyIsValidTurnId()
        {
            if (_turnId == null || !Turns.Any(t => t.turn_id == _turnId))
            {
                ClearProperties();
            }
            else
            {
                LoadTurn();
            }
        }

        private void LoadTurn()
        {
            Turns turns = Turns.Single(t => t.turn_id == _turnId);

            Morning_d = turns.morning_d;
            Morning_h = turns.morning_h;
            Late_d = turns.late_d;
            Late_h = turns.late_h;
            Color = turns.color;
        }
        #endregion

        #region Dialog
        private string _textLoadingDialog;
        public string TextLoadingDialog
        {
            get => _textLoadingDialog;
            set => Set(ref _textLoadingDialog, value);
        }

        private bool _loadingDialog;
        public bool LoadingDialog
        {
            get => _loadingDialog;
            set => Set(ref _loadingDialog, value);
        }

        private bool _confirmDialog;
        public bool ConfirmDialog
        {
            get => _confirmDialog;
            set => Set(ref _confirmDialog, value);
        }

        public string ConfirmDialogText => "¿Desea eliminar el turno?";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return _turnId != null; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    await Task.Run(() => TurnB.DeleteTurn((int)_turnId));
                    Turns.Remove(Turns.Single(t => t.turn_id == _turnId));

                    LoadingDialog = false;
                });
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(Turns.IndexOf(Turns.Single(t => t.turn_id == _turnId)), -1);
                },
                () => { return Turns.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(Turns.IndexOf(Turns.Single(t => t.turn_id == _turnId)), 1);
            },
            () => { return Turns.Count > 1; }
            );

        public ICommand New => new RelayCommand
                (
                () =>
                {
                    TextLoadingDialog = "Agregando";
                }
                );

        public ICommand Update => new RelayCommand
                (
                () =>
                {
                    TextLoadingDialog = "Actualizando";
                },
                () => { return _turnId != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                Turns turn = GetTurn();

                if (turn.turn_id == 0)
                {
                    await Task.Run(() => TurnB.InsertTurn(turn));
                    Turns.Add(turn);
                }
                else
                {
                    await Task.Run(() => TurnB.UpdateTurn(turn));
                    Turns[Turns.IndexOf(Turns.Single(t => t.turn_id == turn.turn_id))] = turn;
                }

                TurnId = turn.turn_id;
                LoadingDialog = false;
            },
            () => { return !string.IsNullOrEmpty(_color) && !(_morning_d == null && _morning_h == null && _late_d == null && _late_h == null); }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                VerifyIsValidTurnId();
            }
            );
        #endregion    
    }
}
