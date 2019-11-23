using BusinessLayout.Business;
using Data.Helper;
using Data.Context;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class ConceptVM : ObservableObject, IDataErrorInfo
    {
        #region ItemsSource      
        public List<Concept_types> ConceptTypes => ConceptTypeB.SelectConceptTypes();
        public ObservableCollection<Formulas> Formulas { get; set; } = new ObservableCollection<Formulas>(FormulaB.SelectFormulas());

        private List<Variables> allVariables = VariableB.GetVariables();
        public ObservableCollection<Variables> Variables { get; set; } = new ObservableCollection<Variables>();

        private List<Concepts> allConcepts = ConceptB.SelectConceptsIncludeTypes();
        public ObservableCollection<Concepts> Concepts { get; set; } = new ObservableCollection<Concepts>();
        #endregion

        #region Properties   
        private int? _conceptId;
        public int? ConceptId
        {
            get => _conceptId;
            set
            {
                Set(ref _conceptId, value);
                if (!searching)
                if (value != null && Concepts.Any(c => c.concept_id == value))
                {
                    LoadConcept();
                }
                else
                {
                    ClearProperties();
                }
            }
        }

        private int? _conceptTypeId;
        public int? ConceptTypeId
        {
            get => _conceptTypeId;
            set => Set(ref _conceptTypeId, value);
        }

        private int? _sortedConceptId;
        public int? SortedConceptId
        {
            get => _sortedConceptId;
            set => Set(ref _sortedConceptId, value);
        }

        private string _concept;
        public string Concept
        {
            get => _concept;
            set => Set(ref _concept, value);
        }

        private bool _quantityEditable;
        public bool QuantityEditable
        {
            get => _quantityEditable;
            set => Set(ref _quantityEditable, value);
        }

        private bool _common;
        public bool Common
        {
            get => _common;
            set => Set(ref _common, value);
        }

        private decimal? _percentage;
        public decimal? Percentage
        {
            get => _percentage;
            set => Set(ref _percentage, value);
        }

        private decimal? _amount;
        public decimal? Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        private int? _formulaId;
        public int? FormulaId
        {
            get => _formulaId;
            set
            {
                Set(ref _formulaId, value);
                if (value != null)
                {
                    LoadFormula();
                }
                else
                {
                    ClearFormulaProperties();
                }
            }
        }

        private string _formula;
        public string Formula
        {
            get => _formula;
            set => Set(ref _formula, value);
        }

        private string _quantity;
        public string Quantity
        {
            get => _quantity;
            set => Set(ref _quantity, value);
        }

        private string _quantityLeyend;
        public string QuantityLeyend
        {
            get => _quantityLeyend;
            set => Set(ref _quantityLeyend, value);
        }

        private readonly List<string> quantityExceptions = new List<string>() { "CANTI", "TOTHA" };

        private bool isValidVariableFormula;
        private bool isValidVariableQuantity;
        #endregion

        #region Errors
        public string Error => null;
        public string this[string property]
        {
            get => GetValidationError(property, canCheckErrors);
        }

        private static readonly string[] validatedProperties = { "SortedConceptId", "Concept", "Formula", "Quantity" };
        public bool IsValid
        {
            get
            {
                foreach (string property in validatedProperties)
                    if (!string.IsNullOrEmpty(GetValidationError(property, true))) return false;
                return true;
            }
        }

        private bool canCheckErrors;
        private string GetValidationError(string property, bool canCheck)
        {
            if (canCheck)
            {
                string result = null;
                switch (property)
                {
                    case "SortedConceptId":
                        if (_sortedConceptId == null)
                        {
                            result = "El identificador del concepto es requerido";
                        }
                        else if (VerifySortedConceptId())
                        {
                            result = "Identificador incorrecto, pruebe otro";
                            break;
                        }
                        else if (allConcepts.Any(c => c.sorted_concept_id == _sortedConceptId) && 
                            allConcepts.SingleOrDefault(c => c.concept_id == (_conceptId ?? 0))?.sorted_concept_id != _sortedConceptId)
                        {
                            result = "El identificador ya existe, pruebe otro";
                        }
                        break;
                    case "Concept":
                        if (string.IsNullOrWhiteSpace(_concept))
                        {
                            result = "El nombre del concepto es requerido";
                        }
                        break;
                    case "Formula":
                        if (string.IsNullOrWhiteSpace(_formula))
                        {
                            result = "La fórmula es requerida";
                        }
                        else if (!string.IsNullOrEmpty(VerifyIfExistInvalidVariable(_formula, null)))
                        {
                            isValidVariableFormula = true;
                            result = "Se encontró variable errónea";
                        }
                        else
                        {
                            isValidVariableFormula = false;
                            if (VerifyResult(_formula))
                            {
                                result = "Error al procesar la fórmula, revísela";
                            }
                        }
                        break;
                    case "Quantity":
                        if (!string.IsNullOrWhiteSpace(_quantity))
                        {
                            if (!string.IsNullOrEmpty(VerifyIfExistInvalidVariable(_quantity, quantityExceptions)))
                            {
                                isValidVariableQuantity = true;
                                result = "Se encontró variable errónea";
                            }
                            else
                            {
                                isValidVariableQuantity = false;
                                if (VerifyResult(_quantity))
                                {
                                    result = "Error al procesar la cantidad, revísela";
                                }
                            }
                        }
                        break;
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Search
        private bool searching;
        public int Filter { get; set; }

        private int? _filterType;
        public int? FilterType
        {
            set
            {
                _filterType = value;
                SearchConcept();
            }
        }

        private string _search;
        public string Search
        {
            set
            {
                _search = value;
                SearchConcept();
            }
        }

        private void SearchConcept()
        {
            searching = true;
            if (string.IsNullOrWhiteSpace(_search))
            {
                if (_filterType == null)
                {
                    allConcepts
                        .Where(c => !Concepts.Any(cx => cx.concept_id == c.concept_id))
                        .ToList()
                        .ForEach(c => Concepts.Add(c));
                }
                else
                {
                    allConcepts
                        .Where(c => Concepts.Any(cx => cx.concept_id == c.concept_id) && c.concept_type_id != _filterType)
                        .ToList()
                        .ForEach(c => Concepts.Remove(c));
                    allConcepts
                        .Where(c => !Concepts.Any(cx => cx.concept_id == c.concept_id) && c.concept_type_id == _filterType)
                        .ToList()
                        .ForEach(c => Concepts.Add(c));
                }
            }
            else
            {
                SearchConceptByFilter();
            }

            searching = false;
            if (_conceptId == null) ConceptId = Concepts.OrderBy(c => c.sorted_concept_id).FirstOrDefault()?.concept_id;
        }
        private void SearchConceptByFilter()
        {
            switch (Filter)
            {
                case 0:
                    if (_filterType == null)
                    {
                        allConcepts
                            .Where(c => Concepts.Any(cx => cx.sorted_concept_id == c.sorted_concept_id) && c.sorted_concept_id != int.Parse(_search))
                            .ToList()
                            .ForEach(c => Concepts.Remove(c));
                        allConcepts
                            .Where(c => (!Concepts.Any(cx => cx.sorted_concept_id == c.sorted_concept_id) || Concepts.Count == 0) && c.sorted_concept_id == int.Parse(_search))
                            .ToList()
                            .ForEach(c => Concepts.Add(c));
                    }
                    else
                    {
                        allConcepts
                            .Where(c => Concepts.Any(cx => cx.sorted_concept_id == c.sorted_concept_id) && c.sorted_concept_id != int.Parse(_search) && c.concept_type_id == _filterType)
                            .ToList()
                            .ForEach(c => Concepts.Remove(c));
                        allConcepts
                            .Where(c => !Concepts.Any(cx => cx.sorted_concept_id == c.sorted_concept_id) && c.sorted_concept_id == int.Parse(_search) && c.concept_type_id == _filterType)
                            .ToList()
                            .ForEach(c => Concepts.Add(c));
                    }
                    break;
                case 1:
                    if (_filterType == null)
                    {
                        allConcepts
                            .Where(c => Concepts.Any(cx => cx.concept_id == c.concept_id) && !c.concept.Contains(_search.ToUpper()))
                            .ToList()
                            .ForEach(c => Concepts.Remove(c));
                        allConcepts
                            .Where(c => !Concepts.Any(cx => cx.concept_id == c.concept_id) && c.concept.Contains(_search.ToUpper()))
                            .ToList()
                            .ForEach(c => Concepts.Add(c));
                    }
                    else
                    {
                        allConcepts
                            .Where(c => Concepts.Any(cx => cx.concept_id == c.concept_id) && !c.concept.Contains(_search.ToUpper()) && c.concept_type_id == _filterType)
                            .ToList()
                            .ForEach(c => Concepts.Remove(c));
                        allConcepts
                            .Where(c => !Concepts.Any(cx => cx.concept_id == c.concept_id) && c.concept.Contains(_search.ToUpper()) && c.concept_type_id == _filterType)
                            .ToList()
                            .ForEach(c => Concepts.Add(c));
                    }
                    break;
            }
        }
                
        public string SearchV
        {
            set => SearchVariable(value);
        }
        private void SearchVariable(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                allVariables
                    .Where(v => !Variables.Any(vx => vx.variable == v.variable))
                    .ToList()
                    .ForEach(v => Variables.Add(v));
            }
            else
            {
                allVariables
                    .Where(v => Variables.Any(vx => vx.variable == v.variable) && !v.variable.Contains(s.ToUpper()))
                    .ToList()
                    .ForEach(v => Variables.Remove(v));
                allVariables
                    .Where(v => !Variables.Any(vx => vx.variable == v.variable) && v.variable.Contains(s.ToUpper()))
                    .ToList()
                    .ForEach(v => Variables.Add(v));
            }
        }
        #endregion

        #region Methods
        private void ClearProperties()
        {
            ConceptTypeId = null;
            SortedConceptId = null;
            Concept = null;
            QuantityEditable = false;
            Common = false;
            Percentage = null;
            Amount = null;

            FormulaId = null;
        }

        private void ClearFormulaProperties()
        {
            Formula = null;
            Quantity = null;
            QuantityLeyend = null;
        }

        private Concepts GetConcept(int formulaId)
        {
            return new Concepts
            {
                concept_id = _conceptId ?? 0,
                sorted_concept_id = (int)_sortedConceptId,
                concept = _concept.ToUpper(),
                common = _common,
                quantity_editable = _quantityEditable,
                concept_type_id = (int)_conceptTypeId,
                percentage = _percentage,
                amount = _amount,

                formula_id = formulaId
            };
        }

        private Formulas GetFormula()
        {
            return new Formulas
            {
                formula = _formula,
                quantity = _quantity,
                quantity_leyend = _quantityLeyend?.ToUpper()
            };
        }

        private async Task<Concepts> GetConceptWithFormula()
        {
            Formulas formula = GetFormula();
            if (Formulas.Any(f => f.formula == formula.formula && f.quantity == formula.quantity && f.quantity_leyend == formula.quantity_leyend))
            {
                formula.formula_id = Formulas.Single(f => f.formula == formula.formula && f.quantity == formula.quantity && f.quantity_leyend == formula.quantity_leyend).formula_id;
            }
            else
            {
                await Task.Run(() => FormulaB.InsertFormula(formula));
                Formulas.Add(formula);
            }

            return GetConcept(formula.formula_id);            
        }

        private void MoveSelection(int index, int n)
        {
            ConceptId = Concepts.OrderBy(c => c.sorted_concept_id).ToList()[(index + n) == Concepts.Count ? 0 : (index + n) == -1 ? (Concepts.Count - 1) : index + n].concept_id;
        }

        private void LoadConcept()
        {
            Concepts concept = Concepts.Single(c => c.concept_id == _conceptId);

            ConceptTypeId = concept.concept_type_id;
            SortedConceptId = concept.sorted_concept_id;
            Concept = concept.concept;
            QuantityEditable = concept.quantity_editable;
            Common = concept.common;
            Percentage = concept.percentage;
            Amount = concept.amount;

            FormulaId = concept.formula_id;
        }

        private void LoadFormula()
        {
            Formulas formula = Formulas.Single(f => f.formula_id == _formulaId);

            Formula = formula.formula;
            Quantity = formula.quantity;
            QuantityLeyend = formula.quantity_leyend;
        }

        private void UpdateProperties(Concepts oldC, Concepts newC)
        {
            if (oldC.sorted_concept_id != newC.sorted_concept_id) oldC.sorted_concept_id = newC.sorted_concept_id;
            if (oldC.concept != newC.concept) oldC.concept = newC.concept;
            if (oldC.concept_type_id != newC.concept_type_id) oldC.concept_type_id = newC.concept_type_id;
            if (oldC.Concept_types != newC.Concept_types) oldC.Concept_types = newC.Concept_types;
            if (oldC.quantity_editable != newC.quantity_editable) oldC.quantity_editable = newC.quantity_editable;
            if (oldC.common != newC.common) oldC.common = newC.common;
            if (oldC.percentage != newC.percentage) oldC.percentage = newC.percentage;
            if (oldC.amount != newC.amount) oldC.amount = newC.amount;
            if (oldC.formula_id != newC.formula_id) oldC.formula_id = newC.formula_id;
        }

        private bool VerifySortedConceptId()
        {
            switch (_conceptTypeId)
            {
                case 1:
                    return _sortedConceptId > 0 && _sortedConceptId < 201 ? false : true;
                case 2:
                    return _sortedConceptId > 200 && _sortedConceptId < 400 ? false : true;
                case 3:
                    return _sortedConceptId > 400 && _sortedConceptId < 999 ? false : true;
                default:
                    return true;
            }
        }
       
        private string VerifyIfExistInvalidVariable(string s, List<string> exceptions)
        {
            List<string> variables = s.Split(new char[] { '+', '-', '*', '/', '(', ')', '?', '<', '>', '=', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

            foreach (string variable in variables)
            {
                if (!decimal.TryParse(variable, out decimal result) && ((exceptions?.Contains(variable) ?? false) || !allVariables.Any(v => v.variable == variable))) return variable;
            }

            return null;
        }

        private bool VerifyResult(string s)
        {
            List<string> variables = s.Split(new char[] { '+', '-', '*', '/', '(', ')', '?', '<', '>', '=', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
            s = s.Replace("?", "IIF");

            foreach (string variable in variables)
            {
                if (!decimal.TryParse(variable, out decimal result)) s = s.Replace(variable, "1");
            }

            try
            {
                new DataTable().Compute(s, null);
                return false;
            }
            catch (Exception)
            {
                return true;
            }
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

        public string ConfirmDialogText => "¿Desea eliminar el concepto?";

        public ICommand OpenConfirmDialog => new RelayCommand
                (
                () => { TextLoadingDialog = "Eliminando"; ConfirmDialog = true; },
                () => { return _conceptId != null; }
                );

        public ICommand AcceptDialog => new RelayCommand
                (
                async () =>
                {
                    LoadingDialog = true;

                    await Task.Run(() => ConceptB.DeleteConcept((int)_conceptId));

                    allConcepts.Remove(allConcepts.Single(c => c.concept_id == _conceptId));
                    Concepts.Remove(Concepts.Single(c => c.concept_id == _conceptId));

                    LoadingDialog = false;
                });
        #endregion

        #region Commands
        public ICommand Previous => new RelayCommand
                (
                () =>
                {
                    MoveSelection(Concepts.OrderBy(c => c.sorted_concept_id).ToList().IndexOf(Concepts.Single(c => c.concept_id == _conceptId)), -1);
                },
                () => { return Concepts.Count > 1; }
                );

        public ICommand Next => new RelayCommand
            (
            () =>
            {
                MoveSelection(Concepts.OrderBy(c => c.sorted_concept_id).ToList().IndexOf(Concepts.Single(c => c.concept_id == _conceptId)), 1);
            },
            () => { return Concepts.Count > 1; }
            );

        public ICommand New => new RelayCommand
                (
                () =>
                {
                    TextLoadingDialog = "Agregando";
                    canCheckErrors = true;
                }
                );

        public ICommand Update => new RelayCommand
                (
                () =>
                {
                    TextLoadingDialog = "Actualizando";
                    canCheckErrors = true;
                },
                () => { return _conceptId != null; }
                );

        public ICommand Apply => new RelayCommand
            (
            async () =>
            {
                LoadingDialog = true;

                Concepts concept = await GetConceptWithFormula();
                canCheckErrors = false;

                if (concept.concept_id == 0)
                {
                    await Task.Run(() => ConceptB.InsertConcept(concept));
                    concept.Concept_types = ConceptTypes.Single(ct => ct.concept_type_id == concept.concept_type_id);
                    allConcepts.Add(concept);
                    SearchConcept();
                }
                else
                {
                    await Task.Run(() => ConceptB.UpdateConcept(concept, Concepts.Single(c => c.concept_id == _conceptId).common));
                    concept.Concept_types = ConceptTypes.Single(ct => ct.concept_type_id == concept.concept_type_id);
                    UpdateProperties(Concepts.Single(c => c.concept_id == _conceptId), concept);                    
                }

                ConceptId = concept.concept_id;
                LoadingDialog = false;
            },
            () => { return IsValid; }
            );

        public ICommand Cancel => new RelayCommand
            (
            () =>
            {
                canCheckErrors = false;
                if (_conceptId == null)
                {
                    ConceptId = Concepts.FirstOrDefault()?.concept_id;
                }
                else
                {
                    LoadConcept();
                }
            }
            );

        public ICommand RenewSortedConceptId => new RelayCommand
            (
            () =>
            {
                switch (_conceptTypeId)
                {
                    case 1:
                        SortedConceptId = ConceptB.SelectAvailableId(0, 201);
                        break;
                    case 2:
                        SortedConceptId = ConceptB.SelectAvailableId(200, 400);
                        break;
                    case 3:
                        SortedConceptId = ConceptB.SelectAvailableId(400, 999);
                        break;
                }
            }
            );        

        public ICommand SearchInvalidVariable => new RelayCommand
        (
        () =>
           {
               if (!string.IsNullOrWhiteSpace(_quantity))
               {
                   Messenger.Default.Send(new NotificationMessage(VerifyIfExistInvalidVariable(_quantity, quantityExceptions)), "Quantity");
               }

               if (!string.IsNullOrWhiteSpace(_formula))
               {
                   Messenger.Default.Send(new NotificationMessage(VerifyIfExistInvalidVariable(_formula, null)), "Formula");
               }                                                                        
           },
           () => { return isValidVariableFormula || isValidVariableQuantity; }
           );
        #endregion
    }
}
