using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Exceptions;
using AcademiaDoZe.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;


using AcademiaDoZe.Application.Mappings;

namespace AcademiaDoZe.Application.Services;

public class AcessoAlunoService : IAcessoAlunoService
{
    private readonly Func<IAcessoAlunoRepository> _repoFactory;
    public AcessoAlunoService(Func<IAcessoAlunoRepository> repoFactory)
    {
        _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
    }


    public async Task<AcessoAlunoDto?> ObterPorIdAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        var acesso = await _repoFactory().ObterPorId(id, cancellationToken);
        if (acesso == null) return null;
        return acesso.ToDto();
    }

    public async Task<IEnumerable<AcessoAlunoDto>> ObterTodasAsync(
        CancellationToken cancellationToken = default)
    {
        var acessos = await _repoFactory().ObterTodos(cancellationToken);
        if (acessos == null) return null;
        return acessos.Select(a => a.ToDto());
    }

    public async Task<AcessoAlunoDto> AdicionarAsync(
        AcessoAlunoDto entity,
        CancellationToken cancellationToken = default)
    {
        var acesso = await _repoFactory().Adicionar(entity.ToEntity(), cancellationToken);
        if (acesso == null) return null;
        return acesso.ToDto();
    }

    public async Task<AcessoAlunoDto> AtualizarAsync(
        AcessoAlunoDto entity,
        CancellationToken cancellationToken = default)
    {
        var acesso = await _repoFactory().Atualizar(entity.ToEntity(), cancellationToken);
        if (acesso == null) return null;
        return acesso.ToDto();

    }

    public async Task<bool> RemoverAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repoFactory().Remover(id, cancellationToken);
    }

    public async Task<IEnumerable<AcessoAlunoDto>> ObterAcessosPorAlunoPeriodoAsync(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null, CancellationToken cancellationToken = default)
    {
        var acessos = (await _repoFactory().ObterAcessosPorAlunoPeriodo(
            alunoId,
            inicio,
            fim,
            cancellationToken)).ToList();
        if (acessos == null) return null;

        return [.. acessos.Select(c => c.ToDto())];
    }

    public async Task<AcessoAlunoDto?> ObterUltimoAcessoAsync(int alunoId, CancellationToken cancellationToken = default)
    {

        var acesso = await _repoFactory().ObterUltimoAcesso(
            alunoId,
            cancellationToken);
        if (acesso == null) return null;
        return acesso.ToDto();
    }

    public async Task<bool> EstaNaAcademiaAsync(int alunoId, CancellationToken cancellationToken = default)
    {
        return await _repoFactory().EstaNaAcademia(alunoId, cancellationToken);
    }

    public async Task<Dictionary<TimeOnly, int>> ObterHorarioMaisProcuradoPorMesAsync(int mes, CancellationToken cancellationToken = default)
    {
        return await _repoFactory().ObterHorarioMaisProcuradoPorMes(mes, cancellationToken);
    }

    public async Task<Dictionary<int, TimeSpan>> ObterPermanenciaMediaPorMesAsync(int mes, CancellationToken cancellationToken = default)
    {
        return await _repoFactory().ObterPermanenciaMediaPorMes(mes, cancellationToken);
    }

    public async Task<IEnumerable<AlunoDto>> ObterAlunosSemAcessoNosUltimosDiasAsync(int dias, CancellationToken cancellationToken = default)
    {
        var alunos = await _repoFactory().ObterAlunosSemAcessoNosUltimosDias(dias, cancellationToken);
        if (alunos == null) return null;
        return [.. alunos.Select(c => c.ToDto())];
    }
}

