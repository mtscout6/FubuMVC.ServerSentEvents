define('util/event-aggregator', function() {
  var appendAggregator = function(obj) {
    var registry = { };

    obj.bind = function (type, handler) {
      if (registry.hasOwnProperty(type)) {
        registry[type].push(handler);
      } else {
        registry[type] = [handler];
      }
      return this;
    };
    obj.singleBind = function (type, handler) {
      registry[type] = [handler];

      return this;
    };
    obj.unbind = function (type, handler) {
      var handlers = registry[type],
          newHandlers = [],
          i, h;
      if (handlers) {
        if (handler) {
          for (i in handlers) {
            h = handlers[i];
            if (h === handler) continue;
            newHandlers.push(h);
          }
        }
        registry[type] = newHandlers;
      }
      return this;
    };

    obj.trigger = function (event) {
      var array,
          func,
          handler,
          i, length,
          type = typeof event === 'string' ?
            event : event.type;

      if (registry.hasOwnProperty(type)) {
        array = registry[type];
        for (i = 0, length = array.length; i < length; i++) {
          handler = array[i];
          func = handler;
          func.apply(this, [event]);
        }
      }
      return this;
    };
    return obj;
  };

  return appendAggregator({
    makeAggregator: appendAggregator
  });
});
