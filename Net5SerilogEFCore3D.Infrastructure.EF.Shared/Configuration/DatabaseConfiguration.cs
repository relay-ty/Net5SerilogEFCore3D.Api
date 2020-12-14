using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Infrastructure.EF.Shared.Configuration
{
    public  class  DatabaseConfiguration
    {
        public string DbContextName { get; set; }
        public int Sort { get; set; }
        public bool Enabled { get; set; }
        public DatabaseProviderType ProviderType { get; set; } = DatabaseProviderType.SqlServer;
        public string ConnectionString { get; set; }
        public bool ApplyDatabaseMigrations { get; set; } = false;//应用数据库迁移
        public bool ApplyDataSeed { get; set; } = false;//应用数据源 暂无用
        public string DataSeedJsonFile { get; set; }//数据源 Json 文件
    }
}
