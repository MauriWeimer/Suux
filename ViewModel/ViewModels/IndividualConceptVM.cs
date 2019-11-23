using BusinessLayout.Business;
using Data.Context;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel.ViewModels
{
    public class IndividualConceptVM : ObservableObject
    {
        public IndividualConceptVM()
        {
            IndividualConcepts = new ObservableCollection<Concepts>(allConcepts);
        }

        #region ItemsSource
        private List<Concepts> allConcepts = ConceptB.SelectConcepts();

        public ObservableCollection<Concepts> Concepts { get; set; } = new ObservableCollection<Concepts>();
        public ObservableCollection<Concepts> IndividualConcepts { get; set; }

        private List<Employees> allEmployees = EmployeeB.SelectEmployeesIncludeConcepts();
        public ObservableCollection<Employees> Employees { get; set; } = new ObservableCollection<Employees>();
        #endregion

        #region Properties   
        private int? _fileN;
        public int? FileN
        {
            get => _fileN;
            set
            {
                Set(ref _fileN, value);
                if (!searching)
                if (value != null && Employees.Any(e => e.file_n == value))
                {
                    LoadConcepts();
                }
                else
                {
                    ClearProperties();
                }
            }
        }

        private int? _conceptId;
        public int? ConceptId
        {
            get => _conceptId;
            set => Set(ref _conceptId, value);
        }
        #endregion

        #region Search
        private bool searching;
        public int Filter { get; set; }

        private string _search;
        public string Search
        {
            set
            {
                Set(ref _search, value);
                SearchEmployee();
            }
        }

        private void SearchEmployee()
        {
            searching = true;
            if (string.IsNullOrWhiteSpace(_search))
            {
                allEmployees
                    .Where(e => !Employees.Any(ex => ex.file_n == e.file_n))
                    .ToList()
                    .ForEach(e => Employees.Add(e));
            }
            else
            {
                SearchEmployeeByFilter();
            }

            searching = false;
            if (_fileN == null) FileN = Employees.FirstOrDefault()?.file_n;
        }
        private void SearchEmployeeByFilter()
        {
            switch (Filter)
            {
                case 0:
                    allEmployees
                        .Where(e => Employees.Any(ex => ex.file_n == e.file_n) && e.file_n != int.Parse(_search))
                        .ToList()
                        .ForEach(e => Employees.Remove(e));
                    allEmployees
                        .Where(e => !Employees.Any(ex => ex.file_n == e.file_n) && e.file_n == int.Parse(_search))
                        .ToList()
                        .ForEach(e => Employees.Add(e));
                    break;
                case 1:
                    allEmployees
                        .Where(e => Employees.Any(ex => ex.fullname == e.fullname) && !e.fullname.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(e => Employees.Remove(e));
                    allEmployees
                        .Where(e => !Employees.Any(ex => ex.fullname == e.fullname) && e.fullname.Contains(_search.ToUpper()))
                        .ToList()
                        .ForEach(e => Employees.Add(e));
                    break;
            }
        }
        #endregion

        #region Methods
        private void ClearProperties()
        {
            Concepts.Clear();
            IndividualConcepts.Clear();
        }

        private void MoveSelection(int index, int n)
        {
            FileN = Employees.OrderBy(e => e.file_n).ToList()[(index + n) == Employees.Count ? 0 : (index + n) == -1 ? (Employees.Count - 1) : index + n].file_n;
        }

        private void LoadConcepts()
        {
            Employees employee = Employees.Single(e => e.file_n == _fileN);

            allConcepts
                .Where(c => !employee.Concepts.Any(cx => cx.concept_id == c.concept_id) &&
                IndividualConcepts.Any(cx => cx.concept_id == c.concept_id))
                .ToList()
                .ForEach(c => 
                {
                    IndividualConcepts.Remove(c);
                    if (!Concepts.Any(cx => cx.concept_id == c.concept_id)) Concepts.Add(c);
                });
            allConcepts
                .Where(c => employee.Concepts.Any(cx => cx.concept_id == c.concept_id) &&
                !IndividualConcepts.Any(cx => cx.concept_id == c.concept_id))
                .ToList()
                .ForEach(c =>
                {
                    IndividualConcepts.Add(c);
                    if (Concepts.Any(cx => cx.concept_id == c.concept_id)) Concepts.Remove(c);
                });            
        }
        #endregion

        #region Dialog
        public string TextLoadingDialog => "Actualizando";

        private bool _loadingDialog;
        public bool LoadingDialog
        {
            get => _loadingDialog;
            set => Set(ref _loadingDialog, value);
        }
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(Employees.OrderBy(e => e.file_n).ToList().IndexOf(Employees.Single(e => e.file_n == _fileN)), -1);
                },
                () => { return Employees.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(Employees.OrderBy(e => e.file_n).ToList().IndexOf(Employees.Single(e => e.file_n == _fileN)), 1);
            },
            () => { return Employees.Count > 1; }
            );     

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                Employees employee = Employees.Single(e => e.file_n == _fileN);

                allConcepts
                .Where(c => employee.Concepts.Any(cx => cx.concept_id == c.concept_id) &&
                !IndividualConcepts.Any(cx => cx.concept_id == c.concept_id))
                .ToList()
                .ForEach(c => employee.Concepts.Remove(employee.Concepts.Single(cx => cx.concept_id == c.concept_id)));

                IndividualConcepts
                .Where(c => !employee.Concepts.Any(cx => cx.concept_id == c.concept_id) &&
                IndividualConcepts.Any(cx => cx.concept_id == c.concept_id))
                .ToList()
                .ForEach(c => employee.Concepts.Add(c));

                await Task.Run(() => EmployeeB.UpdateIndividualConcepts(employee));

                LoadingDialog = false;
            }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                LoadConcepts();
            }
            );

        public ICommand MoveAllToConcepts => new RelayCommand
            (
            () =>
            {
                IndividualConcepts
                .Where(c => c.common == false && !Concepts.Any(cx => cx.concept_id == c.concept_id))
                .ToList()
                .ForEach(c => Concepts.Add(c));
                IndividualConcepts
                .Where(c => c.common == false)
                .ToList()
                .ForEach(c => IndividualConcepts.Remove(c));
            },
            () => { return IndividualConcepts.Count != allConcepts.Where(c => c.common == false).ToList().Count; }
            );

        public ICommand MoveToConcepts => new RelayCommand
            (
            () =>
            {
                Concepts concept = IndividualConcepts.Single(c => c.concept_id == (int)_conceptId);

                Concepts.Add(concept);
                IndividualConcepts.Remove(concept);
            },
            () =>
            {
                return IndividualConcepts.Any(c => c.concept_id == (_conceptId ?? 0) && 
                !allConcepts.Any(cx => cx.common && cx.concept_id == _conceptId));
            }
            );

        public ICommand MoveAllToIndividual => new RelayCommand
           (
           () =>
           {
               Concepts
               .Where(c => !IndividualConcepts.Any(cx => cx.concept_id == c.concept_id))
               .ToList()
               .ForEach(c => IndividualConcepts.Add(c));
               Concepts
               .ToList()
               .ForEach(c => Concepts.Remove(c));
           },
           () =>
           {
               return Concepts.Count > 0;
           }
           );

        public ICommand MoveToIndividual => new RelayCommand
            (
            () =>
            {
                Concepts concept = Concepts.Single(c => c.concept_id == (int)_conceptId);

                IndividualConcepts.Add(concept);
                Concepts.Remove(concept);
            },
            () =>
            {
                return Concepts.Any(c => c.concept_id == (_conceptId ?? 0));
            }
            );
        #endregion   
    }
}
