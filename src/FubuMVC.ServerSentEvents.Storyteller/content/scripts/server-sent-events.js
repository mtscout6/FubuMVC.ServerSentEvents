function simpleEventPost(event, element) {
  var $this = $(element);
  event.preventDefault();

  $.ajax({
    type: 'post',
    url: 'simple-event',
    data: {
      EventId: $this.siblings('.event-id').val(),
      Data: $this.siblings('.event-data').val()
    }
  });
}

var simpleEventSource = new EventSource("_events/simple");

simpleEventSource.onmessage = function(event) {
  $('#receivedSimpleEvents').append('<div>Event Id: <span class="event-id">' + event.lastEventId + '</span> Raw Event Data: <span class="event-data">' + event.data + '</span></div>');
};



//var source = new EventSource("demo_sse.php");
//source.onmessage = function (event) {
//  document.getElementById("result").innerHTML += event.data + "<br>";
//};