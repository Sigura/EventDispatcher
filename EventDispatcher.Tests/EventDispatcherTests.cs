namespace Dispatcher.Tests
{
    using Mocks;
    using System;
    using Contracts;
    using NUnit.Framework;

    [TestFixture]
    public class EventDispatcherTests
    {
        private int _counter = 0;
        private IEventDispatcher _dispatcher;
        private static int _order = 0;

        [Test]
        public void Should_Work_Event_Dispatcher_Test()
        {
            _order = -1;
            this.SampleOfRutine();

            Assert.AreEqual(this._counter, 2);
        }

        private void SampleOfRutine()
        {
            var projectServices = new ProjectServices(this._dispatcher);

            var project = projectServices.Create(DateTime.UtcNow);

            projectServices.Remove(project);
        }

        [Test]
        public void Should_Work_Event_Dispatcher_Context_Test()
        {
            using (this._dispatcher.CreateContext())
            {
                this.SampleOfRutine();

                Assert.AreEqual(this._counter, 0);
                Assert.AreEqual(_order, 0);
            }


            Assert.AreEqual(this._counter, 2);
            Assert.AreEqual(_order, 2);
        }

        [TestFixtureSetUp]
        public void Ctor()
        {
            this._dispatcher = new EventDispatcher();

            this._dispatcher.Subscribe(new ActionEventHandler<ProjectEvent, ProjectCreatedEventArgument>(
                e =>
                {
                    Assert.IsNotNull(e.Subject.Project);
                    ++this._counter;
                }));

            this._dispatcher.Subscribe(new Action<ProjectRemovedEventArgument>(
                e =>
                {
                    Assert.IsNotNull(e.Subject.Project);

                    ++this._counter;
                }));

            this._dispatcher.Subscribe(new LowPriorityEventHandler());
            this._dispatcher.Subscribe(new HightPriorityEventHandler());
        }

        [SetUp]
        public void Setup()
        {
            _order = this._counter = 0;
        }

        internal class HightPriorityEventHandler: Contracts.EventHandler<ProjectRemovedEventArgument>, IPriorityEventHandler
        {
            public Priority Priority => Priority.High;

            public override void OnNext(ProjectRemovedEventArgument eventArgument)
            {
                if (_order == -1)
                    return;
                Assert.AreEqual(_order, 0);

                ++_order;
            }
        }

        internal class LowPriorityEventHandler : Contracts.EventHandler<ProjectCreatedEventArgument>, IPriorityEventHandler
        {
            public Priority Priority => Priority.Low;

            public override void OnNext(ProjectCreatedEventArgument eventArgument)
            {
                if (_order == -1)
                    return;

                Assert.AreNotEqual(_order, 0);

                ++_order;
            }
        }
    }
}
