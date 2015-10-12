## Samples

### dispatch event in routine

```c#
var p = new Project { StartDate = startDate };
this._dispatcher.OnNext<ProjectEvent, ProjectCreatedEventArgument>(new ProjectCreatedEventArgument
{
    Subject = new ProjectEvent {Project = p},
    Created = DateTime.UtcNow
});
```

### dispatch events when transaction done
```c#
using (this._dispatcher.CreateContext())
{
    var projectServices = new ProjectServices(this._dispatcher);

    var project = projectServices.Create(DateTime.UtcNow);

    projectServices.Remove(project);
}
```

### subscribe in action
```c#
this._dispatcher.Subscribe(new ActionEventHandler<ProjectEvent, ProjectCreatedEventArgument>(
    e =>
    {
        ++this._counter;
    }));
```

### subscribe as named priority handler
```c#
this._dispatcher.Subscribe(new LowPriorityEventHandler());
this._dispatcher.Subscribe(new HightPriorityEventHandler());
```

### sample of priority handler

```c#
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
```
