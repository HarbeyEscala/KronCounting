using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(2026052109)]
public sealed class Initial_Create_StoreDailyMetrics : Migration
{
    private const string Schema = "dbo";
    private const string TableName = "StoreDailyMetrics";

    public override void Up()
    {
        Create.Table(TableName).InSchema(Schema)

            .WithColumn("Id").AsInt64().PrimaryKey().Identity()

            .WithColumn("StoreId").AsGuid().NotNullable()

            .WithColumn("MetricDate").AsDate().NotNullable()

            .WithColumn("PeopleIn").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("PeopleOut").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("PeakOccupancy").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("AvgOccupancy").AsDecimal(10, 2).NotNullable().WithDefaultValue(0)

            .WithColumn("CreatedAtUtc")
                .AsDateTime2()
                .NotNullable()
                .WithDefault(SystemMethods.CurrentUTCDateTime)

            .WithColumn("UpdatedAtUtc")
                .AsDateTime2()
                .Nullable();

        Create.ForeignKey("FK_StoreDailyMetrics_Stores")
            .FromTable(TableName).InSchema(Schema).ForeignColumn("StoreId")
            .ToTable("Stores").InSchema(Schema).PrimaryColumn("Id");

        Execute.Sql("""
            CREATE UNIQUE INDEX UX_StoreDailyMetrics_StoreId_MetricDate
            ON dbo.StoreDailyMetrics(StoreId, MetricDate);
        """);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_StoreDailyMetrics_Stores").OnTable(TableName).InSchema(Schema);

        Execute.Sql("DROP INDEX UX_StoreDailyMetrics_StoreId_MetricDate ON dbo.StoreDailyMetrics;");

        Delete.Table(TableName).InSchema(Schema);
    }
}