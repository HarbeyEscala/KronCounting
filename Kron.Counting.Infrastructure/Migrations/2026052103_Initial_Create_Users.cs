using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(2026052103)]
public sealed class Initial_Create_Users : Migration
{
    private const string Schema = "dbo";
    private const string TableName = "Users";

    public override void Up()
    {
        Create.Table(TableName).InSchema(Schema)

            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()

            .WithColumn("TenantId").AsGuid().NotNullable()

            .WithColumn("Email").AsString(256).NotNullable()

            .WithColumn("FirstName").AsString(100).NotNullable()

            .WithColumn("LastName").AsString(100).NotNullable()

            .WithColumn("PasswordHash").AsString(500).NotNullable()

            .WithColumn("Role").AsString(50).NotNullable()

            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)

            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("LastLoginUtc").AsDateTime2().Nullable()

            .WithColumn("CreatedAtUtc")
                .AsDateTime2()
                .NotNullable()
                .WithDefault(SystemMethods.CurrentUTCDateTime)

            .WithColumn("UpdatedAtUtc").AsDateTime2().Nullable()

            .WithColumn("DeletedAtUtc").AsDateTime2().Nullable();

        Create.ForeignKey("FK_Users_Tenants_TenantId")
            .FromTable(TableName).InSchema(Schema).ForeignColumn("TenantId")
            .ToTable("Tenants").InSchema(Schema).PrimaryColumn("Id");

        Create.Index("IX_Users_TenantId").OnTable(TableName).InSchema(Schema).OnColumn("TenantId");
        Create.Index("IX_Users_IsActive").OnTable(TableName).InSchema(Schema).OnColumn("IsActive");
        Create.Index("IX_Users_IsDeleted").OnTable(TableName).InSchema(Schema).OnColumn("IsDeleted");

        Execute.Sql("""
            CREATE UNIQUE INDEX UX_Users_TenantId_Email
            ON dbo.Users(TenantId, Email)
            WHERE IsDeleted = 0;
        """);

        EnableTemporal(TableName, "UsersHistory");
    }

    public override void Down()
    {
        DisableTemporal(TableName);

        Delete.ForeignKey("FK_Users_Tenants_TenantId").OnTable(TableName).InSchema(Schema);

        Delete.Index("IX_Users_TenantId").OnTable(TableName).InSchema(Schema);
        Delete.Index("IX_Users_IsActive").OnTable(TableName).InSchema(Schema);
        Delete.Index("IX_Users_IsDeleted").OnTable(TableName).InSchema(Schema);

        Execute.Sql("DROP INDEX UX_Users_TenantId_Email ON dbo.Users;");

        Delete.Table(TableName).InSchema(Schema);

        Execute.Sql("DROP TABLE dbo.UsersHistory;");
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