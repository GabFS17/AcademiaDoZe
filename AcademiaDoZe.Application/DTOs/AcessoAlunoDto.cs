using System;
using System.Collections.Generic;
using System.Text;

using AcademiaDoZe.Domain.Entities;
namespace AcademiaDoZe.Application.DTOs;

public class AcessoAlunoDto
{
    public required int AlunoId { get; set; }
    public required Aluno Aluno { get; set; }
    public required DateTime DataHora { get; set; }
}