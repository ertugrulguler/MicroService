using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Search.Model
{
    public class SearchResponse
    {
        public string senderName { get; set; }
        public string userId { get; set; }
        public string session_id { get; set; }
        public string referrer { get; set; }
        public string response_type { get; set; }
        public List<Message> message { get; set; }
        public bool conversationStep { get; set; }
        public string category { get; set; }
        public string category_name { get; set; }
        public List<string> rates { get; set; }
        public Entities entities { get; set; }
        public Intent intent { get; set; }
        public string fixed_message { get; set; }
        public int isFollowUp { get; set; }
        public string speak { get; set; }
        public ServiceOutput serviceOutput { get; set; }
        public object survey { get; set; }
        public object domain_id { get; set; }
        public object domain_name { get; set; }
        public object live_chat { get; set; }
        public bool live_bot { get; set; }
        public bool threshold_pass { get; set; }
        public List<SelectedCategory> selected_categories { get; set; }
        public string unique_id { get; set; }
        public List<object> timing_response { get; set; }
        public bool survey_feedback { get; set; }
    }
    public class Message
    {
        public string message_type { get; set; }
        public string message { get; set; }
    }

    public class Entities
    {
    }

    public class Intent
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Res
    {
        public string ID_match { get; set; }
    }

    public class ServiceOutput
    {
        public List<Res> res { get; set; }
        public List<string> redirect { get; set; }
        public Cbot cbot { get; set; }
    }

    public class SelectedCategory
    {
        public string intent_id { get; set; }
        public string intent_name { get; set; }
    }

    public class Cbot
    {
        public string name { get; set; }
        public string type { get; set; }
        public int deeplinkId { get; set; }
        public string deeplinkUrl { get; set; }
    }


    /*
     {
        "session_id": 12345,
        "senderName": "Bot",
        "userId": "654321",
        "referrer": "WIDGET",
        "response_type": "MESSAGE | REQUEST | FUNCTION | FUSION",
        "message": [
        ref: TEXT,
        ref: BUTTON_COMPONENT,
        ref: GENERIC_COMPONENT,
        ref: LIST_COMPONENT,
        ref: IMAGE_COMPONENT,
        ref: VIDEO_COMPONENT,
        ref: PREVIEW_COMPONENT,
        ref: LOCATION_COMPONENT
        ],
        "conversationStep": false,
        "nodes": [
        "string"
        ],
        "category": "1a2a1",
        "category_name": "Karta Para Transferi",
        "rates": [
            "50",
            "100"
        ],
        "entities": {
            "entity_name": [
                "string"
            ],
            "entity_name2": [
                {
                    "key1": "value1",
                    "key2": "value2"
                }
            ]
        },
        "intent": {
                "id": "1",
                "name": "Para Transferi"
        },
        "fixed_message": "kredi kartı ücretleri",
        "isFollowUp": 0,
        "speak": "Bilgi almak istediğiniz kredi türünü seçiniz.",
        "ri_id": "5dea20a766e3cd4bf0a76315",
        "ri_unique_id": "no",
        "ri_name": "no",
        "serviceOutput": {},
        "ri_all_mapped": [
             {
                "id": 0,
                "unique_id": "12"
             }
        ],
        "survey": null,
        "live_chat": false,
        "live_bot": false,
        "possibleEntities": null,
        "labels": null,
        "branches": [
            {
                "category_id": "1a2",
                "title": "Category title",
                "name": "Category name",
                "rate": 23,
                "rates": [],
                "totalRate": 55.35
            }
        ]
        }
     
     */

}
