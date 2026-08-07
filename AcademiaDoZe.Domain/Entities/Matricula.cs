//Gabriel Francisco de Sousa
using AcademiaDoZe.Domain.Common;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Services;
using AcademiaDoZe.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AcademiaDoZe.Domain.Entities;

public class Matricula : Entity
{
    public Aluno AlunoMatricula { get; private set; }
    public MatriculaPlano Plano { get; private set; }
    public DateOnly DataInicio { get; private set; }
    public DateOnly DataFinal { get; private set; }
    public string Objetivo { get; private set; }
    public MatriculaRestricoes RestricoesMedicas { get; private set; }
    public string ObservacoesRestricoes { get; private set; }
    public Arquivo? LaudoMedico { get; private set; }

    private Matricula(int id, Aluno alunoMatricula, MatriculaPlano plano, DateOnly dataInicio, DateOnly dataFinal, string objetivo, MatriculaRestricoes restricoesMedicas, string observacoesRestricoes, Arquivo? laudoMedico) : base(id)
    {
        AlunoMatricula = alunoMatricula;
        Plano = plano;
        DataInicio = dataInicio;
        DataFinal = dataFinal;
        Objetivo = objetivo;
        RestricoesMedicas = restricoesMedicas;
        ObservacoesRestricoes = observacoesRestricoes;
        LaudoMedico = laudoMedico;
    }
    public static Result<Matricula> Criar(int id, Aluno alunoMatricula, MatriculaPlano plano, DateOnly dataInicio, DateOnly dataFinal, string objetivo, MatriculaRestricoes restricoesMedicas, string observacoesRestricoes, Arquivo? laudoMedico)
    {
        var notifications = new List<Notification>();

        if (alunoMatricula == default)
            notifications.Add(new Notification("AlunoMatricula", "ALUNO_MATRICULA_OBRIGATORIO"));
        if (plano == default)
            notifications.Add(new Notification("MatriculaPlano", "PLANO_OBRIGATORIO"));
        if (dataInicio == default)
            notifications.Add(new Notification("DataInicio", "DATA_INICIO_OBRIGATORIA"));
        if (dataFinal == default)
            notifications.Add(new Notification("DataFinal", "DATA_FINAL_OBRIGATORIA"));
        if (NormalizadoService.TextoVazioOuNulo(objetivo))
            notifications.Add(new Notification("Objetivo", "OBJETIVO_MATRICULA_OBRIGATORIO"));
        else objetivo = NormalizadoService.LimparEspacos(objetivo);

        // alunos de 12 a 16 anos precisam de laudo medico autorizando a fazer atividades fisicas
        if ((alunoMatricula.DataNascimento >= DateOnly.FromDateTime(DateTime.Now).AddYears(-16))
            || (alunoMatricula.DataNascimento <= DateOnly.FromDateTime(DateTime.Now).AddYears(-12)))
        {
            if (laudoMedico == default)
                notifications.Add(new Notification("LaudoMedico", "LAUDO_OBRIGATORIO_POR_IDADE"));
            if (NormalizadoService.TextoVazioOuNulo(observacoesRestricoes))
                notifications.Add(new Notification("ObservacoesRestricoes", "OBSERVACOES_RESTRICOES_OBRIGATORIO"));
        }

        // restrições não nulas tbm necessitam de laudo
        if (restricoesMedicas != MatriculaRestricoes.None)
        {
            notifications.Add(new Notification("LaudoMedico", "LAUDO_OBRIGATORIO"));
        }

        if (notifications.Count != 0)
            return Result<Matricula>.Failure(notifications);

        var matricula = new Matricula(id, alunoMatricula, plano, dataInicio, dataFinal, objetivo, restricoesMedicas, observacoesRestricoes, laudoMedico);
        return Result<Matricula>.Success(matricula);
    }
}