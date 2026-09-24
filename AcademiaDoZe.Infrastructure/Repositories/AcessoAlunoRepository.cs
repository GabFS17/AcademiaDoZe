using AcademiaDoZe.Domain.Entities;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace AcademiaDoZe.Infrastructure.Repositories;

public class AcessoAlunoRepository : BaseRepository, IAcessoAlunoRepository
{
    public AcessoAlunoRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType)
    {
    }
    private static string BaseSelectQuery => @"
    SELECT
    id_acesso, pessoa_tipo, pessoa_id, data_hora
    FROM tb_acesso";

    public static AcessoAluno Map(DbDataReader reader)
    {
        try
        {
            int id = reader.GetInt32Value("id_acesso");
            Aluno aluno = AlunoRepository.Map(reader);
            DateTime dataHora = reader.GetDateTimeValue("dataHora");

            var result = AcessoAluno.Criar(
            id: id,
            aluno: aluno,
            dataHora: dataHora
            );
            if (result.IsFailure)
            {
                throw new InfrastructureException("ERRO_DOMINIO_MAPEAMENTO", $"Erro de domínio ao mapear acesso ID {id}: {string.Join(", ", result.Notifications.Select(n => n.Mensagem))}");
            }
            return result.Value!;
        }
        catch (Exception ex) when (ex is not InfrastructureException)
        {
            throw new InfrastructureException("ERRO_MAPEAMENTO_ACESSO", $"Erro ao mapear dados do acesso: {ex.Message}", ex);
        }
    }
    // MÉTODOS BASE IRepository

    //Task<TEntity?> ObterPorId(int id, CancellationToken cancellationToken = default);
    public async Task<AcessoAluno?> ObterPorId(int id, CancellationToken cancellationToken = default)
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
            throw new InfrastructureException("ERRO_OBTER_POR_ID", $"Erro ao obter acesso por ID {id}: {ex.Message}", ex);
        }
    }

    //Task<IEnumerable<TEntity>> ObterTodos(CancellationToken cancellationToken = default);
    public async Task<IEnumerable<AcessoAluno>> ObterTodos(CancellationToken cancellationToken = default)
    {
        try
        {
            string query = $"{BaseSelectQuery} ORDER BY data_hora";
            await using var command = await CreateCommandAsync(query, cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var acessos = new List<AcessoAluno>();
            while (await reader.ReadAsync(cancellationToken))
            {
                acessos.Add(Map(reader));
            }
            return acessos;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_OBTER_TODOS", $"Erro ao obter todos os acessos de alunos: {ex.Message}", ex);
        }
    }

    //Task<TEntity> Adicionar(TEntity entity, CancellationToken cancellationToken = default);
    public async Task<AcessoAluno> Adicionar(AcessoAluno entity, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = FormatInsertQuery("INSERT INTO tb_acesso (pessoa_tipo, pessoa_id, data_hora) VALUES(@PessoaTipo, @PessoaId, @DataHora)");

            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@PessoaTipo", "Aluno", DbType.String);
            command.AddParameter("@PessoaId", entity.AlunoId, DbType.Int32);
            command.AddParameter("@DataHora", entity.DataHora, DbType.Date);
            int id = await command.ExecuteScalarIdAsync("ERRO_ADICIONAR_ACESSO_ALUNO", "Falha ao obter ID inserido para o aluno.", cancellationToken);
            var idProperty = typeof(Entity).GetProperty("Id");
            idProperty?.SetValue(entity, id);
            return entity;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_ADICIONAR_ACESSO_ALUNO", $"Erro ao adicionar acesso: {ex.Message}", ex);
        }
    }

    //Task<TEntity> Atualizar(TEntity entity, CancellationToken cancellationToken = default);
    public async Task<AcessoAluno> Atualizar(AcessoAluno entity, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = "UPDATE tb_acesso SET pessoa_id = @PessoaId, data_hora = @DataHora WHERE id_acesso = @Id";

            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@Id", entity.Id, DbType.Int32);
            command.AddParameter("@PessoaId", entity.AlunoId, DbType.Int32);
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
            throw new InfrastructureException("ERRO_ATUALIZAR_ACESSO", $"Erro ao atualizar acesso ID {entity.Id}: {ex.Message}", ex);
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

    // MÉTODOS DE INTERFACE
    public async Task<IEnumerable<AcessoAluno>> ObterAcessosPorAlunoPeriodo(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null, CancellationToken cancellationToken = default)
    {
        try
        {

            string query = $"{BaseSelectQuery} WHERE pessoa_id = @AlunoId AND data_hora >= @Inicio AND data_hora <= @Fim ORDER BY data_hora ASC";
            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@AlunoId", alunoId, DbType.Int32);
            command.AddParameter("@Inicio", inicio, DbType.Date);
            command.AddParameter("@Fim", fim, DbType.Date);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var acessos = new List<AcessoAluno>();
            while (await reader.ReadAsync(cancellationToken))
            {
                acessos.Add(Map(reader));
            }
            return acessos;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_OBTER_ACESSOS_POR_ALUNO_PERIODO", $"Erro ao obter acessos de alunos entre {inicio} e {fim}: {ex.Message}", ex);
        }
    }
    public async Task<AcessoAluno?> ObterUltimoAcesso(int alunoId, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = $"{BaseSelectQuery} WHERE pessoa_id = @AlunoId ORDER BY data_hora DESC LIMIT 1";
            await using var command = await CreateCommandAsync(query, cancellationToken);

            command.AddParameter("@AlunoId", alunoId, DbType.Int32);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            return await reader.ReadAsync(cancellationToken) ? Map(reader) : null;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException("ERRO_OBTER_ULTIMO_ACESSO_ALUNO", $"Erro ao obter ultimo acesso do aluno {alunoId}: {ex.Message}", ex);
        }
    }

    // infelizmente passei esses pro GPT por falta de tempo:
    public async Task<bool> EstaNaAcademia(int alunoId, CancellationToken cancellationToken = default)
    {
        try
        {
            string query = @"
            SELECT COUNT(*)
            FROM tb_acesso
            WHERE pessoa_tipo = 'Aluno'
              AND pessoa_id = @AlunoId";

            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@AlunoId", alunoId, DbType.Int32);

            object? result = await command.ExecuteScalarAsync(cancellationToken);
            int quantidadeAcessos = Convert.ToInt32(result);

            return quantidadeAcessos % 2 != 0;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException(
                "ERRO_VERIFICAR_ALUNO_NA_ACADEMIA",
                $"Erro ao verificar se o aluno {alunoId} está na academia: {ex.Message}",
                ex);
        }
    }
    // horário mensal de maior procura, baseado na entrada, por exemplo, em dezembro o horário de maior procura é entre 18h e 20h
    // Retorna um dicionário onde a chave é o horário e o valor é a quantidade de acessos nesse horário
    public async Task<Dictionary<TimeOnly, int>> ObterHorarioMaisProcuradoPorMes(int mes, CancellationToken cancellationToken = default)
    {
        if (mes < 1 || mes > 12)
            throw new ArgumentOutOfRangeException(nameof(mes), "O mês deve estar entre 1 e 12.");

        try
        {
            string query = @"
            SELECT pessoa_id, data_hora
            FROM tb_acesso
            WHERE pessoa_tipo = 'Aluno'
            ORDER BY pessoa_id ASC, data_hora ASC";

            await using var command = await CreateCommandAsync(query, cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var acessosPorAluno = new Dictionary<int, List<DateTime>>();

            while (await reader.ReadAsync(cancellationToken))
            {
                int alunoId = reader.GetInt32Value("pessoa_id");
                DateTime dataHora = reader.GetDateTimeValue("data_hora");

                if (!acessosPorAluno.ContainsKey(alunoId))
                    acessosPorAluno[alunoId] = new List<DateTime>();

                acessosPorAluno[alunoId].Add(dataHora);
            }

            var horarios = new Dictionary<TimeOnly, int>();

            foreach (var acessos in acessosPorAluno.Values)
            {
                for (int i = 0; i < acessos.Count; i += 2)
                {
                    DateTime entrada = acessos[i];

                    if (entrada.Month != mes)
                        continue;

                    TimeOnly horario = new TimeOnly(entrada.Hour, 0);

                    if (horarios.ContainsKey(horario))
                        horarios[horario]++;
                    else
                        horarios[horario] = 1;
                }
            }

            return horarios;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException(
                "ERRO_OBTER_HORARIO_MAIS_PROCURADO",
                $"Erro ao obter horário mais procurado no mês {mes}: {ex.Message}",
                ex);
        }
    }
    // Permanência média dos alunos na academia, mensal.
    // retorna um dicionário onde a chave é o mês e o valor é a média de permanência dos alunos nesse mês
    public async Task<Dictionary<int, TimeSpan>> ObterPermanenciaMediaPorMes(int mes, CancellationToken cancellationToken = default)
    {
        if (mes < 1 || mes > 12)
            throw new ArgumentOutOfRangeException(nameof(mes), "O mês deve estar entre 1 e 12.");

        try
        {
            string query = @"
            SELECT pessoa_id, data_hora
            FROM tb_acesso
            WHERE pessoa_tipo = 'Aluno'
            ORDER BY pessoa_id ASC, data_hora ASC";

            await using var command = await CreateCommandAsync(query, cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var acessosPorAluno = new Dictionary<int, List<DateTime>>();

            while (await reader.ReadAsync(cancellationToken))
            {
                int alunoId = reader.GetInt32Value("pessoa_id");
                DateTime dataHora = reader.GetDateTimeValue("data_hora");

                if (!acessosPorAluno.ContainsKey(alunoId))
                    acessosPorAluno[alunoId] = new List<DateTime>();

                acessosPorAluno[alunoId].Add(dataHora);
            }

            TimeSpan totalPermanencia = TimeSpan.Zero;
            int quantidadePermanencias = 0;

            foreach (var acessos in acessosPorAluno.Values)
            {
                for (int i = 0; i + 1 < acessos.Count; i += 2)
                {
                    DateTime entrada = acessos[i];
                    DateTime saida = acessos[i + 1];

                    if (entrada.Month != mes || saida.Month != mes)
                        continue;

                    totalPermanencia += saida - entrada;
                    quantidadePermanencias++;
                }
            }

            var resultado = new Dictionary<int, TimeSpan>();

            if (quantidadePermanencias > 0)
            {
                TimeSpan media = TimeSpan.FromTicks(
                    totalPermanencia.Ticks / quantidadePermanencias);

                resultado[mes] = media;
            }

            return resultado;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException(
                "ERRO_OBTER_PERMANENCIA_MEDIA",
                $"Erro ao obter permanência média no mês {mes}: {ex.Message}",
                ex);
        }
    }
    // alunos que não registraram acesso nos últimos x dias
    public async Task<IEnumerable<Aluno>> ObterAlunosSemAcessoNosUltimosDias(int dias, CancellationToken cancellationToken = default)
    {
        if (dias < 0)
            throw new ArgumentOutOfRangeException(nameof(dias), "A quantidade de dias não pode ser negativa.");

        try
        {
            DateTime dataLimite = DateTime.Now.AddDays(-dias);

            string query = @"
            SELECT a.*
            FROM tb_aluno a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM tb_acesso ac
                WHERE ac.pessoa_tipo = 'Aluno'
                  AND ac.pessoa_id = a.id_aluno
                  AND ac.data_hora >= @DataLimite
            )
            ORDER BY a.id_aluno";

            await using var command = await CreateCommandAsync(query, cancellationToken);
            command.AddParameter("@DataLimite", dataLimite, DbType.DateTime);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var alunos = new List<Aluno>();

            while (await reader.ReadAsync(cancellationToken))
            {
                alunos.Add(AlunoRepository.Map(reader));
            }

            return alunos;
        }
        catch (DbException ex)
        {
            throw new InfrastructureException(
                "ERRO_OBTER_ALUNOS_SEM_ACESSO",
                $"Erro ao obter alunos sem acesso nos últimos {dias} dias: {ex.Message}",
                ex);
        }
    }
}