using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(2026052106)]
public sealed class Initial_Create_DeviceReadings : Migration
{
    private const string Schema = "dbo";
    private const string TableName = "DeviceReadings";

    public override void Up()
    {
        Create.Table(TableName).InSchema(Schema)

            .WithColumn("Id").AsInt64().PrimaryKey().Identity()

            .WithColumn("DeviceId").AsGuid().NotNullable()

            .WithColumn("ReadingTimestampUtc").AsDateTime2().NotNullable()

            .WithColumn("PeopleIn").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("PeopleOut").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("Occupancy").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("ConfidenceScore").AsDecimal(5, 2).Nullable()

            .WithColumn("RawPayloadJson").AsCustom("NVARCHAR(MAX)").Nullable()

            .WithColumn("CreatedAtUtc")
                .AsDateTime2()
                .NotNullable()
                .WithDefault(SystemMethods.CurrentUTCDateTime);

        Create.ForeignKey("FK_DeviceReadings_Devices")
            .FromTable(TableName).InSchema(Schema).ForeignColumn("DeviceId")
            .ToTable("Devices").InSchema(Schema).PrimaryColumn("Id");

        Create.Index("IX_DeviceReadings_DeviceId")
            .OnTable(TableName)
            .InSchema(Schema)
            .OnColumn("DeviceId");

        Execute.Sql("""
            CREATE INDEX IX_DeviceReadings_DeviceId_ReadingTimestampUtc
            ON dbo.DeviceReadings(DeviceId, ReadingTimestampUtc DESC);
        """);

        Execute.Sql("""
            ALTER TABLE dbo.DeviceReadings
            ADD CONSTRAINT CK_DeviceReadings_PeopleIn CHECK (PeopleIn >= 0);
        """);

        Execute.Sql("""
            ALTER TABLE dbo.DeviceReadings
            ADD CONSTRAINT CK_DeviceReadings_PeopleOut CHECK (PeopleOut >= 0);
        """);

        Execute.Sql("""
            ALTER TABLE dbo.DeviceReadings
            ADD CONSTRAINT CK_DeviceReadings_Occupancy CHECK (Occupancy >= 0);
        """);

        Execute.Sql("""
            ALTER TABLE dbo.DeviceReadings
            ADD CONSTRAINT CK_DeviceReadings_ConfidenceScore
            CHECK (ConfidenceScore IS NULL OR (ConfidenceScore >= 0 AND ConfidenceScore <= 100));
        """);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_DeviceReadings_Devices").OnTable(TableName).InSchema(Schema);

        Delete.Index("IX_DeviceReadings_DeviceId").OnTable(TableName).InSchema(Schema);

        Execute.Sql("DROP INDEX IX_DeviceReadings_DeviceId_ReadingTimestampUtc ON dbo.DeviceReadings;");

        Delete.Table(TableName).InSchema(Schema);
    }
}