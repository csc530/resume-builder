using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using resume_builder.models;
using resume_builder.models.database;
using Spectre.Console;
using Spectre.Console.Cli;
using static resume_builder.Globals;

namespace resume_builder.cli.commands.add;

//todo: add interactive mode
public class AddSkillCommand : Command<AddSkillSettings>
{
	public override int Execute([NotNull] CommandContext context, [NotNull] AddSkillSettings settings)
	{
		var skillName = settings.Skill;
		var skillType = settings.SkillType;
		if(settings.Interactive || skillName.IsBlank() || skillType == null)
		{
			skillName = AnsiConsole.Ask<string>("Skill: ");
			skillType = new SelectionPrompt<SkillType>()
			            .Title("Skill Type")
			            .AddChoices(Enum.GetValues<SkillType>())
			            .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
			            .WrapAround()
			            .Show(AnsiConsole.Console);
		}

		try
		{
			var skill = new Skill(skillName, skillType.Value);
			Database database = new();
			database.AddSkill(skill);
			AnsiConsole.MarkupLine($"""✅ [bold]{skill.Type}[/] Skill "[bold]{skill.Name}[/]" added""");
			return ExitCode.Success.ToInt();
		}
		catch(Exception e)
		{
			return PrintError(settings, e);
		}
	}
}

public class AddSkillSettings : AddCommandSettings
{
	[CommandArgument(0, "[skill]")]
	[Description("The name, abbreviation, or short description of the skill")]
	public string? Skill { get; set; }

	//todo: allows for shortcuts: so you don't have to write out the whole type's name
	//ex. s => soft
	[CommandArgument(1, "[type]")]
	[Description("The type of skill: soft, hard, etc.")]
	[DefaultValue(models.SkillType.Soft)]
	public SkillType? SkillType { get; set; }
}