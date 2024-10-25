# Events

Event API that fetches events from Third-party integrated with 2 API 
providing the generated Token for easy use 

## Ticket Master API  (Event)

 * Methods:
  1.  FetchEvents(SearchQuery eventSearch) depends on different types of search criteria
  2.  Save the result in Local storage (Events.json) file 

 **Example** of payload

```js
{
  "search": [
    {
      "type": "countryCode",
      "value": "IE"
    },
 {
      "type": "keyword",
      "value": "Music"
    }
  ],
  "size": "10"
}
```
## GetAllEvent

 * Methods:
  1.  Retrieving the Records from local storage (Events.json)
  2.  Saving in EventEntity Model


## Ayshare API  (Social Media)

 * Methods:
  1.  PostEvent will take the EventId, and description that you want to post it on social media currently I integrated with linkedIn only
  2.  Pass the generated eventID from the local storage (events.json)

 **Example** of payload

```js
{
  "eventID": "c023f08b-cf64-4090-807f-7e0ec57944b3",
  "postDescription": "this event is about using Machine learning in Medicine"
}
```
