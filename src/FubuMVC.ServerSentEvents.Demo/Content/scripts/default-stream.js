define('default-stream',
[
  'lib/knockout-2.1.0',
  'util/server-event-aggregator'
],
function (ko, serverEventAggregator) {
  var stream = serverEventAggregator.startStream('/url::DefaultStreamType'),
      viewModel = {
        receivedEvents: ko.observableArray([])
      };

  stream.bind('SimpleStringEvent', function (event) {
    viewModel.receivedEvents.push(event.data);
  });

  ko.applyBindings(viewModel, $('#defaultmethodresults')[0]);
});