using FluentMigrator;

namespace Task4Database.Migrations
{
    [Migration(202410131)]
    public class InitDatabase : Migration
    {
        public override void Up()
        {
            Create.Table("junior")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable();

            Create.Table("teamlead")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable();

            Create.Table("hackaton")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("harmony").AsDouble().Nullable();

            Create.Table("junior_preference")
                .WithColumn("hackaton_id").AsInt32().NotNullable().ForeignKey("hackaton", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("junior_id").AsInt32().NotNullable().ForeignKey("junior", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("teamlead_id").AsInt32().NotNullable().ForeignKey("teamlead", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("priority").AsInt32().NotNullable();
            Create.PrimaryKey("junior_preference_pk").OnTable("junior_preference").Columns(["hackaton_id", "junior_id", "teamlead_id"]);
            
            Create.Table("teamlead_preference")
                .WithColumn("hackaton_id").AsInt32().NotNullable().ForeignKey("hackaton", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("teamlead_id").AsInt32().NotNullable().ForeignKey("teamlead", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("junior_id").AsInt32().NotNullable().ForeignKey("junior", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("priority").AsInt32().NotNullable();
            Create.PrimaryKey("teamlead_preference_pk").OnTable("teamlead_preference").Columns(["hackaton_id", "teamlead_id", "junior_id"]);

            Create.Table("team")
                .WithColumn("hackaton_id").AsInt32().NotNullable().ForeignKey("hackaton", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("junior_id").AsInt32().NotNullable().ForeignKey("junior", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
                .WithColumn("teamlead_id").AsInt32().NotNullable().ForeignKey("teamlead", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade);
            Create.PrimaryKey("team_pk").OnTable("team").Columns(["hackaton_id", "junior_id", "teamlead_id"]);
        }

        public override void Down()
        {
        }
    }
}
