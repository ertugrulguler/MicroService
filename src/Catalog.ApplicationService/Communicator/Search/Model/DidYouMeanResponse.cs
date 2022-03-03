using System.Collections.Generic;

namespace Catalog.ApplicationService.Communicator.Search.Model
{
    public class DidYouMeanResponse
    {
        public int Session_Id { get; set; }
        public string SenderName { get; set; }
        public string UserId { get; set; }
        public string Referrer { get; set; }
        public string Response_Type { get; set; }
        public List<MessageData> Messages { get; set; }
        public bool ConversationStep { get; set; }
        public string Category { get; set; }
        public string Category_Name { get; set; }
        public List<string> Rates { get; set; }
        public EntitiesData Entities { get; set; }
        public IntentData Intent { get; set; }
        public string Fixed_Message { get; set; }
        public int IsFollowUp { get; set; }
        public string Speak { get; set; }
        public List<ServiceOutputData> ServiceOutput { get; set; }
        public string Survey { get; set; } //null
        public string Domain_Id { get; set; } //null
        public string Domain_Name { get; set; } //null
        public bool Live_Bot { get; set; }
        public SelectedCategoriesData Selected_Categories { get; set; }//null
        public string Uniq_Id { get; set; }
        public List<string> Timing_Response { get; set; }
        public bool Survey_Feedback { get; set; }

    }

    public class EntitiesData
    {
        public List<string> Entity_Name { get; set; }
        public List<KeyValuePair<string, string>> Entity_Name2 { get; set; }
    }

    public class IntentData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class RiAllMappedData
    {
        public int Id { get; set; }
        public string Uniq_Id { get; set; }
    }
    public class BranchesData
    {
        public string Category_Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
        public List<string> Rates { get; set; }
        public double totalRate { get; set; }
    }
    public class MessageData
    {
        public string Message_Type { get; set; }
        public string Message { get; set; }
    }
    public class SelectedCategoriesData
    {
        public string Intent_Id { get; set; }
        public string Intent_Name { get; set; }
    }
    public class ServiceData
    {
        public List<IdMatchData> Res { get; set; }
    }
    public class IdMatchData
    {
        public string ID_match { get; set; }
    }
    public class ServiceOutputData
    {
        public string name { get; set; }
        public string type { get; set; }
        public string id { get; set; }
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
