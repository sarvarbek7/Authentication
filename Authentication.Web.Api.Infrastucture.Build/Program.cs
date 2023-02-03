// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Collections.Generic;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace Authentication.Web.Api.Infrastucture.Build
{
    class Program
    {
        static void Main(string[] args)
        {
            var aDotNetClient = new ADotNetClient();

            var githubPipeline = new GithubPipeline
            {
                Name = "Authentication Web Api Pipeline",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] {"main"}
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] {"main"}
                    }
                },

                Jobs = new Jobs
                {
                    Build = new BuildJob
                    {
                        RunsOn = BuildMachines.Windows2022,

                        Steps = new List<GithubTask>
                        {
                            new CheckoutTaskV2
                            {
                                Name = "Checking Out"
                            },

                            new SetupDotNetTaskV1
                            {
                                Name = "Installing .NET",

                                TargetDotNetVersion = new TargetDotNetVersion
                                {
                                    DotNetVersion = "7.0.102",
                                    IncludePrerelease= true
                                }
                            },

                            new RestoreTask
                            {
                                Name = "Restoring Nuget Packages"
                            },

                            new DotNetBuildTask
                            {
                                Name = "Building Project"
                            },

                            new TestTask
                            {
                                Name = "Running Tests"
                            }
                        }
                    }
                }
            };

            aDotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: "../../.github/workflows/dotnet.yml"
                );
        }
    }
}