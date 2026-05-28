using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(202605271100)]
public sealed class Add_Device_ApiKey : Migration
{
    public override void Up()
    {
        Alter.Table("Devices")
            .AddColumn("ApiKey")
            .AsString(200)
            .NotNullable()
            .WithDefault(SystemMethods.NewGuid);

        Create.Index("IX_Devices_ApiKey")
            .OnTable("Devices")
            .OnColumn("ApiKey")
            .Unique();
    }

    public override void Down()
    {
        Delete.Index("IX_Devices_ApiKey")
            .OnTable("Devices");

        Delete.Column("ApiKey")
            .FromTable("Devices");
    }
}