using System.Diagnostics.CodeAnalysis;
using resume_builder.cli.settings;
using resume_builder.models;
using resume_builder.models.database;
using Spectre.Console;
using Spectre.Console.Cli;

namespace resume_builder.cli.commands.get;

public class GetCompanyCommand : Command<OutputCommandSettings>
{
	public override int Execute([NotNull] CommandContext context, [NotNull] OutputCommandSettings settings)
	{
		Database database = new();
		var companies = database.GetCompanies();
		if(companies.Count == 0)
			AnsiConsole.MarkupLine("No companies found");
		else
		{
			var table = settings.GetTable();
			if(table == null)
				foreach(var company in companies)
					AnsiConsole.WriteLine(company);
			else
			{
				table.AddTableColumn("Company Name");
				foreach(var company in companies)
					table.AddRow(company);
				AnsiConsole.Write(table);
			}
		}

		return ExitCode.Success.ToInt();
	}
}