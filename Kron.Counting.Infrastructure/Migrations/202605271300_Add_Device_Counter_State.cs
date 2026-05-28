using FluentMigrator;

namespace Kron.Counting.Infrastructure.Migrations;

[Migration(202605271300)]
public sealed class Add_Device_Counter_State : Migration
{
    public override void Up()
    {
        Alter.Table("Devices")
            .AddColumn("LastTotalIn")
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(0);

        Alter.Table("Devices")
            .AddColumn("LastTotalOut")
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Column("LastTotalIn")
            .FromTable("Devices");

        Delete.Column("LastTotalOut")
            .FromTable("Devices");
    }
}