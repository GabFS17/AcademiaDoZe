using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcademiaDoZe.Application.Interfaces;

public interface IAcessoColaboradorService
{
    /// <summary>
    /// Obtém um acesso pelo ID, enriquecida com os dados do aluno.
    /// </summary>
    Task<AcessoColaboradorDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Obtém todas os acessos cadastradas.
    /// </summary>
    Task<IEnumerable<AcessoColaboradorDto>> ObterTodasAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Cadastra um nova acesso para o colaborador
    /// </summary>
    Task<AcessoColaboradorDto> AdicionarAsync(AcessoColaboradorDto acessoColaboradorDto, CancellationToken cancellationToken = default);
    /// <summary>
    /// Atualiza os dados de um acesso existente.
    /// </summary>
    Task<AcessoColaboradorDto> AtualizarAsync(AcessoColaboradorDto acessoColaboradorDto, CancellationToken cancellationToken = default);
    /// <summary>
    /// Remove um acesso pelo ID.
    /// </summary>
    Task<bool> RemoverAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Obtém todos os acessos de um colaborador em determinado período
    /// </summary>
    Task<IEnumerable<AcessoColaboradorDto>> ObterAcessosPorColaboradorPeriodoAsync(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Obtém o último acesso de um colaborador.
    /// </summary>
    Task<AcessoColaboradorDto?> ObterUltimoAcessoAsync(int colaboradorId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retorna a quantidade de horas trabalhadas no dia.
    /// </summary>
    Task<TimeSpan> ObterHorasTrabalhadasNoDiaAsync(int colaboradorId, DateOnly data, CancellationToken cancellationToken = default);

}
