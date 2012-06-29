define('state-machine-stream',
[
  'lib/knockout-2.1.0',
  'util/server-event-aggregator'
],
function (ko, serverEventAggregator) {
  var stream = serverEventAggregator.startStream('/url::CurrentStateTopic'),
      viewModel = {
        receivedStates: ko.observableArray([])
      };

  stream.bind('CurrentStateChanged', function (event) {
    var item = _.find(viewModel.receivedStates(), function (s) {
      return s.stateId === event.data.StateId;
    });

    if (item) {
      item.someValue(event.data.SomeValue);
      item.currentName(event.data.CurrentName);
    } else {
      viewModel.receivedStates.push({
        stateId: event.data.StateId,
        someValue: ko.observable(event.data.SomeValue),
        currentName: ko.observable(event.data.CurrentName)
      });
    }
  });

  ko.applyBindings(viewModel, $('#customqueueresults')[0]);
});