using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(2026052102)]
public sealed class Initial_Create_Tenants : Migration
{
    private const string Schema = "dbo";
    private const string TableName = "Tenants";

    public override void Up()
    {
        Create.Table(TableName).InSchema(Schema)

            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()

            .WithColumn("BrandId").AsGuid().NotNullable()

            .WithColumn("Code").AsString(50).NotNullable()

            .WithColumn("Name").AsString(200).NotNullable()

            .WithColumn("TimeZone").AsString(100).NotNullable()

            .WithColumn("Currency").AsString(10).NotNullable()

            .WithColumn("Locale").AsString(20).NotNullable()

            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)

            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("CreatedAtUtc")
                .AsDateTime2()
                .NotNullable()
                .WithDefault(SystemMethods.CurrentUTCDateTime)

            .WithColumn("UpdatedAtUtc").AsDateTime2().Nullable()

            .WithColumn("DeletedAtUtc").AsDateTime2().Nullable();

        Create.ForeignKey("FK_Tenants_Brands_BrandId")
            .FromTable(TableName).InSchema(Schema).ForeignColumn("BrandId")
            .ToTable("Brands").InSchema(Schema).PrimaryColumn("Id");

        Create.Index("IX_Tenants_BrandId").OnTable(TableName).InSchema(Schema).OnColumn("BrandId");
        Create.Index("IX_Tenants_IsActive").OnTable(TableName).InSchema(Schema).OnColumn("IsActive");
        Create.Index("IX_Tenants_IsDeleted").OnTable(TableName).InSchema(Schema).OnColumn("IsDeleted");

        Execute.Sql("""
            CREATE UNIQUE INDEX UX_Tenants_BrandId_Code
            ON dbo.Tenants(BrandId, Code)
            WHERE IsDeleted = 0;
        """);

        EnableTemporal(TableName, "TenantsHistory");
    }

    public override void Down()
    {
        DisableTemporal(TableName);

        Delete.ForeignKey("FK_Tenants_Brands_BrandId").OnTable(TableName).InSchema(Schema);

        Delete.Index("IX_Tenants_BrandId").OnTable(TableName).InSchema(Schema);
        Delete.Index("IX_Tenants_IsActive").OnTable(TableName).InSchema(Schema);
        Delete.Index("IX_Tenants_IsDeleted").OnTable(TableName).InSchema(Schema);

        Execute.Sql("DROP INDEX UX_Tenants_BrandId_Code ON dbo.Tenants;");

        Delete.Table(TableName).InSchema(Schema);

        Execute.Sql("DROP TABLE dbo.TenantsHistory;");
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