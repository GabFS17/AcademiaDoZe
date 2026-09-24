using System;
using System.Collections.Generic;
using System.Text;

using AcademiaDoZe.Infrastructure.Data;
namespace AcademiaDoZe.Application.DependencyInjection;

public class RepositoryConfig
{
    public required string ConnectionString { get; set; }
    public required DatabaseType DatabaseType { get; set; }
}