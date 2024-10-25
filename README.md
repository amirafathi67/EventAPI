# Events

Event API that fetches events from Third-party integrated with 2 API 

* secure using JWT , generated Token will be called and provided in the swagger UI header 
* Integration with Ticket Master third-party API to get event list upon specific search criteria
* Integrated with Ayshare to post event on social media (currently configured linked in only)
* using local storage as json file 

## Ticket Master API  (Event)

 * Details:
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

 * Details:
  1.  Retrieving the Records from local storage (Events.json)
  2.  Saving in EventEntity Model


## Ayshare API  (Social Media)

 * Details:
  1.  PostEvent will take the EventId, and description that you want to post it on social media currently I integrated with linkedIn only
  2.  Pass the generated eventID from the local storage (events.json)

 **Example** of payload

```js
{
  "eventID": "c023f08b-cf64-4090-807f-7e0ec57944b3",
  "postDescription": "this event is about using Machine learning in Medicine"
}
```
