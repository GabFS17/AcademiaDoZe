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

public class AcessoColaboradorService : IAcessoColaboradorService
{
    private readonly Func<IAcessoColaboradorRepository> _repoFactory;
    public AcessoColaboradorService(Func<IAcessoColaboradorRepository> repoFactory)
    {
        _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
    }


    public async Task<AcessoColaboradorDto?> ObterPorIdAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        var acesso = await _repoFactory().ObterPorId(id, cancellationToken);
        if (acesso == null) return null;
        return acesso.ToDto();
    }

    public async Task<IEnumerable<AcessoColaboradorDto>> ObterTodasAsync(
        CancellationToken cancellationToken = default)
    {
        var acessos = await _repoFactory().ObterTodos(cancellationToken);
        if (acessos == null) return null;
        return acessos.Select(a => a.ToDto());
    }

    public async Task<AcessoColaboradorDto> AdicionarAsync(
        AcessoColaboradorDto entity,
        CancellationToken cancellationToken = default)
    {
        var acesso = await _repoFactory().Adicionar(entity.ToEntity(), cancellationToken);
        if (acesso == null) return null;
        return acesso.ToDto();
    }

    public async Task<AcessoColaboradorDto> AtualizarAsync(
        AcessoColaboradorDto entity,
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

    public async Task<IEnumerable<AcessoColaboradorDto>> ObterAcessosPorColaboradorPeriodoAsync(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null, CancellationToken cancellationToken = default)
    {
        var acessos = (await _repoFactory().ObterAcessosPorColaboradorPeriodo(
            colaboradorId,
            inicio,
            fim,
            cancellationToken)).ToList();
        if (acessos == null) return null;

        return [.. acessos.Select(c => c.ToDto())];
    }

    public async Task<AcessoColaboradorDto?> ObterUltimoAcessoAsync(int colaboradorId, CancellationToken cancellationToken = default)
    {

        var acesso = await _repoFactory().ObterUltimoAcesso(
            colaboradorId,
            cancellationToken);
        if (acesso == null) return null;
        return acesso.ToDto();
    }

    public async Task<TimeSpan> ObterHorasTrabalhadasNoDiaAsync(int colaboradorId, DateOnly data, CancellationToken cancellationToken = default)
    {
        return await _repoFactory().ObterHorasTrabalhadasNoDia(
            colaboradorId,
            data,
            cancellationToken);
    }
}
   
