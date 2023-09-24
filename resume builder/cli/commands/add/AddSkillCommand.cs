using System.Diagnostics.CodeAnalysis;
using resume_builder.models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace resume_builder.cli.commands.add;

public class AddSkillCommand : Command<AddSkillSettings>
{
	public override int Execute([NotNull] CommandContext context, [NotNull] AddSkillSettings settings)
	{
		var skill = new Skill(settings.Skill, settings.SkillType);
		Database database = new();
		database.AddSkill(skill);
		if(skill.Type == null)
			AnsiConsole.MarkupLine($"✅ Skill \"[bold]{skill.Name}[/]\" added");
		else
			AnsiConsole.MarkupLine($"✅ [bold]{skill.Type}[/] Skill \"[bold]{skill.Name}[/]\" added");
		return ExitCode.Success.ToInt();
	}
}

public class AddSkillSettings : CommandSettings
{
	[CommandArgument(0, "<skill>")] public string Skill { get; set; }

	//todo: allows for shortcuts: so you don't have to write out the whole type's name
	//ex. s => soft
	[CommandOption("-t|--type")] public SkillType? SkillType { get; set; }

	public override ValidationResult Validate()
	{
		if(string.IsNullOrWhiteSpace(Skill))
			return ValidationResult.Error(Skill == null ? "Skill is required" : "Skill cannot be empty");
		return ValidationResult.Success();
	}
}