using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Application.Interfaces;

public interface IAcessoAlunoService
{
    /// <summary>
    /// Obtém um acesso pelo ID, enriquecida com os dados do aluno.
    /// </summary>
    Task<AcessoAlunoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Obtém todas os acessos cadastradas.
    /// </summary>
    Task<IEnumerable<AcessoAlunoDto>> ObterTodasAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Cadastra um nova acesso para o colaborador
    /// </summary>
    Task<AcessoAlunoDto> AdicionarAsync(AcessoAlunoDto acessoAlunoDto, CancellationToken cancellationToken = default);
    /// <summary>
    /// Atualiza os dados de um acesso existente.
    /// </summary>
    Task<AcessoAlunoDto> AtualizarAsync(AcessoAlunoDto acessoAlunoDto, CancellationToken cancellationToken = default);
    /// <summary>
    /// Remove um acesso pelo ID.
    /// </summary>
    Task<bool> RemoverAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Obtém todos os acessos de um aluno em determinado período
    /// </summary>
    Task<IEnumerable<AcessoAlunoDto>> ObterAcessosPorAlunoPeriodoAsync(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Obtém o último acesso de um aluno.
    /// </summary>
    Task<AcessoAlunoDto?> ObterUltimoAcessoAsync(int alunoId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Verifica se o aluno está na academia.
    /// </summary>
    Task<bool> EstaNaAcademiaAsync(int alunoId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retorna o horário em que a academia tem mais procura.
    /// </summary>
    Task<Dictionary<TimeOnly, int>> ObterHorarioMaisProcuradoPorMesAsync(int mes, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retorna quantas horas, em média, os alunos permaneceram na academia no mês.
    /// </summary>
    Task<Dictionary<int, TimeSpan>> ObterPermanenciaMediaPorMesAsync(int mes, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retorna uma lista de alunos que não acessaram em uma quantidade de dias.
    /// </summary>
    Task<IEnumerable<AlunoDto>> ObterAlunosSemAcessoNosUltimosDiasAsync(int dias, CancellationToken cancellationToken = default);
}