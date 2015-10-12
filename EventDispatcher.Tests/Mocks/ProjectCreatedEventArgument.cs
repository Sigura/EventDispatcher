namespace Dispatcher.Tests.Mocks
{
    using System;
    using Contracts;

    internal class ProjectCreatedEventArgument : IEventArgument<ProjectEvent>
    {
        public DateTime Created;
        public ProjectEvent Subject { get; set; }
    }

    internal class ProjectRemovedEventArgument : IEventArgument<ProjectEvent>
    {
        public DateTime Removed;
        public ProjectEvent Subject { get; set; }
    }
}