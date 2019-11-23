using Data.Helper;
using System.Collections.Generic;

namespace BusinessLayout.Business
{
    public class VariableB
    {
        public static List<Variables> GetVariables()
        {
            return new List<Variables>()
                {
                new Variables()
                {
                    variable = "PORCE",
                    variable_tooltip = "Referencia al porcentaje introducido"
                },
                new Variables()
                {
                    variable = "IMPOR",
                    variable_tooltip = "Referencia al importe introducido"
                },
                new Variables()
                {
                    variable = "CANTI",
                    variable_tooltip = "Referencia a la cantidad introducida"
                },
                new Variables()
                {
                    variable = "SUEBA",
                    variable_tooltip = "Sueldo básico del empleado"
                },
                new Variables()
                {
                    variable = "CATA1",
                    variable_tooltip = "Importe adicional (1) de la categoría"
                },
                new Variables()
                {
                    variable = "CATA2",
                    variable_tooltip = "Importe adicional (2) de la categoría"
                },
                new Variables()
                {
                    variable = "CATA3",
                    variable_tooltip = "Importe adicional (3) de la categoría"
                },
                new Variables()
                {
                    variable = "CATA4",
                    variable_tooltip = "Importe adicional (4) de la categoría"
                },
                new Variables()
                {
                    variable = "OBRRP",
                    variable_tooltip = "Porcentaje de retención de la obra social"
                },
                new Variables()
                {
                    variable = "OBRRI",
                    variable_tooltip = "Importe de retención de la obra social"
                },
                new Variables()
                {
                    variable = "SINRP",
                    variable_tooltip = "Porcentaje de retención del sindicato"
                },
                new Variables()
                {
                    variable = "SINRI",
                    variable_tooltip = "Importe de retención del sindicato"
                },
                new Variables()
                {
                    variable = "ANTIA",
                    variable_tooltip = "Cantidad de años de antigüedad del empleado"
                },
                new Variables()
                {
                    variable = "ANTID",
                    variable_tooltip = "Cantidad de días de antigüedad del empleado"
                },
                new Variables()
                {
                    variable = "TOTHA",
                    variable_tooltip = "Total de haberes del empleado"
                },
                new Variables()
                {
                    variable = "SEMDI",
                    variable_tooltip = "Cantidad de días del semestre"
                }
            };
        }
    }
}
