using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(2026052104)]
public sealed class Initial_Create_Stores : Migration
{
    private const string Schema = "dbo";
    private const string TableName = "Stores";

    public override void Up()
    {
        Create.Table(TableName).InSchema(Schema)

            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()

            .WithColumn("TenantId").AsGuid().NotNullable()

            .WithColumn("Code").AsString(50).NotNullable()

            .WithColumn("Name").AsString(200).NotNullable()

            .WithColumn("Description").AsString(500).Nullable()

            .WithColumn("StoreType").AsString(50).Nullable()

            .WithColumn("Region").AsString(100).Nullable()

            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)

            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("CreatedAtUtc")
                .AsDateTime2()
                .NotNullable()
                .WithDefault(SystemMethods.CurrentUTCDateTime)

            .WithColumn("UpdatedAtUtc").AsDateTime2().Nullable()

            .WithColumn("DeletedAtUtc").AsDateTime2().Nullable();

        Create.ForeignKey("FK_Stores_Tenants_TenantId")
            .FromTable(TableName).InSchema(Schema).ForeignColumn("TenantId")
            .ToTable("Tenants").InSchema(Schema).PrimaryColumn("Id");

        Create.Index("IX_Stores_TenantId").OnTable(TableName).InSchema(Schema).OnColumn("TenantId");
        Create.Index("IX_Stores_IsActive").OnTable(TableName).InSchema(Schema).OnColumn("IsActive");
        Create.Index("IX_Stores_IsDeleted").OnTable(TableName).InSchema(Schema).OnColumn("IsDeleted");

        Execute.Sql("""
            CREATE UNIQUE INDEX UX_Stores_TenantId_Code
            ON dbo.Stores(TenantId, Code)
            WHERE IsDeleted = 0;
        """);

        EnableTemporal(TableName, "StoresHistory");
    }

    public override void Down()
    {
        DisableTemporal(TableName);

        Delete.ForeignKey("FK_Stores_Tenants_TenantId").OnTable(TableName).InSchema(Schema);

        Delete.Index("IX_Stores_TenantId").OnTable(TableName).InSchema(Schema);
        Delete.Index("IX_Stores_IsActive").OnTable(TableName).InSchema(Schema);
        Delete.Index("IX_Stores_IsDeleted").OnTable(TableName).InSchema(Schema);

        Execute.Sql("DROP INDEX UX_Stores_TenantId_Code ON dbo.Stores;");

        Delete.Table(TableName).InSchema(Schema);

        Execute.Sql("DROP TABLE dbo.StoresHistory;");
    }

    private void EnableTemporal(string table, string historyTable)
    {
        Execute.Sql($"""
            ALTER TABLE dbo.{table}
            ADD
                ValidFrom DATETIME2(7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL
                    CONSTRAINT DF_{table}_ValidFrom DEFAULT SYSUTCDATETIME(),
                ValidTo DATETIME2(7) GENERATED ALWAYS AS ROW END HIDDEN NOT NULL
                    CONSTRAINT DF_{table}_ValidTo DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),
                PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo);

            ALTER TABLE dbo.{table}
            SET
            (
                SYSTEM_VERSIONING = ON
                (
                    HISTORY_TABLE = dbo.{historyTable},
                    DATA_CONSISTENCY_CHECK = ON
                )
            );
        """);
    }

    private void DisableTemporal(string table)
    {
        Execute.Sql($"ALTER TABLE dbo.{table} SET (SYSTEM_VERSIONING = OFF);");
    }
}