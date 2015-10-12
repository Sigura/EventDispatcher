namespace Dispatcher.Tests.Mocks
{
    using System;
    using Contracts;

    internal class ProjectServices
    {
        private readonly IEventDispatcher _dispatcher;

        public ProjectServices(IEventDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        internal Project Create(DateTime startDate)
        {
            var p = new Project { StartDate = startDate };
            this._dispatcher.OnNext<ProjectEvent, ProjectCreatedEventArgument>(new ProjectCreatedEventArgument
            {
                Subject = new ProjectEvent {Project = p},
                Created = DateTime.UtcNow
            });
            return p;
        }

        internal void Remove(Project project)
        {
            this._dispatcher.OnNext<ProjectEvent, ProjectRemovedEventArgument>(new ProjectRemovedEventArgument
            {
                Subject = new ProjectEvent { Project = project },
                Removed = DateTime.UtcNow
            });
        }
    }
}