using Catalog.ApiContract.Response.Query.SearchQueries;
using Framework.Core.Model;
using MediatR;

namespace Catalog.ApiContract.Request.Query.SearchQueries
{
    public class DidYouMeanQuery : IRequest<ResponseBase<DidYouMeanDetail>>
    {
        public string Message { get; set; }

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
