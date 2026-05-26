using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(2026052105)]
public sealed class Initial_Create_Devices : Migration
{
    private const string Schema = "dbo";
    private const string TableName = "Devices";

    public override void Up()
    {
        Create.Table(TableName).InSchema(Schema)

            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()

            .WithColumn("StoreId").AsGuid().NotNullable()

            .WithColumn("SerialNumber").AsString(100).NotNullable()

            .WithColumn("Name").AsString(150).NotNullable()

            .WithColumn("DeviceType").AsString(50).NotNullable()

            .WithColumn("FirmwareVersion").AsString(50).Nullable()

            .WithColumn("LastSeenAtUtc").AsDateTime2().Nullable()

            .WithColumn("IsOnline").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)

            .WithColumn("IsDeleted").AsBoolean().NotNullable().WithDefaultValue(false)

            .WithColumn("CreatedAtUtc")
                .AsDateTime2()
                .NotNullable()
                .WithDefault(SystemMethods.CurrentUTCDateTime)

            .WithColumn("UpdatedAtUtc")
                .AsDateTime2()
                .Nullable()

            .WithColumn("DeletedAtUtc")
                .AsDateTime2()
                .Nullable();

        Create.ForeignKey("FK_Devices_Stores")
            .FromTable(TableName).InSchema(Schema).ForeignColumn("StoreId")
            .ToTable("Stores").InSchema(Schema).PrimaryColumn("Id");

        Create.Index("IX_Devices_StoreId")
            .OnTable(TableName)
            .InSchema(Schema)
            .OnColumn("StoreId");

        Create.Index("IX_Devices_IsActive")
            .OnTable(TableName)
            .InSchema(Schema)
            .OnColumn("IsActive");

        Create.Index("IX_Devices_IsDeleted")
            .OnTable(TableName)
            .InSchema(Schema)
            .OnColumn("IsDeleted");

        Execute.Sql("""
            CREATE UNIQUE INDEX UX_Devices_StoreId_SerialNumber
            ON dbo.Devices(StoreId, SerialNumber)
            WHERE IsDeleted = 0;
        """);

        EnableTemporal(TableName, "DevicesHistory");
    }

    public override void Down()
    {
        DisableTemporal(TableName);

        Delete.ForeignKey("FK_Devices_Stores").OnTable(TableName).InSchema(Schema);

        Delete.Index("IX_Devices_StoreId").OnTable(TableName).InSchema(Schema);
        Delete.Index("IX_Devices_IsActive").OnTable(TableName).InSchema(Schema);
        Delete.Index("IX_Devices_IsDeleted").OnTable(TableName).InSchema(Schema);

        Execute.Sql("DROP INDEX UX_Devices_StoreId_SerialNumber ON dbo.Devices;");

        Delete.Table(TableName).InSchema(Schema);

        Execute.Sql("DROP TABLE dbo.DevicesHistory;");
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
        Execute.Sql($"""
            ALTER TABLE dbo.{table}
            SET (SYSTEM_VERSIONING = OFF);
        """);
    }
}