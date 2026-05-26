using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(2026052108)]
public sealed class Initial_Create_StoreHourlyMetrics : Migration
{
    private const string Schema = "dbo";
    private const string TableName = "StoreHourlyMetrics";

    public override void Up()
    {
        Create.Table(TableName).InSchema(Schema)

            .WithColumn("Id").AsInt64().PrimaryKey().Identity()

            .WithColumn("StoreId").AsGuid().NotNullable()

            .WithColumn("MetricDate").AsDate().NotNullable()

            .WithColumn("MetricHour").AsByte().NotNullable()

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

        Create.ForeignKey("FK_StoreHourlyMetrics_Stores")
            .FromTable(TableName).InSchema(Schema).ForeignColumn("StoreId")
            .ToTable("Stores").InSchema(Schema).PrimaryColumn("Id");

        Execute.Sql("""
            CREATE UNIQUE INDEX UX_StoreHourlyMetrics_StoreId_MetricDate_MetricHour
            ON dbo.StoreHourlyMetrics(StoreId, MetricDate, MetricHour);
        """);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_StoreHourlyMetrics_Stores").OnTable(TableName).InSchema(Schema);

        Execute.Sql("DROP INDEX UX_StoreHourlyMetrics_StoreId_MetricDate_MetricHour ON dbo.StoreHourlyMetrics;");

        Delete.Table(TableName).InSchema(Schema);
    }
}