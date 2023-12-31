using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using resume_builder.cli.settings;
using resume_builder.models;
using resume_builder.models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace resume_builder.cli.commands.search;

public class SearchCompanyCommand : Command<SearchCompanyCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SearchCompanyCommandSettings settings)
    {
        ResumeContext database = new();
        var companies = from company in database.Companies
                        where company.Name.Contains(settings.Name)
                        select company.Name;
        if(!companies.Any())
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

public class SearchCompanyCommandSettings : OutputCommandSettings
{
    [CommandArgument(0, "[name]")]
    [Description("The name of the company")]
    [DisplayName("Company Name")]
    public string? Name { get; set; }
}