using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(202605271430)]
public sealed class Remove_ConfidenceScore_From_DeviceReadings : Migration
{
    public override void Up()
    {
        Execute.Sql("""
            ALTER TABLE dbo.DeviceReadings
            DROP CONSTRAINT CK_DeviceReadings_ConfidenceScore;
        """);

        Delete.Column("ConfidenceScore")
            .FromTable("DeviceReadings")
            .InSchema("dbo");
    }
    public override void Down()
    {
        Alter.Table("DeviceReadings")
            .InSchema("dbo")
            .AddColumn("ConfidenceScore")
            .AsDecimal(5, 2)
            .Nullable();
    }
}