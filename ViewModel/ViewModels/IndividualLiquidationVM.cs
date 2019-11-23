using BusinessLayout.Business;
using BusinessLayout.Helper;
using Data.Context;
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
    public class IndividualLiquidationVM : ObservableObject
    {
        public IndividualLiquidationVM()
        {
            foreach (Employees e in Employees)
            {
                foreach (Concepts c in e.Concepts)
                {
                    if (c.concept.Contains("[OBR]"))
                    {
                        c.concept = c.concept.Replace("[OBR]", e.Social_works.social_work);
                    }
                    else if (c.concept.Contains("[SIN]"))
                    {
                        c.concept = c.concept.Replace("[SIN]", e.Labor_unions.labor_union);
                    }
                }
            }
        }

        #region ItemsSource
        public List<Concepts> allConceptsNotCommons = ConceptB.SelectConceptsNotCommonsIncludeAll();

        public List<Employees> allEmployees = EmployeeB.SelectEmployeesIncludeAll();
        public ObservableCollection<Employees> Employees { get; set; } = new ObservableCollection<Employees>();

        public ObservableCollection<Concepts> Concepts { get; set; } = new ObservableCollection<Concepts>();
        public ObservableCollection<Concepts> AddConcepts { get; set; } = new ObservableCollection<Concepts>();
        public List<Concepts> ConceptsE { get; set; } = ConceptB.SelectConceptsRAndNRIncludeAll();

        public ObservableCollection<Liquidation_fixed_datas> LiquidationFixedDatas =>
            new ObservableCollection<Liquidation_fixed_datas>(LiquidationFixedDataB.SelectLiquidationFixedDatasIncludeAll());
        #endregion

        #region Properties   
        private int? _liquidationFixedDataId;
        public int? LiquidationFixedDataId
        {
            get => _liquidationFixedDataId;
            set
            {
                Set(ref _liquidationFixedDataId, value);
                if (value != null)
                {
                    LoadLiquidationFixedData();

                    Liquidation_fixed_datas liquidationFixedData = LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId);
                    if (liquidationFixedData.liquidation_type_id == 4 || liquidationFixedData.liquidation_type_id == 5)
                    {
                        ConceptEDialog = true;
                    }
                    else
                    {
                        #pragma warning disable CS4014 // Ya que no se esperaba esta llamada, la ejecución del método actual continúa antes de que se complete la llamada
                        ChargeEmployees();
                        #pragma warning restore CS4014 // Ya que no se esperab esta llamada, la ejecución del método actual continúa antes de que se complete la llamada
                    }
                    FileN = Employees.FirstOrDefault()?.file_n;
                }      
                else
                {
                    LiquidationEnabled = false;
                }
            }
        }

        public bool LiquidationEnabled { get; set; } = true;

        private string _periodS;
        public string PeriodS
        {
            get => _periodS;
            set => Set(ref _periodS, value);
        }

        private string _liquidationTypeS;
        public string LiquidationTypeS
        {
            get => _liquidationTypeS;
            set => Set(ref _liquidationTypeS, value);
        }

        private int? _fileN;
        public int? FileN
        {
            get => _fileN;
            set
            {
                _fileN = value;
                RaisePropertyChanged("FileN");
                if (value != null) LoadEmployee();
            }
        }

        private string _entryDateS;
        public string EntryDateS
        {
            get => _entryDateS;
            set => Set(ref _entryDateS, value);
        }

        private string _categoryS;
        public string CategoryS
        {
            get => _categoryS;
            set => Set(ref _categoryS, value);
        }

        private string _cuilS;
        public string CuilS
        {
            get => _cuilS;
            set => Set(ref _cuilS, value);
        }

        private decimal? _r;
        public decimal? R
        {
            get => _r;
            set => Set(ref _r, value);
        }

        private decimal? _nr;
        public decimal? NR
        {
            get => _nr;
            set => Set(ref _nr, value);
        }

        private decimal? _d;
        public decimal? D
        {
            get => _d;
            set => Set(ref _d, value);
        }

        private decimal? _net;
        public decimal? NET
        {
            get => _net;
            set => Set(ref _net, value);
        }

        public int? SortedConceptId { get; set; }

        private int _addSortedConceptId;
        public int AddSortedConceptId
        {
            get => _addSortedConceptId;
            set
            {
                Set(ref _addSortedConceptId, value);
                LoadAddedConcept();
            }
        }

        private decimal? editingQuantity;

        private int? _sortedConceptEId;
        public int? SortedConceptEId
        {
            get => _sortedConceptEId;
            set => Set(ref _sortedConceptEId, value);
        }

        private string _conceptE;
        public string ConceptE
        {
            get => _conceptE;
            set => Set(ref _conceptE, value);
        }

        private string _amountE;
        public string AmountE
        {
            get => _amountE;
            set => Set(ref _amountE, value);
        }

        public string VQuantityS { get; set; }
        public string VQuantity { get; set; }
        public decimal? VEditedQuantity { get; set; }
        public string VFormulaS { get; set; }
        public string VFormula { get; set; }
        public decimal VResult { get; set; }
        #endregion

        #region Methods
        private void MoveSelection(int index, int n)
        {
            FileN = Employees[(index + n) == Employees.Count ? 0 : (index + n) == -1 ? (Employees.Count - 1) : index + n].file_n;
        }

        private void LoadLiquidationFixedData()
        {
            Liquidation_fixed_datas liquidationFixedData = LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId);

            PeriodS = liquidationFixedData.period.ToString("MMMM").ToUpper() + " de " + liquidationFixedData.period.ToString("yyyy");
            LiquidationTypeS = liquidationFixedData.Liquidation_types.fullliquidationtype;
        }        

        private Concepts GetBonusConcept(int fileN)
        {
            int liquidatedMonths = LiquidationFixedDatas.Where(lfd => lfd.Employees_liquidated.Select(el => el.file_n).ToList().Contains(fileN)
            && lfd.period <= LiquidationFixedDatas.Single(x => x.liquidation_fixed_data_id == _liquidationFixedDataId).period && lfd.liquidation_type_id == 1).Count();
            Concepts c = new Concepts() { sorted_concept_id = 0, common = true, Formulas = new Formulas() };

            if (liquidatedMonths >= 6)
            {
                if (LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId).period.Month <= 6)
                {
                    c.concept = "SAC 1ER SEMESTRE";
                }
                else
                {
                    c.concept = "SAC 2DO SEMESTRE";
                }
                c.concept_type_id = 1;
                c.Formulas.formula = "MSUEB*0.5";
            }
            else
            {
                c.concept = "SAC PROPORCIONAL";
                c.concept_type_id = 1;
                c.Formulas.quantity = "ANTID";
                c.Formulas.quantity_leyend = "DÍAS";
                c.Formulas.formula = "MSUEB*0.5*(ANTID/SEMDI)";
            }

            return c;
        }

        private Concepts GetEConcept(int t)
        {
            if (SortedConceptEId != null)
            {
                Concepts concept = ConceptsE.Single(c => c.sorted_concept_id == SortedConceptEId);
                return new Concepts()
                {
                    sorted_concept_id = concept.sorted_concept_id,
                    concept = concept.concept,
                    concept_type_id = t,
                    percentage = concept.percentage,
                    amount = concept.amount,
                    common = true,

                    Formulas = new Formulas()
                    {
                        quantity = concept.Formulas.quantity,
                        quantity_leyend = concept.Formulas.quantity_leyend,
                        formula = concept.Formulas.formula
                    }
                };
            }
            else
            {
                return new Concepts()
                {
                    sorted_concept_id = 0,
                    concept = ConceptE,
                    concept_type_id = t,
                    amount = decimal.Parse(AmountE.Contains(".") ? AmountE.Insert(AmountE.Length, "00").Replace(".", ",") : AmountE.Insert(AmountE.Length, ",00")),
                    common = true,

                    Formulas = new Formulas()
                    {
                        formula = "IMPOR"
                    }
                };
            }
        }

        private async Task ChargeEmployees()
        {
            LoadingDialog = true;

            List<Employees> employees = new List<Employees>();
            await Task.Run(() =>
            {
                foreach (Employees e in allEmployees)
                {
                    Employees employee = new Employees()
                    {
                        file_n = e.file_n,
                        name = e.name,
                        last_name = e.last_name,
                        entry_date = e.entry_date,
                        cuil_n = e.cuil_n,
                        basic_salary = e.basic_salary,

                        Categorys = e.Categorys,
                        Social_works = e.Social_works,
                        Labor_unions = e.Labor_unions
                    };

                    switch (LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId).liquidation_type_id)
                    {
                        case 1:
                            e.Concepts
                                .ToList()
                                .ForEach(c => employee.Concepts.Add(new Concepts()
                                {
                                    sorted_concept_id = c.sorted_concept_id,
                                    concept = c.concept,
                                    percentage = c.percentage,
                                    amount = c.amount,
                                    quantity_editable = c.quantity_editable,
                                    common = c.common,
                                    concept_type_id = c.concept_type_id,

                                    Formulas = new Formulas()
                                    {
                                        formula = c.Formulas.formula,
                                        quantity = c.Formulas.quantity,
                                        quantity_leyend = c.Formulas.quantity_leyend
                                    }
                                }));
                            if (LiquidationFixedDatas.Where(lfd => lfd.period == LiquidationFixedDatas.Single(lfdx => lfdx.liquidation_fixed_data_id == _liquidationFixedDataId).period
                            && lfd.liquidation_type_id == 3).Any(lfd => lfd.Employees_liquidated.Select(el => el.file_n).ToList().Contains(employee.file_n)))
                                employee.Concepts.Add(new Concepts()
                                {
                                    sorted_concept_id = 400,
                                    concept = "DIFERENCIA VACACIONES",
                                    concept_type_id = 1,
                                    quantity_editable = true,
                                    common = true,

                                    Formulas = new Formulas()
                                    {
                                        formula = "SUEBT/30*CANTI-SUEBT",
                                        quantity = "30-DIVAF",
                                        quantity_leyend = "DÍAS"
                                    }
                                });
                            break;
                        case 2:
                            e.Concepts
                                .Where(c => c.concept_type_id == 3)
                                .ToList()
                                .ForEach(c => employee.Concepts.Add(new Concepts()
                                {
                                    sorted_concept_id = c.sorted_concept_id,
                                    concept = c.concept,
                                    percentage = c.percentage,
                                    amount = c.amount,
                                    quantity_editable = c.quantity_editable,
                                    common = c.common,
                                    concept_type_id = c.concept_type_id,

                                    Formulas = new Formulas()
                                    {
                                        formula = c.Formulas.formula,
                                        quantity = c.Formulas.quantity,
                                        quantity_leyend = c.Formulas.quantity_leyend
                                    }
                                }));
                            employee.Concepts.Add(GetBonusConcept(employee.file_n));
                            break;
                        case 3:
                            e.Concepts
                                .Where(c => c.concept_type_id == 3)
                                .ToList()
                                .ForEach(c => employee.Concepts.Add(new Concepts()
                                {
                                    sorted_concept_id = c.sorted_concept_id,
                                    concept = c.concept,
                                    percentage = c.percentage,
                                    amount = c.amount,
                                    quantity_editable = c.quantity_editable,
                                    common = c.common,
                                    concept_type_id = c.concept_type_id,

                                    Formulas = new Formulas()
                                    {
                                        formula = c.Formulas.formula,
                                        quantity = c.Formulas.quantity,
                                        quantity_leyend = c.Formulas.quantity_leyend
                                    }
                                }));
                            employee.Concepts.Add(new Concepts()
                            {
                                sorted_concept_id = 0,
                                concept = "ADELANTO VACACIONAL",
                                common = true,
                                quantity_editable = true,
                                concept_type_id = 1,

                                Formulas = new Formulas()
                                {
                                    formula = "SUEBP/25*CANTI",
                                    quantity = "DIVAP",
                                    quantity_leyend = "DÍAS"
                                }
                            });
                            break;
                        case 4:
                            e.Concepts
                                .Where(c => c.concept_type_id == 3)
                                .ToList()
                                .ForEach(c => employee.Concepts.Add(new Concepts()
                                {
                                    sorted_concept_id = c.sorted_concept_id,
                                    concept = c.concept,
                                    percentage = c.percentage,
                                    amount = c.amount,
                                    quantity_editable = c.quantity_editable,
                                    common = c.common,
                                    concept_type_id = c.concept_type_id,

                                    Formulas = new Formulas()
                                    {
                                        formula = c.Formulas.formula,
                                        quantity = c.Formulas.quantity,
                                        quantity_leyend = c.Formulas.quantity_leyend
                                    }
                                }));
                            employee.Concepts.Add(GetEConcept(1));
                            break;
                        case 5:
                            employee.Concepts.Add(GetEConcept(2));
                            break;
                        case 6:
                            e.Concepts
                                .ToList()
                                .ForEach(c => employee.Concepts.Add(new Concepts()
                                {
                                    sorted_concept_id = c.sorted_concept_id,
                                    concept = c.concept,
                                    percentage = c.percentage,
                                    amount = c.amount,
                                    quantity_editable = c.quantity_editable,
                                    common = c.common,
                                    concept_type_id = c.concept_type_id,

                                    Formulas = new Formulas()
                                    {
                                        formula = c.Formulas.formula,
                                        quantity = c.Formulas.quantity,
                                        quantity_leyend = c.Formulas.quantity_leyend
                                    }
                                }));
                            break;
                    }
                    
                    foreach (Concepts c in employee.Concepts)
                    {
                        if (c.concept.Contains("[OBR]"))
                        {
                            c.concept = c.concept.Replace("[OBR]", e.Social_works.social_work);
                        }
                        else if (c.concept.Contains("[SIN]"))
                        {
                            c.concept = c.concept.Replace("[SIN]", e.Labor_unions.labor_union);
                        }
                    }

                    employees.Add(employee);
                }
            });

            Employees.Clear();
            employees.ForEach(e => Employees.Add(e));
            FileN = Employees.First().file_n;
        }

        private void LoadEmployee()
        {
            Employees employee = Employees.Single(e => e.file_n == _fileN);

            EntryDateS = employee.entry_date.ToString("dd/MM/yyyy");
            CategoryS = employee.Categorys.category;
            CuilS = employee.cuil_n.ToString().Substring(0, 2) + "-" +
                employee.cuil_n.ToString().Substring(2, 8) + "-" +
                employee.cuil_n.ToString().Substring(10, 1);

            if (Concepts.Count > 0) Concepts.Clear();
            employee.Concepts.ToList().ForEach(c => Concepts.Add(c));

            CalculateConcepts(employee);
        }

        private async void CalculateConcepts(Employees employee)
        {
            if (!_loadingDialog && !_globalLoadingDialog) LoadingDialog = true;

            await Task.Run(() =>
            {
                using (CalculateVariable calculateVariable = new CalculateVariable())
                {
                    DateTime period = LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId).period;

                    foreach (Concepts c in Concepts.OrderBy(c => c.sorted_concept_id))
                    {
                        if (!string.IsNullOrWhiteSpace(c.Formulas.quantity) || c.quantitynedited != null)
                        {
                            if (c.quantitynedited != null)
                            {
                                decimal d = (decimal)c.quantitynedited;
                                calculateVariable._quantity = d;
                                c.quantityn = c.quantitynedited < 1 ? decimal.Round(d * 100, 2).ToString().Replace(",", ".") : c.quantitynedited.ToString();
                            }
                            else
                            {
                                decimal d = calculateVariable.CalculateQuantity(c.Formulas.quantity, employee, c, period);
                                c.quantityn = d < 1 ? decimal.Round(d * 100, 2).ToString().Replace(",", ".") : d.ToString();
                            }
                        }
                        else
                        {
                            c.quantityn = null;
                            calculateVariable._quantity = 0;
                        }

                        switch (c.concept_type_id)
                        {
                            case 1:
                                c.r = Convert.ToDecimal(string.Format("{0:F2}", calculateVariable.CalculateFormula(c.Formulas.formula, employee, c, period)));
                                break;
                            case 2:
                                c.nr = Convert.ToDecimal(string.Format("{0:F2}", calculateVariable.CalculateFormula(c.Formulas.formula, employee, c, period)));
                                break;
                            case 3:
                                c.d = Convert.ToDecimal(string.Format("{0:F2}", calculateVariable.CalculateFormula(c.Formulas.formula, employee, c, period)));
                                break;
                        }
                    }

                    R = calculateVariable._r;
                    NR = calculateVariable._nr;
                    D = calculateVariable._d;
                    NET = ((_r ?? 0) - (_d ?? 0)) + (_nr ?? 0);
                }
            });
            await Task.Delay(200);

            if (!_globalLoadingDialog) LoadingDialog = false;
        }

        private void LoadAddedConcept()
        {
            Concepts concept = allConceptsNotCommons.Single(c => c.sorted_concept_id == _addSortedConceptId);
            Concepts updateConcept = Concepts.Last();

            updateConcept.sorted_concept_id = concept.sorted_concept_id;
            updateConcept.concept = concept.concept;
            updateConcept.percentage = concept.percentage;
            updateConcept.amount = concept.amount;
            updateConcept.quantity_editable = concept.quantity_editable;

            updateConcept.concept_type_id = concept.concept_type_id;
            updateConcept.Formulas = new Formulas()
            {
                formula = concept.Formulas.formula,
                quantity = concept.Formulas.quantity,
                quantity_leyend = concept.Formulas.quantity_leyend
            };
        }
        #endregion

        #region Dialog
        private bool _loadingDialog;
        public bool LoadingDialog
        {
            get => _loadingDialog;
            set => Set(ref _loadingDialog, value);
        }

        private bool _conceptEDialog;
        public bool ConceptEDialog
        {
            get => _conceptEDialog;
            set => Set(ref _conceptEDialog, value);
        }

        private bool _viewConceptDialog;
        public bool ViewConceptDialog
        {
            get => _viewConceptDialog;
            set => Set(ref _viewConceptDialog, value);
        }

        public string TextGlobalLoadingDialog => "Liquidando";

        private bool _globalLoadingDialog;
        public bool GlobalLoadingDialog
        {
            get => _globalLoadingDialog;
            set => Set(ref _globalLoadingDialog, value);
        }

        private bool _globalAlertDialog;
        public bool GlobalAlertDialog
        {
            get => _globalAlertDialog;
            set => Set(ref _globalAlertDialog, value);
        }
        #endregion

        #region Commands
        public ICommand Liquidate => new RelayCommand
            (
            async () =>
            {
                GlobalLoadingDialog = true;

                Liquidation_fixed_datas liquidationFixedData = LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId);
                Employees employee = Employees.Single(e => e.file_n == _fileN);

                Employees_liquidated employeesLiquidated = new Employees_liquidated()
                {
                    liquidation_fixed_data_id = (int)_liquidationFixedDataId,
                    file_n = (int)_fileN,
                    rem_total = _r ?? 0,
                    no_rem_total = _nr ?? 0,
                    ded_total = _d ?? 0,
                    basic_salary = employee.basic_salary ?? employee.Categorys.basic_salary,
                    gross_salary = (_r ?? 0) + (_nr ?? 0),
                    net_salary = _net ?? 0
                };

                await Task.Run(() => EmployeeLiquidatedB.InsertEmployeeLiquidated(employeesLiquidated));
                await Task.Run(() => EmployeeLiquidatedConceptB.InsertEmployeeLiquidatedConcepts(employeesLiquidated.employee_liquidated_id, employee.Concepts.ToList()));

                liquidationFixedData.Employees_liquidated.Add(employeesLiquidated);

                GlobalLoadingDialog = false;
                GlobalAlertDialog = true;
            }
            );

        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(Employees.IndexOf(Employees.Single(e => e.file_n == _fileN)), -1);
                },
                () => { return Employees.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(Employees.IndexOf(Employees.Single(e => e.file_n == _fileN)), 1);
            },
            () => { return Employees.Count > 1; }
            );

        public ICommand Add => new RelayCommand
                (
                () =>
                {
                    allConceptsNotCommons
                    .Where(c => Concepts.Any(cx => cx.sorted_concept_id == c.sorted_concept_id) &&
                    AddConcepts.Any(cx => cx.sorted_concept_id == c.sorted_concept_id))
                    .ToList()
                    .ForEach(c => AddConcepts.Remove(c));
                    allConceptsNotCommons
                    .Where(c => !Concepts.Any(cx => cx.sorted_concept_id == c.sorted_concept_id) &&
                    !AddConcepts.Any(cx => cx.sorted_concept_id == c.sorted_concept_id))
                    .ToList()
                    .ForEach(c => AddConcepts.Add(c));

                    Concepts.Add(new Concepts() { sorted_concept_id = 1000, add = true });
                    AddSortedConceptId = AddConcepts.First().sorted_concept_id;
                },
                () => { return Concepts.Where(c => !c.common).ToList().Count < allConceptsNotCommons.Count; }
                );

        public ICommand Update => new RelayCommand
            (
            () =>
            {
                Concepts concept = Concepts.Single(c => c.sorted_concept_id == SortedConceptId);
                concept.edit = true;
                editingQuantity = concept.quantitynedited;
            },
            () => { return Concepts.Any(c => c.sorted_concept_id == SortedConceptId && c.quantity_editable); }
            );

        public ICommand Delete => new RelayCommand
            (
            () =>
            {
                Employees employee = Employees.Single(e => e.file_n == _fileN);
                Concepts concept = Concepts.Single(c => c.sorted_concept_id == SortedConceptId);

                Concepts.Remove(concept);
                employee.Concepts.Remove(concept);
                CalculateConcepts(employee);
            },
            () => { return Concepts.Where(c => !c.common).Any(c => c.sorted_concept_id == SortedConceptId); }
            );

        public ICommand Apply => new RelayCommand
            (
            () =>
            {
                Employees employee = Employees.Single(e => e.file_n == _fileN);

                if (Concepts.Last().add)
                {
                    Concepts.Last().add = false;
                    employee.Concepts.Add(Concepts.Last());
                    Concepts.Move(Concepts.Count - 1, 0);
                }
                else
                {
                    Concepts.Single(c => c.edit).edit = false;
                }
                CalculateConcepts(employee);
            },
            () => { return _fileN > 0; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                if (Concepts.Last().add)
                {
                    Concepts.RemoveAt(Concepts.Count - 1);
                }
                else
                {
                    Concepts concept = Concepts.Single(c => c.edit);
                    concept.edit = false;
                    concept.quantitynedited = editingQuantity;
                }
            }
            );

        public ICommand ExistConceptE => new RelayCommand
            (
            async () =>
            {
                ConceptE = null;
                AmountE = null;

                await ChargeEmployees();

                SortedConceptEId = null;
            },
            () => { return SortedConceptEId != null; }
            );

        public ICommand NewConceptE => new RelayCommand
            (
            async () =>
            {
                SortedConceptEId = null;

                await ChargeEmployees();

                ConceptE = null;
                AmountE = null;
            },
            () => { return !string.IsNullOrEmpty(ConceptE) && !string.IsNullOrWhiteSpace(AmountE); }
            );

        public ICommand CancelConceptE => new RelayCommand
           (
           () =>
           {
               ConceptEDialog = false;
           }
           );

        public ICommand ViewConcept => new RelayCommand
           (
           () =>
           {
               Concepts concept = Concepts.Single(c => c.sorted_concept_id == SortedConceptId);

               VQuantityS = concept.Formulas.quantity;
               VEditedQuantity = concept.quantitynedited;
               VFormulaS = concept.Formulas.formula;

               using (CalculateVariable calculateVariable = new CalculateVariable())
               {
                   if (!string.IsNullOrWhiteSpace(concept.Formulas.quantity))
                   {
                       VQuantity = calculateVariable.GetEquation(concept.Formulas.quantity, Employees.Single(e => e.file_n == _fileN),
                                concept, LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId).period, true);
                   }
                   else
                   {
                       VQuantity = null;
                   }
                   if (concept.quantitynedited != null) calculateVariable._quantity = (decimal)concept.quantitynedited;

                   VFormula = calculateVariable
                   .GetEquation(concept.Formulas.formula.Replace("TOTHA", _r.ToString().Replace(",", ".")).Replace("SUEBT", (_r ?? 0 + _nr ?? 0 - Concepts.SingleOrDefault(c => c.sorted_concept_id == 400)?.r).ToString().Replace(",", ".")),
                       Employees.Single(e => e.file_n == _fileN), concept, LiquidationFixedDatas.Single(lfd => lfd.liquidation_fixed_data_id == _liquidationFixedDataId).period, false);
                   VResult = (decimal)(concept.r ?? concept.nr ?? concept.d);
               }

               RaisePropertyChanged("VQuantityS");
               RaisePropertyChanged("VQuantity");
               RaisePropertyChanged("VEditedQuantity");
               RaisePropertyChanged("VFormulaS");
               RaisePropertyChanged("VFormula");
               RaisePropertyChanged("VResult");

               ViewConceptDialog = true;
           }
           );
        #endregion
    }
}
