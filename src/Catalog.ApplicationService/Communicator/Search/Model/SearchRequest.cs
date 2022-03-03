using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Search.Model
{
    public class SearchRequest
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Referrer { get; set; }
        public bool Entity_Search { get; set; }
        public List<string> Entities { get; set; }
        public string Intent_Id { get; set; }
        public bool Response_Intent_Search { get; set; }
        public string Response_Intent_Filter { get; set; }
        public bool Is_List { get; set; }
        public string Session_Id { get; set; }
        public string PresetCategory { get; set; }

    }

    /* {
        "userId": "6457707",
        "message":"user message",
        "referrer": "WIDGET",
        "entity_search": true,
        "entities": ["CARD_TYPE"],
        "intent_id": "1a1"
        "response_intent_search": true,
        "response_intent_filter": "0,1,2",
        "is_list": true,
        "session_id":"54321",
        "presetCategory ":"INTENT_NAME"
        }
     */
}
