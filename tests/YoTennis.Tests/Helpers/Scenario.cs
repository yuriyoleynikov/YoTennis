using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using YoTennis.Models;
using YoTennis.Models.Events;

namespace YoTennis.Tests.Helpers
{
    class Scenario
    {
        private JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
        };
        private JsonSerializer Serializer = new JsonSerializer { Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() } };

        private GameHandler _handler = new GameHandler();
        private StringBuilder _result = new StringBuilder();
        private DateTime _nextTime = new DateTime(1986, 09, 26);

        public Scenario()
        {
            _result.Append("Initial state ");
            DumpState();
            _result.AppendLine();
        }

        private void DumpState()
        {
            var state = JsonConvert.SerializeObject(_handler.CurrentState, JsonSettings);
            _result.Append(state);
        }

        private void DumpChange(JToken prevState, JToken nextState, string indent = "")
        {
            if (JToken.DeepEquals(prevState, nextState))
            {
                _result.Append("<no changes>");
            }
            else if (prevState.Type == JTokenType.Object && nextState.Type == JTokenType.Object)
            {
                DumpObjectDiff((JObject)prevState, (JObject)nextState, indent + "  ");
            }
            else
            {
                _result.Append($"{JsonConvert.SerializeObject(prevState, JsonSettings)} -> {JsonConvert.SerializeObject(nextState, JsonSettings)}");
            }
        }

        private void DumpObjectDiff(JObject prevObject, JObject nextObject, string indent = "")
        {
            _result.AppendLine("{");
            foreach (var prevProperty in prevObject)
            {
                var nextProperty = nextObject.Property(prevProperty.Key);
                if (!JToken.DeepEquals(prevProperty.Value, nextProperty.Value))
                {
                    _result.Append($"{indent}\"{prevProperty.Key}\": ");
                    DumpChange(prevProperty.Value, nextProperty.Value, indent);
                    _result.AppendLine(",");
                }
            }
            _result.Append("}");
        }

        public void AddEvent(GameEvent e)
        {
            if (e.OccuredAt == default(DateTime))
                e.OccuredAt = _nextTime;
            _nextTime = e.OccuredAt.AddMinutes(1);

            _result.AppendLine();
            _result.AppendLine($"Event {e.GetType().Name} {JsonConvert.SerializeObject(e, JsonSettings)}");
            var prevState = JToken.FromObject(_handler.CurrentState, Serializer);

            try
            {
                _handler.AddEvent(e);
            }
            catch (Exception ex)
            {
                _result.AppendLine($"Exception {ex.Message}");
            }

            var nextState = JToken.FromObject(_handler.CurrentState, Serializer);
            _result.Append("State ");
            DumpChange(prevState, nextState);
            _result.AppendLine();
        }

        public override string ToString() => _result.ToString();

        public void Verify([CallerFilePath] string filepath = null, [CallerMemberName] string membername = null)
        {
            Approvals.Verify(this.ToString(), filepath, membername);
        }
    }
}
