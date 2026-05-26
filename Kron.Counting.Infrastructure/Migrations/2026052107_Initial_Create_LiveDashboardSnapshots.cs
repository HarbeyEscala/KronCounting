using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(2026052107)]
public sealed class Initial_Create_LiveDashboardSnapshots : Migration
{
    private const string Schema = "dbo";
    private const string TableName = "LiveDashboardSnapshots";

    public override void Up()
    {
        Create.Table(TableName).InSchema(Schema)

            .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()

            .WithColumn("StoreId").AsGuid().NotNullable()

            .WithColumn("CurrentOccupancy").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("TodayIn").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("TodayOut").AsInt32().NotNullable().WithDefaultValue(0)

            .WithColumn("LastReadingAtUtc").AsDateTime2().Nullable()

            .WithColumn("UpdatedAtUtc")
                .AsDateTime2()
                .NotNullable()
                .WithDefault(SystemMethods.CurrentUTCDateTime);

        Create.ForeignKey("FK_LiveDashboardSnapshots_Stores")
            .FromTable(TableName).InSchema(Schema).ForeignColumn("StoreId")
            .ToTable("Stores").InSchema(Schema).PrimaryColumn("Id");

        Execute.Sql("""
            CREATE UNIQUE INDEX UX_LiveDashboardSnapshots_StoreId
            ON dbo.LiveDashboardSnapshots(StoreId);
        """);
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_LiveDashboardSnapshots_Stores").OnTable(TableName).InSchema(Schema);

        Execute.Sql("DROP INDEX UX_LiveDashboardSnapshots_StoreId ON dbo.LiveDashboardSnapshots;");

        Delete.Table(TableName).InSchema(Schema);
    }
}