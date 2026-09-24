using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Domain.ValueObjects;
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Exceptions;
using Google.Protobuf;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace AcademiaDoZe.Infrastructure.Repositories;

public class AcessoColaboradorRepository : BaseRepository, IAcessoColaboradorRepository
{
    public AcessoColaboradorRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType)
    {
    }
    private static string BaseSelectQuery => @"
    SELECT
    id_acesso, pessoa_tipo, pessoa_id, data_hora
    FROM tb_acesso";
    // MÉTODOS BASE IRepository

    //Task<TEntity?> ObterPorId(int id, CancellationToken cancellationToken = default);
    public async Task<AcessoColaborador?> ObterPorId(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = $"{BaseSelectQuery} WHERE id_acesso = @Id";
            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@Id", id, DbType.Int32);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            return await reader.ReadAsync(cancellationToken) ? Map(reader) : null;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_OBTER_POR_ID", $"Erro ao obter aluno por ID {id}: {ex.Message}", ex);
        }
    }

    //Task<IEnumerable<TEntity>> ObterTodos(CancellationToken cancellationToken = default);
    public async Task<IEnumerable<AcessoColaborador>> ObterTodos(CancellationToken cancellationToken = default)
    {
        try
        {
            string query = $"{BaseSelectQuery} ORDER BY data_hora";
            await using var command = await CreateCommandAsync(query, cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var acessos = new List<AcessoColaborador>();
            while (await reader.ReadAsync(cancellationToken))
            {
                acessos.Add(Map(reader));
            }
            return acessos;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_OBTER_TODOS", $"Erro ao obter todos os acessos de colaborador: {ex.Message}", ex);
        }
    }

    //Task<TEntity> Adicionar(TEntity entity, CancellationToken cancellationToken = default);
    public async Task<AcessoColaborador> Adicionar(AcessoColaborador entity, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = FormatInsertQuery("INSERT INTO tb_acesso (pessoa_tipo, pessoa_id, data_hora) VALUES(@PessoaTipo, @PessoaId, @DataHora)");

            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@PessoaTipo", "Colaborador", DbType.String);
            command.AddParameter("@PessoaId", entity.ColaboradorId, DbType.Int32);
            command.AddParameter("@DataHora", entity.DataHora, DbType.Date);
            int id = await command.ExecuteScalarIdAsync("ERRO_ADICIONAR_ACESSO_COLABORADOR", "Falha ao obter ID inserido para o colaborador.", cancellationToken);
            var idProperty = typeof(Entity).GetProperty("Id");
            idProperty?.SetValue(entity, id);
            return entity;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_ADICIONAR_ACESSO_COLABORADOR", $"Erro ao adicionar acesso: {ex.Message}", ex);
        }
    }

    //Task<TEntity> Atualizar(TEntity entity, CancellationToken cancellationToken = default);
    public async Task<AcessoColaborador> Atualizar(AcessoColaborador entity, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = "UPDATE tb_acesso SET pessoa_id = @PessoaId, data_hora = @DataHora WHERE id_acesso = @Id";

            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@Id", entity.Id, DbType.Int32);
            command.AddParameter("@PessoaId", entity.ColaboradorId, DbType.Int32);
            command.AddParameter("@DataHora", entity.DataHora, DbType.Date);

            int rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
            if (rowsAffected == 0)
            {
                throw new InfrastructureException("REGISTRO_NAO_ENCONTRADO", $"Nenhum acesso encontrado com ID {entity.Id} para atualização.");
            }
            return entity;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_ATUALIZAR_ALUNO", $"Erro ao atualizar aluno ID {entity.Id}: {ex.Message}", ex);
        }
    }
    //Task<bool> Remover(int id, CancellationToken cancellationToken = default);
    public async Task<bool> Remover(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = "DELETE FROM tb_acesso WHERE id_acesso = @Id";
            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@Id", id, DbType.Int32);
            var result = await command.ExecuteNonQueryAsync(cancellationToken);
            return result > 0;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_REMOVER_ACESSO", $"Erro ao remover acesso ID {id}: {ex.Message}", ex);
        }
    }

    // METODOS DA INTERFACE IAcessoColaboradorRepository

    //Task<IEnumerable<AcessoColaborador>> ObterAcessosPorColaboradorPeriodo(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null, CancellationToken cancellationToken = default);
   
    public async Task<IEnumerable<AcessoColaborador>> ObterAcessosPorColaboradorPeriodo(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null, CancellationToken cancellationToken = default)
    {
        try
        {
            
            string query = $"{BaseSelectQuery} WHERE pessoa_id = @ColaboradorId AND data_hora >= @Inicio AND data_hora <= @Fim ORDER BY data_hora ASC";
            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@ColaboradorId", colaboradorId, DbType.Int32);
            command.AddParameter("@Inicio", inicio, DbType.Date);
            command.AddParameter("@Fim", fim, DbType.Date);
            
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var acessos = new List<AcessoColaborador>();
            while (await reader.ReadAsync(cancellationToken))
            {
                acessos.Add(Map(reader));
            }
            return acessos;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_OBTER_ACESSOS_POR_COLABORADOR_PERIODO", $"Erro ao obter acessos de colaboradores entre {inicio} e {fim}: {ex.Message}", ex);
        }
    }
    //Task<AcessoColaborador?> ObterUltimoAcesso(int colaboradorId, CancellationToken cancellationToken = default);
    public async Task<AcessoColaborador?> ObterUltimoAcesso(int colaboradorId, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = $"{BaseSelectQuery} WHERE pessoa_id = {colaboradorId} ORDER BY data_hora DESC LIMIT 1";
            await using var command = await CreateCommandAsync(query, cancellationToken);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            
            return await reader.ReadAsync(cancellationToken) ? Map(reader) : null;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_OBTER_ULTIMO_ACESSO_COLABORADOR", $"Erro ao obter ultimo acesso do colaborador {colaboradorId}: {ex.Message}", ex);
        }
    }
    //Task<TimeSpan> ObterHorasTrabalhadasNoDia(int colaboradorId, DateOnly data, CancellationToken cancellationToken = default);

    public async Task<TimeSpan> ObterHorasTrabalhadasNoDia(int colaboradorId, DateOnly data, CancellationToken cancellationToken = default)
    {
        try
        {
            // criar lista de acessos, obter intervalo de tempo entre eles

            string inicio = data.ToDateTime(TimeOnly.MinValue).ToString();
            string fim = data.AddDays(1).ToDateTime(TimeOnly.MinValue).ToString();

            string query = $"{BaseSelectQuery} WHERE pessoa_id = @ColaboradorId AND data_hora >= @Inicio AND data_hora < @Fim ORDER BY data_hora ASC";
            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@ColaboradorId", colaboradorId, DbType.Int32);
            command.AddParameter("@Inicio", inicio, DbType.DateTime);
            command.AddParameter("@Fim", fim, DbType.DateTime);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var acessos = new List<AcessoColaborador>();

            while (await reader.ReadAsync(cancellationToken))
            {
                acessos.Add(Map(reader));
            }

            TimeSpan total = TimeSpan.Zero;

            for (int i = 0; i + 1 < acessos.Count ; i += 2)
            {
                TimeSpan intervalo = acessos[i + 1].DataHora - acessos[i].DataHora;
                total += intervalo;
            }

            return total;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_OBTER_HORAS_TRABALHADAS", $"Erro ao obter horas trabalhadas do colaborador {colaboradorId} na data {data}: {ex.Message}", ex);
        }
    }


    public static AcessoColaborador Map(DbDataReader reader)
    {
        try
        {
            int id = reader.GetInt32Value("id_acesso");
            Colaborador colaborador = ColaboradorRepository.Map(reader);
            DateTime dataHora = reader.GetDateTimeValue("dataHora");

            var result = AcessoColaborador.Criar(
            id: id,
            colaborador: colaborador,
            dataHora: dataHora
            );
            if (result.IsFailure)
            {
                throw new InfrastructureException("ERRO_DOMINIO_MAPEAMENTO", $"Erro de domínio ao mapear aluno ID {id}: {string.Join(", ", result.Notifications.Select(n => n.Mensagem))}");
            }
            return result.Value!;
        }
        catch (Exception ex) when (ex is not InfrastructureException)
        {
            throw new InfrastructureException("ERRO_MAPEAMENTO_COLABORADOR", $"Erro ao mapear dados do aluno: {ex.Message}", ex);
        }
    }
}