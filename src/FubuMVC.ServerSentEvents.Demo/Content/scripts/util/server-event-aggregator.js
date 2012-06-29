define('util/server-event-aggregator',
[
  'jquery',
  'util/underscore',
  'util/event-aggregator'
],
function($, _, ea) {
  var eventRgx = /.*\/(.*)/,
      registry = { },
      buildStream = function(url, data) {
        var source,
            queryData = data,
            stream = ea.makeAggregator({
              suspend: function () {
                source.close();
                source = undefined;
              },
              changeData: function (newData) {
                queryData = $.extend(queryData, newData);
                if (source) {
                  stream.suspend();
                  stream.resume();
                }
              },
              resume: function () {
                if (source) {
                  throw 'EventSource is currently open, failed to open a new stream.';
                }

                source = new EventSource(url + (queryData ? ('?' + $.param(queryData)) : ''));

                source.onmessage = function(rawdata) {
                  var parsedData = JSON.parse(rawdata.data),
                      event = eventRgx.test(rawdata.lastEventId)
                        ? eventRgx.exec(rawdata.lastEventId)[1]
                        : 'message';

                  console.log('[' + (new Date().toString()) + '] Received Server-Sent Event: ' + event);
                  
                  stream.trigger({
                    type: event,
                    data: parsedData
                  });
                };

                source.onopen = function(rawData) {
                  stream.trigger('onopen', rawData);
                };

                source.onerror = function(rawData) {
                  stream.trigger('onerror', rawData);
                };
              }
            });

        return stream;
      },
      exports = {
        startStream: function (url) {
          var stream = this.getStream(url);
          stream.resume();
          return stream;
        },
        getStream: function (url) {
          return registry[url]
            ? registry[url]
            : (registry[url] = buildStream(url));
        },
        removeStream: function(url) {
          if (!registry[url]) { return; }
          registry[url].suspend();
          delete registry[url];
        }
      };

  return exports;
});
