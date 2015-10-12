namespace Dispatcher.Tests.Mocks
{
    using System;
    using Contracts;

    internal class ProjectEvent : IEvent<ProjectCreatedEventArgument>, IEvent<ProjectRemovedEventArgument>
    {
        public Project Project { get; set; }
    }

    internal class Project
    {
        public DateTime? StartDate { get; set; }
    }
}