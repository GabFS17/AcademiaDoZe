//Gabriel Francisco de Sousa
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Domain.Enums;

[Flags]
public enum MatriculaPlano
{
    Mensal = 0,
    Trimestral = 1,
    Semestral = 2,
    Anual = 4
}
