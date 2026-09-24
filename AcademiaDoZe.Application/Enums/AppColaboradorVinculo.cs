using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;
namespace AcademiaDoZe.Application.Enums;

public enum AppColaboradorVinculo
{
    [Display(Name = "CLT")]
    CLT = 0,
    [Display(Name = "Estagiário")]
    Estagio = 1
}