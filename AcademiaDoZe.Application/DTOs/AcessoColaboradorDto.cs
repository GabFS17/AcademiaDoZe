using System;
using System.Collections.Generic;
using System.Text;

using AcademiaDoZe.Domain.Entities;
namespace AcademiaDoZe.Application.DTOs;

public class AcessoColaboradorDto
{
    public required int ColaboradorId { get; set; }
    public required Colaborador Colaborador { get; set; }
    public required DateTime DataHora { get; set; }
}