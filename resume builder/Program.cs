﻿using resume_builder.cli.commands;
using resume_builder.cli.commands.add;
using resume_builder.cli.commands.get;
using resume_builder.cli.commands.search;
using resume_builder.cli.settings;
using Spectre.Console.Cli;

namespace resume_builder;

public static class Program
{
    private static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(AppConfiguration);
        return app.Run(args);
    }

    //todo: don't like that parent options and arguments are positional; spectre problem
    public static void AppConfiguration(IConfigurator config)
    {
    #if DEBUG
        config.PropagateExceptions();
        config.ValidateExamples();
    #endif

        config.SetApplicationName("resume builder");
        config.SetApplicationVersion("1.0.0");
        config.CaseSensitivity(CaseSensitivity.None);


        //todo: add option if adding an existing entry to edit it
        config.AddBranch<AddCommandSettings>("add", add =>
        {
            add.SetDescription("add new information to job database/bank");
            add.AddCommand<AddJobCommand>("job")
               .WithDescription("add a new job")
               .WithExample("add", "job", "-s", "2022-01-01", "-e", "2026-11-01", "-t", "foreman")
               .WithAlias("j");
            add.AddCommand<AddProfileCommand>("profile")
               .WithDescription("add a new profile")
               .WithAlias("user")
               .WithAlias("u");
            add.AddCommand<AddSkillCommand>("skill")
               .WithDescription("add a new skill")
               .WithExample("add", "skill", "Teamwork",                 "soft")
               .WithExample("add", "skill", "'Psychoanalytic therapy'", "hard")
               .WithAlias("s");
        });
        config.AddBranch<OutputCommandSettings>("get", get =>
        {
            get.SetDescription("get job information from database/bank");
            get.AddCommand<GetSkillCommand>("skill")
               .WithAlias("s")
               .WithAlias("skills");
            get.AddCommand<GetCompanyCommand>("company")
               .WithAlias("c")
               .WithDescription("get company information you've worked for");
            get.AddCommand<GetJobCommand>("job")
               .WithAlias("jobs")
               .WithAlias("j");
        });
        config.AddBranch<OutputCommandSettings>("search", search =>
        {
            search.SetDescription("search for jobs matching search terms");
            search.AddCommand<SearchCompanyCommand>("company")
                  .WithAlias("c")
                  .WithAlias("companies");
            search.AddCommand<SearchSkillCommand>("skill")
                  .WithAlias("s")
                  .WithAlias("skills");
            search.AddCommand<SearchJobCommand>("job")
                  .WithAlias("jobs")
                  .WithAlias("j");
        });

        config.AddCommand<InitCommand>("init")
              .WithDescription("initializes resume database")
              .WithAlias("start");
    }
}